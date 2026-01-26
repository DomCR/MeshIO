using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V1;
using MeshIO.Formats.Gltf.Schema.V2;
using System;
using System.Collections.Generic;

namespace MeshIO.Formats.Gltf.Readers;

internal class GlbV1FileBuilder : IGlbFileBuilder
{
	public event NotificationEventHandler OnNotification;

	private readonly GlbHeader _header;

	private GltfV1Root _root;

	public GlbV1FileBuilder(GlbHeader header)
	{
		this._header = header;
	}

	public Scene Build()
	{
		//var map = this._header.GetRoot<Dictionary<string, object>>();
		//this._root = new GltfV1Root(map);

		//var a = new GltfRoot(map);

		throw new NotImplementedException();
	}
}