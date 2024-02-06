using MeshIO.FBX.Readers;

namespace MeshIO.FBX.Templates
{
	internal class FbxRootNodeTemplate : FbxNodeTemplate
	{
		public FbxRootNodeTemplate(Node root) : base(root)
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			//TODO: Set properties from GlobalSettings

			processChildren(builder);
		}
	}
}
