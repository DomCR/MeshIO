using MeshIO.Entities.Geometries;

namespace MeshIO.FBX.Readers.Templates
{
	internal class FbxMeshTemplate : FbxGeometryTemplate<Mesh>
	{
		public FbxMeshTemplate(FbxNode node) : base(node, new Mesh())
		{
		}
	}
}
