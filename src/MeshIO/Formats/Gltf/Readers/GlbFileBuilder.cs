using CSUtilities.IO;
using MeshIO.Formats.Gltf.Builders;
using MeshIO.Formats.Gltf.Exceptions;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Gltf.Readers;

internal class GlbFileBuilder : IGlbFileBuilder
{
	public event NotificationEventHandler OnNotification;

	private readonly Dictionary<string, GltfAccessorBuilder> _accessors = new();

	private readonly Dictionary<string, GltfBuffer> _buffers = new();

	private readonly Dictionary<string, GltfBufferView> _bufferViews = new();

	private readonly Dictionary<string, GltfCameraBuilder> _cameras = new();

	private readonly GlbHeader _header;

	private readonly Dictionary<string, GltfMaterialBuilder> _materials = new();

	private readonly Dictionary<string, GltfMeshBuilder> _meshes = new();

	private readonly Dictionary<string, GltfNodeBuilder> _nodes = new();

	private readonly Dictionary<string, GltfSceneBuilder> _scenes = new();

	private GltfRoot _root;

	public GlbFileBuilder(GlbHeader header)
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

		this.createBuilders(_scenes, this._root.Scenes);
		this.createBuilders(_meshes, this._root.Meshes);
		this.createBuilders(_nodes, this._root.Nodes);
		this.createBuilders(_cameras, this._root.Cameras);
		this.createBuilders(_accessors, this._root.Accessors);
		this.createBuilders(_materials, this._root.Materials);

		for (int i = 0; i < this._root.Buffers.Length; i++)
		{
			var gltf = this._root.Buffers[i];
			if (this._header.Version == GltfVersion.V1)
			{
				_buffers.Add(gltf.Name, gltf);
			}
			else
			{
				_buffers.Add(i.ToString(), gltf);
			}
		}
		for (int i = 0; i < this._root.BufferViews.Length; i++)
		{
			var gltf = this._root.BufferViews[i];
			if (this._header.Version == GltfVersion.V1)
			{
				_bufferViews.Add(gltf.Name, gltf);
			}
			else
			{
				_bufferViews.Add(i.ToString(), gltf);
			}
		}

		sceneBuilder.Build(this);

		return sceneBuilder.Scene;
	}

	public StreamIO GetBufferStream(GltfAccessor accessor)
	{
		GltfBufferView bufferView = this._bufferViews[accessor.BufferView];
		GltfBuffer buffer = this._buffers[bufferView.Buffer];

		StreamIO stream = new StreamIO(_header.BinData);
		stream.Position = bufferView.ByteOffset + accessor.ByteOffset;

		return stream;
	}

	public void Notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
	{
		this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
	}

	public bool TryGetBuilder<T>(string id, out T builder, bool notify = true)
		where T : IGltfObjectBuilder
	{
		if (string.IsNullOrEmpty(id))
		{
			builder = default;
			return false;
		}

		Dictionary<string, T> dict;

		var builderType = typeof(T);
		if (builderType == typeof(GltfAccessorBuilder))
		{
			dict = this._accessors as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfMeshBuilder))
		{
			dict = this._meshes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfNodeBuilder))
		{
			dict = this._nodes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfSceneBuilder))
		{
			dict = this._scenes as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfCameraBuilder))
		{
			dict = this._cameras as Dictionary<string, T>;
		}
		else if (builderType == typeof(GltfMaterialBuilder))
		{
			dict = this._materials as Dictionary<string, T>;
		}
		else
		{
			throw new InvalidOperationException();
		}

		if (dict.TryGetValue(id, out builder))
		{
			builder.Build(this);
			return true;
		}
		else if (notify)
		{
			this.Notify($"[{typeof(T)}] with id {id} not found.", NotificationType.Warning);
		}

		return false;
	}

	private void createBuilders<Builder, Gltf>(Dictionary<string, Builder> collection, Gltf[] gltfArray)
		where Builder : GltfObjectBuilder<Gltf>, new()
		where Gltf : IGltfNamedObject
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

			if (this._header.Version == GltfVersion.V1)
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