namespace MeshIO.FBX.Readers.Templates
{
	internal class FbxRootNodeTemplate : FbxNodeTemplate
	{
		public FbxRootNodeTemplate(Node root) : base(root)
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			//TODO: Set properties from GlobalSettings

			this.processChildren(builder);
		}
	}
}
