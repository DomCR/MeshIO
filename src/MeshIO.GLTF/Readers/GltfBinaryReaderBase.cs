using CSMath;
using CSUtilities.IO;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.GLTF.Schema.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using MeshIO.Entities.Geometries;
using MeshIO.Shaders;
using MeshIO.Entities;
using MeshIO.GLTF.Exceptions;
using MeshIO.Core;

namespace MeshIO.GLTF
{
	internal abstract class GltfBinaryReaderBase : IDisposable
	{
		public event NotificationEventHandler OnNotification;

		public static GltfBinaryReaderBase GetBynaryReader(int version, GltfRoot root, byte[] chunk)
		{
			switch (version)
			{
				case 2:
					return new GltfBinaryReaderV2(root, chunk);
				case 1:
				default:
					throw new NotSupportedException($"Version {version} not supported");
			}
		}

		protected readonly Dictionary<int, List<Mesh>> _meshMap = new Dictionary<int, List<Mesh>>();
		protected readonly Dictionary<int, Material> _materialMap = new Dictionary<int, Material>();
		protected readonly GltfRoot _root;
		protected StreamIO _chunk;
		protected Dictionary<int, StreamIO> _buffers;   //TODO: implement gltf reader for multiple buffers

		public GltfBinaryReaderBase(GltfRoot root, byte[] chunk)
		{
			this._root = root;
			this._chunk = new StreamIO(chunk);
		}

		public Scene Read()
		{
			GltfScene rootScene = null;

			if (this._root.Scene.HasValue)
			{
				rootScene = this._root.Scenes[this._root.Scene.Value];
			}
			else
			{
				rootScene = this._root.Scenes.FirstOrDefault();
			}

			if (rootScene == null)
				throw new GltfReaderException("Scene not found");

			Scene scene = new Scene(rootScene.Name);

			foreach (int nodeIndex in rootScene.Nodes)
			{
				Node n = readNode(nodeIndex);

				if (n != null)
					scene.RootNode.Nodes.Add(n);
			}

			return scene;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_chunk.Dispose();
			foreach (var item in _buffers.Values)
			{
				item.Dispose();
			}
		}

		protected Node readNode(int index)
		{
			GltfNode gltfNode = this._root.Nodes[index];
			Node node = new Node(gltfNode.Name);

			if (gltfNode.Skin.HasValue)
			{
				this.notify($"Gltf skin found in node {index} | {gltfNode.Name}", NotificationType.NotImplemented);
			}

			if (gltfNode.Matrix != null)
			{
				//Matrix is organized by columns
				node.Transform = new Transform(new Matrix4(gltfNode.Matrix.Select(f => (double)f).ToArray()).Transpose());
			}

			if (gltfNode.Translation != null)
			{
				node.Transform.Translation = new XYZ(gltfNode.Translation.Select(f => (double)f).ToArray());
			}

			if (gltfNode.Rotation != null)
			{
				node.Transform.EulerRotation = new XYZ(gltfNode.Rotation.Select(f => (double)f).ToArray());
			}

			if (gltfNode.Scale != null)
			{
				node.Transform.Scale = new XYZ(gltfNode.Scale.Select(f => (double)f).ToArray());
			}

			if (gltfNode.Weights != null)
			{
				this.notify($"Gltf weights found in node {index} | {gltfNode.Name}", NotificationType.NotImplemented);
			}

			if (gltfNode.Mesh.HasValue)
			{
				node.Entities.AddRange(readPrimitivesInMesh(gltfNode.Mesh.Value, node));
			}

			if (gltfNode.Camera.HasValue)
			{
				node.Entities.Add(readCamera(gltfNode.Camera.Value));
			}

			gltfNode.Children?.ToList().ForEach((i) =>
			{
				Node n = readNode(i);

				if (n != null)
					node.Nodes.Add(n);
			});

			return node;
		}

		protected Camera readCamera(int index)
		{
			GltfCamera gltfCamera = this._root.Cameras[index];
			Camera camera = new Camera(gltfCamera.Name);

			//TODO: implement gltf camera reader
			switch (gltfCamera.Type)
			{
				case GltfCamera.TypeEnum.perspective:
				case GltfCamera.TypeEnum.orthographic:
				default:
					this.notify($"Camera type not identified {gltfCamera.Type}", NotificationType.Warning);
					break;
			}

			return camera;
		}

		protected List<Mesh> readPrimitivesInMesh(int index, Node parent)
		{
			GltfMesh gltfMesh = this._root.Meshes[index];
			List<Mesh> meshes = new List<Mesh>();

			foreach (GltfMeshPrimitive p in gltfMesh.Primitives)
			{
				Mesh mesh = readPrimitive(p, out Material material);
				mesh.Name = gltfMesh.Name;
				meshes.Add(mesh);

				parent.Materials.Add(material);    //TODO: fix the material reference
			}

			return meshes;
		}

		protected Mesh readPrimitive(GltfMeshPrimitive p, out Material material)
		{
			Mesh mesh = new Mesh();

			foreach (KeyValuePair<string, int> att in p.Attributes)
			{
				switch (att.Key)
				{
					case "POSITION":
						mesh.Vertices = this.readXYZ(_root.Accessors[att.Value]);
						break;
					case "NORMAL":
						var normals = new LayerElementNormal();
						normals.Normals = readXYZ(_root.Accessors[att.Value]);
						mesh.Layers.Add(normals);
						break;
					case "TANGENT":
					case "TEXCOORD_0":
					case "TEXCOORD_1":
					case "TEXCOORD_2":
					case "COLOR_0":
					case "JOINTS_0":
					case "WEIGHTS_0":
						this.notify($"Attribute in mesh {att.Key}", NotificationType.NotImplemented);
						break;
					default:
						this.notify($"Attribute in mesh not identified {att.Key}", NotificationType.Warning);
						break;
				}
			}

			if (p.Indices.HasValue)
			{
				mesh.Polygons.AddRange(readIndices(this._root.Accessors[p.Indices.Value], p.Mode));

				//Add missing layers

				//TODO: Fix the gltf normal reading
				//var normals = new LayerElementNormal(mesh);
				//normals.CalculateFlatNormals();
				//mesh.Layers.Add(normals);
			}

			if (p.Material.HasValue)
			{
				material = readMaterial(p.Material.Value);
				mesh.Layers.Add(new LayerElementMaterial());
			}
			else
			{
				material = null;
			}

			return mesh;
		}

		private Material readMaterial(int index)
		{
			if (_materialMap.TryGetValue(index, out Material material))
			{
				return material;
			}

			GltfMaterial gltfMaterial = _root.Materials[index];
			material = new Material(gltfMaterial.Name);
			_materialMap.Add(index, material);

			//TODO: implement gltf material reader
			if (gltfMaterial.PbrMetallicRoughness != null)
			{
				byte r = (byte)(gltfMaterial.PbrMetallicRoughness.BaseColorFactor[0] * 255);
				byte g = (byte)(gltfMaterial.PbrMetallicRoughness.BaseColorFactor[1] * 255);
				byte b = (byte)(gltfMaterial.PbrMetallicRoughness.BaseColorFactor[2] * 255);
				byte a = (byte)(gltfMaterial.PbrMetallicRoughness.BaseColorFactor[3] * 255);

				material.AmbientColor = new Color(r, g, b, a);
				material.DiffuseColor = new Color(r, g, b, a);
				material.SpecularColor = new Color(r, g, b, a);
			}

			return material;
		}

		protected List<XYZ> readXYZ(GltfAccessor accessor)
		{
			if (!accessor.BufferView.HasValue)
				return new List<XYZ>();

			if (accessor.Type != GltfAccessor.TypeEnum.VEC3)
				throw new Exception();

			return readAccessor<XYZ, double>(getBufferStream(accessor), accessor.ComponentType, accessor.Count, 3);
		}

		private IEnumerable<Polygon> readIndices(GltfAccessor accessor, GltfMeshPrimitive.ModeEnum mode)
		{
			if (!accessor.BufferView.HasValue)
				return new List<Polygon>();

			StreamIO stream = getBufferStream(accessor);

			switch (mode)
			{
				case GltfMeshPrimitive.ModeEnum.TRIANGLES:
					//return this.readTriangles(stream, accessor.ComponentType, accessor.Count / 3, 3);
					return readAccessor<Triangle, int>(stream, accessor.ComponentType, accessor.Count / 3, 3);
				case GltfMeshPrimitive.ModeEnum.POINTS:
				case GltfMeshPrimitive.ModeEnum.LINES:  //Works with the 3d lines, like polylines and lines
				case GltfMeshPrimitive.ModeEnum.LINE_LOOP:
				case GltfMeshPrimitive.ModeEnum.LINE_STRIP:
				case GltfMeshPrimitive.ModeEnum.TRIANGLE_STRIP:
				case GltfMeshPrimitive.ModeEnum.TRIANGLE_FAN:
					return new List<Polygon>();
				default:
					throw new NotImplementedException();
			}
		}

		protected IEnumerable<Polygon> readTriangles(StreamIO stream, GltfAccessor.ComponentTypeEnum componentType, int count, int nargs)
		{
			throw new NotImplementedException();
		}

		protected List<T> readAccessor<T, R>(StreamIO stream, GltfAccessor.ComponentTypeEnum componentType, int count, int nargs)
		{
			List<T> vecs = new List<T>();

			for (int i = 0; i < count; i++)
			{
				List<R> args = new List<R>();

				for (int j = 0; j < nargs; j++)
				{
					object value = null;
					switch (componentType)
					{
						case GltfAccessor.ComponentTypeEnum.BYTE:
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_BYTE:
							value = (stream.ReadByte());
							break;
						case GltfAccessor.ComponentTypeEnum.SHORT:
							value = (stream.ReadShort());
							break;
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_SHORT:
							value = (stream.ReadUShort());
							break;
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_INT:
							value = (stream.ReadUInt());
							break;
						case GltfAccessor.ComponentTypeEnum.FLOAT:
							value = stream.ReadSingle();
							break;
						default:
							throw new Exception();
					}

					args.Add((R)Convert.ChangeType(value, typeof(R)));
				}

				vecs.Add((T)Activator.CreateInstance(typeof(T), args.ToArray()));
			}

			return vecs;
		}

		private StreamIO getBufferStream(GltfAccessor accessor)
		{
			GltfBufferView bufferView = _root.BufferViews[accessor.BufferView.Value];
			GltfBuffer buffer = _root.Buffers[bufferView.Buffer];

			StreamIO stream = new StreamIO(_chunk.GetBytes(0, buffer.ByteLength));
			stream.Position = bufferView.ByteOffset + accessor.ByteOffset;

			return stream;
		}

		protected void notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
		}
	}
}
