namespace MeshIO.FBX.Readers.Templates
{
	internal class FbxNodeTemplate : FbxObjectTemplate<Node>
	{
		public override string FbxObjectName { get { return FbxFileToken.Model; } }

		public FbxNodeTemplate(FbxNode node) : base(node, new Node())
		{
		}
	}
}
