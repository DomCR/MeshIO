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

	public Dictionary<string, GltfAccessorBuilder> Accessors { get; } = new();

	public Dictionary<string, GltfMeshBuilder> Meshes { get; } = new();

	public Dictionary<string, GltfNodeBuilder> Nodes { get; } = new();

	public Dictionary<string, GltfSceneBuilder> Scenes { get; } = new();

	private Dictionary<string, GltfBuffer> Buffers { get; } = new();

	private Dictionary<string, GltfBufferView> BufferViews { get; } = new();

	private Dictionary<string, GltfCameraBuilder> Cameras { get; } = new();

	private Dictionary<string, GltfMaterialBuilder> Materials { get; } = new();

	private readonly GlbHeader _header;

	private GltfRoot _root;

	public GlbV2FileBuilder(GlbHeader header)
	{
		this._header = header;
	}

	public Scene Build()
	{
		_root = this._header.GetRoot();

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


		for (int i = 0; i < this._root.Buffers.Length; i++)
		{
			var gltf = this._root.Buffers[i];
			if (this._header.Version == 1)
			{
				Buffers.Add(gltf.Name, gltf);
			}
			else
			{
				Buffers.Add(i.ToString(), gltf);
			}
		}
		for (int i = 0; i < this._root.BufferViews.Length; i++)
		{
			var gltf = this._root.BufferViews[i];
			if (this._header.Version == 1)
			{
				BufferViews.Add(gltf.Name, gltf);
			}
			else
			{
				BufferViews.Add(i.ToString(), gltf);
			}
		}

		sceneBuilder.Build(this);

		return sceneBuilder.Scene;
	}

	public StreamIO GetBufferStream(GltfAccessor accessor)
	{
		GltfBufferView bufferView = this.BufferViews[accessor.BufferView];
		GltfBuffer buffer = this.Buffers[bufferView.Buffer];

		StreamIO stream = new StreamIO(_header.BinData);
		stream.Position = bufferView.ByteOffset + accessor.ByteOffset;

		return stream;
	}

	public T GetBuilder<T>(string id)
		where T : IGltfObjectBuilder
	{
		Dictionary<string, T> dict;

		var builderType = typeof(T);
		if (builderType == typeof(GltfAccessorBuilder))
		{
			dict = this.Accessors as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfMeshBuilder))
		{
			dict = this.Meshes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfNodeBuilder))
		{
			dict = this.Nodes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfSceneBuilder))
		{
			dict = this.Scenes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfCameraBuilder))
		{
			dict = this.Cameras as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfMaterialBuilder))
		{
			dict = this.Materials as Dictionary<string, T>;
		}
		else
		{
			throw new InvalidOperationException();
		}

		var value = dict[id];
		value.Build(this);
		return value;
	}

	public void Notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
	{
		this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
	}

	private void createBuilders<Builder, Gltf>(Dictionary<string, Builder> collection, Gltf[] gltfArray)
		where Builder : GltfObjectBuilder<Gltf>, new()
		where Gltf : INamedObject
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

			if (this._header.Version == 1)
			{
				collection.Add(builder.GltfObject.Name, builder);
			}
			else
			{
				collection.Add(i.ToString(), builder);
			}
		}
	}
}