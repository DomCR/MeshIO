#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif
using MeshIO.Entities.Geometries;

namespace MeshIO.Formats.Fbx.Templates;

internal abstract class FbxGeometryTemplate<T> : FbxObjectTemplate<T>
	where T : Geometry
{
	public override string FbxObjectName
	{
		get
		{
			if (Is6000Fbx)
			{
				return FbxFileToken.Model;
			}
			else
			{
				return FbxFileToken.Geometry;
			}
		}
	}

	protected FbxGeometryTemplate(FbxVersion version, T geometry) : base(version, geometry) { }
}
