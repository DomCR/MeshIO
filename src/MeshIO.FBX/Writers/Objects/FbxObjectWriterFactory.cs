using MeshIO.Entities.Geometries;
using System;

namespace MeshIO.FBX.Writers.Objects
{
	internal static class FbxObjectWriterFactory
	{
		public static IFbxObjectWriter Create<T>(T element)
			where T : Element3D
		{
			switch (element)
			{
				case Mesh mesh:
					return new FbxMeshWriter(mesh);
				case Node node:
					return new FbxNodeWriter(node);
				default:
					throw new NotImplementedException($"IFbxObjectWriter for {element.GetType()}");
			}
		}
	}
}
