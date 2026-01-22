using MeshIO.Formats.Fbx.Readers;

namespace MeshIO.Formats.Fbx.Builders;

internal class FbxRootNodeBuilder : FbxNodeBuilder
{
	public FbxRootNodeBuilder(Node root) : base(root)
	{
	}

	public override void Build(FbxFileBuilderBase builder)
	{
		//TODO: Set properties from GlobalSettings
		buildChildren(builder);
	}
}
