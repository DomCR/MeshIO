using System;
using MeshIO.Entities;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;

namespace MeshIO.Formats.Fbx.Templates
{
	internal static class FbxTemplateFactory
	{
		public static IFbxObjectTemplate Create<T>(T element)
			where T : Element3D
		{
			switch (element)
			{
				case Primitive primitive:
					return new FbxMeshTemplate(primitive.ToMesh());
				case Mesh mesh:
					return new FbxMeshTemplate(mesh);
				case Node node:
					return new FbxNodeTemplate(node);
				default:
					throw new NotImplementedException($"{nameof(IFbxObjectTemplate)} for {element.GetType()}");
			}
		}
	}
}
