using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfSceneBuilder : GltfObjectBuilder<GltfScene>
{
	public Scene Scene { get; private set; }

	public GltfSceneBuilder()
	{ }

	public GltfSceneBuilder(GltfScene scene) : base(scene)
	{
	}

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		var rootScene = this.GltfObject;
		Scene = new Scene(rootScene.Name);
		foreach (var nodeIndex in rootScene.Nodes)
		{
			var node = builder.GetBuilder<GltfNodeBuilder>(nodeIndex);
			Scene.RootNode.Nodes.Add(node.Node);
		}
	}
}