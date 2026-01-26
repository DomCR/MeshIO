using CSMath;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using System.Linq;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfNodeBuilder : GltfObjectBuilder<GltfNode>
{
	public Node Node { get; private set; }

	public override void Build(GlbFileBuilder builder)
	{
		base.Build(builder);

		this.Node = new Node(this.GltfObject.Name);

		if (this.GltfObject.Matrix != null)
		{
			//Matrix is organized by columns
			this.Node.Transform = new Transform(new Matrix4(this.GltfObject.Matrix.Select(f => (double)f).ToArray()).Transpose());
		}

		if (this.GltfObject.Translation != null)
		{
			var tranlation = this.GltfObject.Translation.Select(f => (double)f).ToArray();
			this.Node.Transform.Translation = new XYZ(tranlation[0], tranlation[1], tranlation[2]);
		}

		if (this.GltfObject.Rotation != null)
		{
			var rotation = this.GltfObject.Rotation.Select(f => (double)f).ToArray();
			var rot = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
		}

		if (this.GltfObject.Scale != null)
		{
			var scale = this.GltfObject.Scale.Select(f => (double)f).ToArray();
			this.Node.Transform.Scale = new XYZ(scale[0], scale[1], scale[2]);
		}

		if (this.GltfObject.Weights != null)
		{
			builder.Notify($"Weights not implemented for node.", NotificationType.NotImplemented);
		}

		if (builder.TryGetBuilder<GltfMeshBuilder>(this.GltfObject.Mesh, out var mesh))
		{
			this.Node.Entities.AddRange(mesh.Meshes);
			this.Node.Materials.AddRange(mesh.Materials);
		}

		foreach (var meshId in this.GltfObject.Meshes)
		{
			if (builder.TryGetBuilder<GltfMeshBuilder>(meshId, out var m))
			{
				this.Node.Entities.AddRange(m.Meshes);
				this.Node.Materials.AddRange(m.Materials);
			}
		}

		if (builder.TryGetBuilder<GltfCameraBuilder>(this.GltfObject.Camera, out var camera))
		{
			this.Node.Entities.Add(camera.Camera);
		}

		this.GltfObject.Children?.ToList().ForEach((i) =>
		{
			if (builder.TryGetBuilder<GltfNodeBuilder>(i.ToString(), out var c))
			{
				this.Node.Nodes.Add(c.Node);
			}
		});
	}
}
