using CSMath;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using System.Linq;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfNodeBuilder : GltfObjectBuilder<GltfNode>
{
	public Node Node { get; private set; }

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		GltfNode gltfNode = this.GltfObject;
		Node = new Node(GltfObject.Name);

		if (gltfNode.Skin.HasValue)
		{
		}

		if (gltfNode.Matrix != null)
		{
			//Matrix is organized by columns
			Node.Transform = new Transform(new Matrix4(gltfNode.Matrix.Select(f => (double)f).ToArray()).Transpose());
		}

		if (gltfNode.Translation != null)
		{
			var tranlation = gltfNode.Translation.Select(f => (double)f).ToArray();
			Node.Transform.Translation = new XYZ(tranlation[0], tranlation[1], tranlation[2]);
		}

		if (gltfNode.Rotation != null)
		{
			var rotation = gltfNode.Rotation.Select(f => (double)f).ToArray();
			var rot = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
		}

		if (gltfNode.Scale != null)
		{
			var scale = gltfNode.Scale.Select(f => (double)f).ToArray();
			Node.Transform.Scale = new XYZ(scale[0], scale[1], scale[2]);
		}

		if (gltfNode.Weights != null)
		{
		}

		if (gltfNode.Mesh.HasValue)
		{
			var mesh = builder.GetBuilder<GltfMeshBuilder>(gltfNode.Mesh.Value.ToString());
			Node.Entities.AddRange(mesh.Meshes);
			Node.Materials.AddRange(mesh.Materials);
		}
		foreach (var m in gltfNode.Meshes)
		{
			var mesh = builder.GetBuilder<GltfMeshBuilder>(m);
			Node.Entities.AddRange(mesh.Meshes);
			Node.Materials.AddRange(mesh.Materials);
		}

		if (gltfNode.Camera.HasValue)
		{
			var camera = builder.GetBuilder<GltfCameraBuilder>(gltfNode.Camera.Value.ToString());
			Node.Entities.Add(camera.Camera);
		}

		gltfNode.Children?.ToList().ForEach((i) =>
		{
			var c = builder.GetBuilder<GltfNodeBuilder>(i.ToString());
			Node.Nodes.Add(c.Node);
		});
	}
}
