using MeshIO.Entities.Geometries;

namespace MeshIO.FBX.Readers.Templates
{
	internal abstract class FbxGeometryTemplate<T> : FbxObjectTemplate<T>
		where T : Geometry
	{
		public override string FbxObjectName { get { return FbxFileToken.Geometry; } }

		protected FbxGeometryTemplate(FbxNode node, T geometry) : base(node, geometry)
		{
		}
	}
}
