using System;
using MeshIO.Entities;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;

namespace MeshIO.Formats.Fbx.Builders;

internal static class FbxBuilderFactory
{
	public static IFbxObjectBuilder Create<T>(T element)
		where T : Element3D
	{
		switch (element)
		{
			case Primitive primitive:
				return new FbxMeshBuilder(primitive.ToMesh());
			case Mesh mesh:
				return new FbxMeshBuilder(mesh);
			case Node node:
				return new FbxNodeBuilder(node);
			default:
				throw new NotImplementedException($"{nameof(IFbxObjectBuilder)} for {element.GetType()}");
		}
	}
}