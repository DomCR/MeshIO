using MeshIO.Entities.Geometries;
using System;

namespace MeshIO.FBX.Extensions
{
	internal static class Element3DExtensions
	{
		public static string GetFbxName(this Element3D element)
		{
			switch (element)
			{
				case Node:
					return FbxFileToken.Model;
				case Geometry:
					return FbxFileToken.Geometry;
				default:
					throw new NotImplementedException($"Unknown Element3D : {element.GetType().FullName}");
			}
		}
	}
}
