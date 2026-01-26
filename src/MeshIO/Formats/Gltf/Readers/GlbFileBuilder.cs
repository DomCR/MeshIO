using MeshIO.Formats.Gltf.Builders;
using MeshIO.Formats.Gltf.Schema;
using System.Collections.Generic;

namespace MeshIO.Formats.Gltf.Readers;

internal abstract class GlbFileBuilder : IGlbFileBuilder
{
	public event NotificationEventHandler OnNotification;

	protected Dictionary<int, GltfAccessorBuilder> Accessors { get; } = new();

	protected Dictionary<int, GltfCameraBuilder> Cameras { get; } = new();

	protected Dictionary<int, GltfMaterialBuilder> Materials { get; } = new();

	protected Dictionary<int, GltfMeshBuilder> Meshes { get; } = new();

	protected Dictionary<int, GltfNodeBuilder> Nodes { get; } = new();

	protected Dictionary<int, GltfSceneBuilder> Scenes { get; } = new();

	protected readonly GlbHeader header;

	public GlbFileBuilder(GlbHeader header)
	{
		this.header = header;
	}

	public abstract Scene Build();
}