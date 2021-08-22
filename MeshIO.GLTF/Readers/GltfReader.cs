using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Elements;
using MeshIO.Elements.Geometries;
using MeshIO.Elements.Geometries.Layers;
using MeshIO.GLTF.Exceptions;
using MeshIO.GLTF.Schema.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.GLTF
{
	internal abstract class GltfBinaryReaderBase : IDisposable
	{
		public static GltfBinaryReaderBase GetBynaryReader(int version, GltfRoot root, byte[] chunk)
		{
			switch (version)
			{
				case 2:
					return new GltfBinaryReaderV2(root, chunk);
				case 1:
				default:
					throw new NotImplementedException($"Version {version} not implemented");
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
			GltfScene rootScene = this._root.Scenes[this._root.Scene.Value];
			Scene scene = new Scene(rootScene.Name);

			foreach (int nodeIndex in rootScene.Nodes)
			{
				Node n = readNode(nodeIndex);

				if (n != null)
					scene.Nodes.Add(n);
			}

			return scene;
		}

		public void Dispose()
		{

		}

		protected Node readNode(int index)
		{
			GltfNode gltfNode = this._root.Nodes[index];
			Node node = new Node(gltfNode.Name);

			if (gltfNode.Camera.HasValue)
			{
				node.Children.Add(readCamera(gltfNode.Camera.Value));
			}

			gltfNode.Children?.ToList().ForEach((i) =>
			{
				Node n = readNode(i);

				if (n != null)
					node.Children.Add(n);
			});

			if (gltfNode.Skin.HasValue)
			{
				//TODO: read skin
			}

			if (gltfNode.Matrix != null)
			{
				//TODO: Apply transform matrix
				node.Transform = new Transform(new Matrix4(gltfNode.Matrix.Select(f => (double)f).ToArray()));
			}

			if (gltfNode.Rotation != null)
			{
				//TODO: Apply rotation matrix
			}

			if (gltfNode.Scale != null)
			{
				//TODO: Apply scale matrix
			}

			if (gltfNode.Translation != null)
			{
				//TODO: Apply translation matrix
			}

			if (gltfNode.Weights != null)
			{
				//TODO: Apply Weights matrix
			}

			if (gltfNode.Mesh.HasValue)
			{
				node.Children.AddRange(readPrimitivesInMesh(gltfNode.Mesh.Value, node));
			}

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
					break;
				case GltfCamera.TypeEnum.orthographic:
					break;
				default:
					throw new Exception();
			}

			return camera;
		}

		protected Element readElement(int index)
		{
			throw new NotImplementedException();
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

				parent.Children.Add(material);    //TODO: fix the material reference
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
						mesh.Vertices = readXYZ(_root.Accessors[att.Value]);
						break;
					case "NORMAL":
						//TODO: Fix the gltf normal reading
						//var normals = new LayerElementNormal(mesh);
						//normals.Normals = readXYZ(_root.Accessors[att.Value]); //.Select(x => -x).ToList();
						//normals.MappingInformationType = MappingMode.ByPolygon;
						//normals.ReferenceInformationType = ReferenceMode.Direct;
						//mesh.Layers.Add(normals);
						break;
					case "TANGENT":
					case "TEXCOORD_0":
					case "TEXCOORD_1":
					case "COLOR_0":
					case "JOINTS_0":
					case "WEIGHTS_0":
						break;
					default:
						throw new Exception();
				}
			}

			if (p.Indices.HasValue)
			{
				mesh.Polygons.AddRange(readIndices(_root.Accessors[p.Indices.Value], p.Mode));

				//TODO: Fix the gltf normal reading
				//var normals = new LayerElementNormal(mesh);
				//normals.CalculateFlatNormals();
				//mesh.Layers.Add(normals);
			}

			if (p.Material.HasValue)
			{
				material = readMaterial(p.Material.Value);
				mesh.Layers.Add(new LayerElementMaterial(mesh));
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

			return readAccessor<XYZ>(getBufferStream(accessor), accessor.ComponentType, accessor.Count, 3);
		}

		private IEnumerable<Polygon> readIndices(GltfAccessor accessor, GltfMeshPrimitive.ModeEnum mode)
		{
			if (!accessor.BufferView.HasValue)
				return new List<Polygon>();

			StreamIO stream = getBufferStream(accessor);

			switch (mode)
			{
				case GltfMeshPrimitive.ModeEnum.TRIANGLES:
					return readAccessor<Triangle>(stream, accessor.ComponentType, accessor.Count / 3, 3);
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

		protected List<T> readAccessor<T>(StreamIO stream, GltfAccessor.ComponentTypeEnum componentType, int count, int nargs)
		{
			List<T> vecs = new List<T>();

			for (int i = 0; i < count; i++)
			{
				List<object> args = new List<object>();

				for (int j = 0; j < nargs; j++)
				{

					switch (componentType)
					{
						case GltfAccessor.ComponentTypeEnum.BYTE:
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_BYTE:
							args.Add(stream.ReadByte());
							break;
						case GltfAccessor.ComponentTypeEnum.SHORT:
							args.Add(stream.ReadShort());
							break;
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_SHORT:
							args.Add(stream.ReadUShort());
							break;
						case GltfAccessor.ComponentTypeEnum.UNSIGNED_INT:
							args.Add(stream.ReadUInt());
							break;
						case GltfAccessor.ComponentTypeEnum.FLOAT:
							args.Add(stream.ReadSingle());
							break;
						default:
							throw new Exception();
					}
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
	}

	internal class GltfBinaryReaderV2 : GltfBinaryReaderBase
	{
		public GltfBinaryReaderV2(GltfRoot root, byte[] chunk) : base(root, chunk) { }
	}

	public class GltfReader : IDisposable
	{
		private GlbHeader _header;
		private GltfRoot _root;
		private readonly StreamIO _stream;
		private StreamIO _binaryStream;

		public GltfReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new StreamIO(new FileStream(path, FileMode.Open));
		}

		public GltfReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = new StreamIO(stream);
		}

		public Scene Read()
		{
			//The 12-byte header consists of three 4-byte entries:
			_header = new GlbHeader();
			//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
			_header.Magic = _stream.ReadUInt<LittleEndianConverter>();
			//version indicates the version of the Binary glTF container format. This specification defines version 2.
			_header.Version = _stream.ReadUInt<LittleEndianConverter>();
			//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
			_header.Length = _stream.ReadUInt<LittleEndianConverter>();

			if (_header.Version != 2)
				throw new NotImplementedException($"Version {_header.Version} not implemented");

			//Chunk 0 Json
			uint jsonChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string jsonChunkType = _stream.ReadString(4);

			if (jsonChunkType != "JSON")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			_root = JsonConvert.DeserializeObject<GltfRoot>(_stream.ReadString((int)jsonChunkLength));

			//Chunk 1 bin
			uint binChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string binChunkType = _stream.ReadString(4);

			//Check the chunk type
			if (binChunkType != "BIN\0")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			byte[] binChunk = _stream.ReadBytes((int)binChunkLength);
			_binaryStream = new StreamIO(binChunk);

			return GltfBinaryReaderBase.GetBynaryReader((int)_header.Version, _root, binChunk).Read();
		}

		public void Dispose()
		{
			_stream.Dispose();
			_binaryStream.Dispose();
		}
	}

	internal class GlbHeader
	{
		public uint Magic { get; set; }
		public uint Version { get; set; }
		public uint Length { get; set; }
	}
}
