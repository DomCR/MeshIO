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

	public override void Build(GlbFileBuilder builder)
	{
		base.Build(builder);

		var rootScene = this.GltfObject;
		Scene = new Scene(rootScene.Name);
		foreach (var id in rootScene.Nodes)
		{
			if (builder.TryGetBuilder<GltfNodeBuilder>(id, out var node))
			{
				Scene.RootNode.Nodes.Add(node.Node);
			}
		}
	}
}