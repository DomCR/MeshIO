using MeshIO.Entities.Geometries;
using MeshIO.Formats.Fbx.Readers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Fbx.Builders;

internal class FbxMeshBuilder : FbxGeometryBuilder<Mesh>
{
	public override string FbxTypeName { get { return FbxFileToken.Mesh; } }

	public FbxMeshBuilder(Mesh mesh) : base(mesh) { }

	public FbxMeshBuilder(FbxNode node) : base(node, new Mesh())
	{
	}

	public override void Build(FbxFileBuilderBase builder)
	{
		base.Build(builder);

		readVertices(builder.Version);
		readPolygons(builder.Version);
		readEdges(builder.Version);
	}

	private void readVertices(FbxVersion version)
	{
		if (FbxNode.TryGetNode("Vertices", out FbxNode vertices))
		{
			_element.Vertices.AddRange(arrToXYZ(arrToDoubleArray(getArrayValue(version, vertices))));
		}
	}

	private void readEdges(FbxVersion version)
	{
		if (FbxNode.TryGetNode("Edges", out FbxNode edges))
		{
			_element.Edges.AddRange(toArr<int>(getArrayValue(version, edges)));
		}
	}

	private void readPolygons(FbxVersion version)
	{
		if (FbxNode.TryGetNode("PolygonVertexIndex", out FbxNode polygons))
		{
			_element.Polygons = mapPolygons(toArr<int>(getArrayValue(version, polygons)).ToArray());
		}
	}

	protected List<Polygon> mapPolygons(int[] arr)
	{
		List<Polygon> polygons = new List<Polygon>();

		if (arr == null)
			return polygons;

		//Check if the arr are faces or quads
		if (arr[2] < 0)
		{
			for (int i = 2; i < arr.Length; i += 3)
			{
				Triangle tmp = new Triangle(
					arr[i - 2],
					arr[i - 1],
					//Substract a unit to the last
					Math.Abs(arr[i]) - 1);

				polygons.Add(tmp);
			}
		}
		//Quads
		else if (arr[3] < 0)
		{
			for (int i = 3; i < arr.Length; i += 4)
			{
				Quad tmp = new Quad(
					Math.Abs(arr[i - 3]),
					Math.Abs(arr[i - 2]),
					Math.Abs(arr[i - 1]),
					//Substract a unit to the last
					Math.Abs(arr[i]) - 1);

				polygons.Add(tmp);
			}
		}

		return polygons;
	}
}
