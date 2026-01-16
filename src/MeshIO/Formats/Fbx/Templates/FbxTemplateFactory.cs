using System;
using MeshIO.Entities.Geometries;

namespace MeshIO.Formats.Fbx.Templates
{
	internal static class FbxTemplateFactory
	{
		public static IFbxObjectTemplate Create<T>(T element)
			where T : Element3D
		{
			switch (element)
			{
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
