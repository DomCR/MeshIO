using MeshIO.Formats.Fbx.Builders;
using MeshIO.Formats.Fbx.Readers;

namespace MeshIO.Formats.Fbx.Templates
{
	internal class FbxRootNodeTemplate : FbxNodeBuilder
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
