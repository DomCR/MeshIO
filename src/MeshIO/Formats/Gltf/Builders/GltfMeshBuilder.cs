using CSMath;
using CSUtilities.IO;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfAccessorBuilder : GltfObjectBuilder<GltfAccessor>
{
	public byte[] Bytes
	{
		get
		{
			return _bytes;
		}
	}

	public double[] Doubles
	{
		get
		{
			return _doubles;
		}
	}

	public float[] Floats
	{
		get
		{
			return _floats;
		}
	}

	public uint[] UInts
	{
		get
		{
			return _uints;
		}
	}

	public short[] Shorts
	{
		get
		{
			return _shorts;
		}
	}

	public ushort[] UShorts
	{
		get
		{
			return _uShorts;
		}
	}

	private byte[] _bytes;

	private double[] _doubles;

	private float[] _floats;

	private int[] _scalars;

	private short[] _shorts;

	private uint[] _uints;

	private ushort[] _uShorts;

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		StreamIO reader = builder.GetBufferStream(this.GltfObject);

		switch (GltfObject.ComponentType)
		{
			case GltfAccessor.ComponentTypeEnum.BYTE:
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_BYTE:
				this.getData(this.GltfObject.Count, ref _bytes, reader.ReadByte);
				this._scalars = this._bytes.Select(b => Convert.ToInt32(b)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.SHORT:
				this.getData(this.GltfObject.Count, ref _shorts, reader.ReadShort);
				this._scalars = this._shorts.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_SHORT:
				this.getData(this.GltfObject.Count, ref _uShorts, reader.ReadUShort);
				this._scalars = this._uShorts.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_INT:
				this.getData(this.GltfObject.Count, ref _uints, reader.ReadUInt);
				this._scalars = this._uints.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.FLOAT:
				this.getData(this.GltfObject.Count, ref _floats, reader.ReadSingle);
				this._doubles = this._floats.Select(f => Convert.ToDouble(f)).ToArray();
				break;
			default:
				throw new NotSupportedException();
		}

		switch (GltfObject.Type)
		{
			case GltfAccessor.TypeEnum.SCALAR:
				break;
			case GltfAccessor.TypeEnum.VEC2:
				break;
			case GltfAccessor.TypeEnum.VEC3:
				break;
			case GltfAccessor.TypeEnum.VEC4:
				break;
			case GltfAccessor.TypeEnum.MAT2:
				break;
			case GltfAccessor.TypeEnum.MAT3:
				break;
			case GltfAccessor.TypeEnum.MAT4:
				break;
			default:
				break;
		}
	}

	public bool TryMapTriangles(out IEnumerable<Triangle> triangles)
	{
		if (this._scalars == null || this._scalars.Length % 3 != 0)
		{
			triangles = null;
			return false;
		}

		var list = new List<Triangle>();
		for (int i = 0; i < this._scalars.Length; i++)
		{
			var i0 = this._scalars[i];
			var i1 = this._scalars[i];
			var i2 = this._scalars[i];

			list.Add(new Triangle(i0, i1, i2));
		}

		triangles = new List<Triangle>(list);
		return true;
	}

	private void getData<T>(int count, ref T[] arr, Func<T> read)
	{
		arr = new T[count];
		for (int i = 0; i < count; i++)
		{
			arr[i] = read.Invoke();
		}
	}
}

internal class GltfMeshBuilder : GltfObjectBuilder<GltfMesh>
{
	public List<Material> Materials { get; } = new();

	public List<Mesh> Meshes { get; } = new();

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		foreach (var p in GltfObject.Primitives)
		{
			Mesh mesh = new Mesh(GltfObject.Name);
			this.Meshes.Add(mesh);

			foreach (var att in p.Attributes)
			{
				var accessor = builder.GetBuilder<GltfAccessorBuilder>(att.Value);

				switch (att.Key)
				{
					case "POSITION":
						if (this.tryMapXYZ(accessor.Doubles, out IEnumerable<XYZ> vertices))
						{
							mesh.Vertices.AddRange(vertices);
						}
						break;
					case "NORMAL":
						if (this.tryMapXYZ(accessor.Doubles, out IEnumerable<XYZ> normals))
						{
							var normalLayer = new LayerElementNormal();
							normalLayer.Normals = new List<XYZ>(normals);
							mesh.Layers.Add(normalLayer);
						}
						break;
					case "TANGENT":
					case "TEXCOORD_0":
					case "TEXCOORD_1":
					case "TEXCOORD_2":
					case "COLOR_0":
					case "JOINTS_0":
					case "WEIGHTS_0":
						//builder.notify($"Attribute in mesh {att.Key}", NotificationType.NotImplemented);
						break;
					default:
						//builder.notify($"Attribute in mesh not identified {att.Key}", NotificationType.Warning);
						break;
				}
			}

			if (p.Indices.HasValue)
			{
				var accessor = builder.GetBuilder<GltfAccessorBuilder>(p.Indices.Value);

				switch (p.Mode)
				{
					case GltfMeshPrimitive.ModeEnum.TRIANGLES:
						if (accessor.TryMapTriangles(out IEnumerable<Triangle> triangles))
						{
							mesh.Polygons.AddRange(triangles);
						}
						break;
					case GltfMeshPrimitive.ModeEnum.POINTS:
					case GltfMeshPrimitive.ModeEnum.LINES:
					case GltfMeshPrimitive.ModeEnum.LINE_LOOP:
					case GltfMeshPrimitive.ModeEnum.LINE_STRIP:
					case GltfMeshPrimitive.ModeEnum.TRIANGLE_STRIP:
					case GltfMeshPrimitive.ModeEnum.TRIANGLE_FAN:
					default:
						throw new NotImplementedException();
				}
			}

			if (p.Material.HasValue)
			{
				var materialBuilder = builder.GetBuilder<GltfMaterialBuilder>(p.Material.Value);
				this.Materials.Add(materialBuilder.Material);

				var layer = new LayerElementMaterial();
				mesh.Layers.Add(layer);
			}
		}
	}

	private bool tryMapXYZ(double[] arr, out IEnumerable<XYZ> points)
	{
		if (arr.Length % 3 != 0)
		{
			points = null;
			return false;
		}

		var list = new List<XYZ>();
		for (int i = 0; i < arr.Length; i++)
		{
			var x = arr[i];
			var y = arr[i];
			var z = arr[i];

			list.Add(new XYZ(x, y, z));
		}

		points = new List<XYZ>(list);
		return true;
	}
}

internal class GltfNodeBuilder : GltfObjectBuilder<GltfNode>
{
	public Node Node { get; private set; }

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		GltfNode gltfNode = this.GltfObject;
		Node = new Node(GltfObject.Name);

		if (gltfNode.Skin.HasValue)
		{
		}

		if (gltfNode.Matrix != null)
		{
			//Matrix is organized by columns
			Node.Transform = new Transform(new Matrix4(gltfNode.Matrix.Select(f => (double)f).ToArray()).Transpose());
		}

		if (gltfNode.Translation != null)
		{
			var tranlation = gltfNode.Translation.Select(f => (double)f).ToArray();
			Node.Transform.Translation = new XYZ(tranlation[0], tranlation[1], tranlation[2]);
		}

		if (gltfNode.Rotation != null)
		{
			var rotation = gltfNode.Rotation.Select(f => (double)f).ToArray();
			var rot = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
		}

		if (gltfNode.Scale != null)
		{
			var scale = gltfNode.Scale.Select(f => (double)f).ToArray();
			Node.Transform.Scale = new XYZ(scale[0], scale[1], scale[2]);
		}

		if (gltfNode.Weights != null)
		{
		}

		if (gltfNode.Mesh.HasValue)
		{
			var mesh = builder.GetBuilder<GltfMeshBuilder>(gltfNode.Mesh.Value);
			Node.Entities.AddRange(mesh.Meshes);
			Node.Materials.AddRange(mesh.Materials);
		}

		if (gltfNode.Camera.HasValue)
		{
			var camera = builder.GetBuilder<GltfCameraBuilder>(gltfNode.Camera.Value);
			Node.Entities.Add(camera.Camera);
		}

		gltfNode.Children?.ToList().ForEach((i) =>
		{
			var c = builder.GetBuilder<GltfNodeBuilder>(i);
			Node.Nodes.Add(c.Node);
		});
	}
}

internal abstract class GltfObjectBuilder<R> : IGltfObjectBuilder
{
	public R GltfObject { get; set; }

	public bool HasBeenBuilt { get; private set; } = false;

	public GltfObjectBuilder()
	{ }

	public GltfObjectBuilder(R gltfObject)
	{
		GltfObject = gltfObject;
	}

	public virtual void Build(GlbV2FileBuilder builder)
	{
		HasBeenBuilt = true;
	}
}

internal class GltfSceneBuilder : GltfObjectBuilder<GltfScene>
{
	public Scene Scene { get; private set; }

	public GltfSceneBuilder()
	{ }

	public GltfSceneBuilder(GltfScene scene) : base(scene)
	{
	}

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		var rootScene = this.GltfObject;
		Scene = new Scene(rootScene.Name);
		foreach (int nodeIndex in rootScene.Nodes)
		{
			var node = builder.GetBuilder<GltfNodeBuilder>(nodeIndex);
			Scene.RootNode.Nodes.Add(node.Node);
		}
	}
}