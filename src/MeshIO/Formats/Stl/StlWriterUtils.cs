using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Entities.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Stl;

internal static class StlWriterUtils
{
	public static IEnumerable<Mesh> ExtractMeshes(Node node)
	{
		List<Mesh> meshes = new List<Mesh>();

		foreach (var n in node.Nodes)
		{
			meshes.AddRange(ExtractMeshes(n));
		}

		foreach (var m in node.Entities.OfType<Mesh>())
		{
			meshes.Add(Prepare(m));
		}

		foreach (var p in node.Entities.OfType<Primitive>())
		{
			meshes.Add(Prepare(p.ToMesh()));
		}

		return meshes;
	}

	public static Mesh Prepare(Mesh mesh)
	{
		if (mesh.Layers.TryGetLayer<LayerElementNormal>(out var layer)
			&& layer.MappingMode == MappingMode.ByPolygon
			&& !mesh.Polygons.Any(p => p is Quad))
		{
			return mesh;
		}

		Mesh m = new Mesh(mesh.Name);
		m.Vertices.AddRange(mesh.Vertices);
		foreach (var item in mesh.Polygons)
		{
			if (item is Quad quad)
			{
				m.Polygons.AddRange(quad.ToTriangles());
			}
			else if (item is Triangle triangle)
			{
				m.Polygons.Add(triangle);
			}
		}

		m.Layers.Add(LayerElementNormal.CreateFlatNormals(m));

		return m;
	}
}