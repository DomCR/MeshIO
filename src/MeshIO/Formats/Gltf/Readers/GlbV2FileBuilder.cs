using CSUtilities.IO;
using MeshIO.Formats.Gltf.Builders;
using MeshIO.Formats.Gltf.Exceptions;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Gltf.Readers;

internal class GlbV2FileBuilder : IGlbFileBuilder
{
	public event NotificationEventHandler OnNotification;

	public Dictionary<int, GltfAccessorBuilder> Accessors { get; } = new();

	public Dictionary<int, GltfMeshBuilder> Meshes { get; } = new();

	public Dictionary<int, GltfNodeBuilder> Nodes { get; } = new();

	public Dictionary<int, GltfSceneBuilder> Scenes { get; } = new();

	private Dictionary<int, GltfCameraBuilder> Cameras { get; } = new();

	private Dictionary<int, GltfMaterialBuilder> Materials { get; } = new();

	private readonly GlbHeader _header;

	private GltfRoot _root;

	public GlbV2FileBuilder(GlbHeader header)
	{
		this._header = header;
	}

	public Scene Build()
	{
		_root = this._header.GetRoot<GltfRoot>();

		foreach (var item in _root.Buffers)
		{
		}

		GltfScene rootScene = null;
		if (this._root.Scene.HasValue)
		{
			rootScene = this._root.Scenes[this._root.Scene.Value];
		}
		else
		{
			rootScene = this._root.Scenes.FirstOrDefault();
		}

		if (rootScene == null)
		{
			throw new GltfReaderException("Scene not found");
		}

		GltfSceneBuilder sceneBuilder = new GltfSceneBuilder(rootScene);

		this.createBuilders(Scenes, this._root.Scenes);
		this.createBuilders(Meshes, this._root.Meshes);
		this.createBuilders(Nodes, this._root.Nodes);
		this.createBuilders(Cameras, this._root.Cameras);
		this.createBuilders(Accessors, this._root.Accessors);
		this.createBuilders(Materials, this._root.Materials);

		sceneBuilder.Build(this);

		return sceneBuilder.Scene;
	}

	public StreamIO GetBufferStream(GltfAccessor accessor)
	{
		GltfBufferView bufferView = _root.BufferViews[accessor.BufferView.Value];
		GltfBuffer buffer = _root.Buffers[bufferView.Buffer];

		StreamIO stream = new StreamIO(_header.BinData);
		stream.Position = bufferView.ByteOffset + accessor.ByteOffset;

		return stream;
	}

	public T GetBuilder<T>(int index)
				where T : IGltfObjectBuilder
	{
		Dictionary<int, T> dict;

		var builderType = typeof(T);
		if (builderType == typeof(GltfAccessorBuilder))
		{
			dict = this.Accessors as Dictionary<int, T>;
		}
		else if (builderType == typeof(GltfMeshBuilder))
		{
			dict = this.Meshes as Dictionary<int, T>;
		}
		else if (builderType == typeof(GltfNodeBuilder))
		{
			dict = this.Nodes as Dictionary<int, T>;
		}
		else if (builderType == typeof(GltfSceneBuilder))
		{
			dict = this.Scenes as Dictionary<int, T>;
		}
		else if (builderType == typeof(GltfCameraBuilder))
		{
			dict = this.Cameras as Dictionary<int, T>;
		}
		else if (builderType == typeof(GltfMaterialBuilder))
		{
			dict = this.Materials as Dictionary<int, T>;
		}
		else
		{
			throw new InvalidOperationException();
		}

		var value = dict[index];
		value.Build(this);
		return value;
	}

	private void createBuilders<Builder, Gltf>(Dictionary<int, Builder> collection, Gltf[] gltfArray)
			where Builder : GltfObjectBuilder<Gltf>, new()
	{
		if (gltfArray == null)
		{
			return;
		}

		for (int i = 0; i < gltfArray.Length; i++)
		{
			var gltf = gltfArray[i];
			Builder builder = new();
			builder.GltfObject = gltf;
			collection.Add(i, builder);
		}
	}
}