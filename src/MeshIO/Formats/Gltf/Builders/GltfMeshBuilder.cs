using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Shaders;
using System;
using System.Collections.Generic;

namespace MeshIO.Formats.Gltf.Builders;

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
						builder.Notify($"Attribute in mesh {att.Key}", NotificationType.NotImplemented);
						break;
					default:
						builder.Notify($"Attribute in mesh not identified {att.Key}", NotificationType.Warning);
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
