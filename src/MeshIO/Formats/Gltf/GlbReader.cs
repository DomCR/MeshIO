using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Exceptions;
using MeshIO.Formats.Gltf.Schema.V2;
using System;
using System.IO;

namespace MeshIO.Formats.Gltf;

public class GlbReader : SceneReader<GltfReaderOptions>
{
	private StreamIO _binaryStream;

	private GlbHeader _header;

	private GltfRoot _root;

	/// <inheritdoc/>
	public GlbReader(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(path, options, notification) { }

	/// <inheritdoc/>
	public GlbReader(Stream stream, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(stream, options, notification)
	{
	}

	public static Scene Read(string path, NotificationEventHandler notificationHandler = null)
	{
		using (GlbReader reader = new GlbReader(path))
		{
			reader.OnNotification += notificationHandler;
			return reader.Read();
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();
		this._binaryStream?.Dispose();
	}

	/// <inheritdoc/>
	public override Scene Read()
	{
		GlbHeader header = GlbHeader.Read(_stream.Stream);

		IGlbFileBuilder reader;
		switch (header.Version)
		{
			case 2:
				reader = new GlbV2FileBuilder(header);
				break;
			case 1:
			default:
				throw new NotSupportedException($"Version {this._header.Version} not supported.");

		}

		return reader.Build();

		//The 12-byte header consists of three 4-byte entries:
		this._header = new GlbHeader();
		//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
		this._header.Magic = this._stream.ReadUInt<LittleEndianConverter>();
		//version indicates the version of the Binary glTF container format. This specification defines version 2.
		this._header.Version = this._stream.ReadUInt<LittleEndianConverter>();
		//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
		this._header.Length = this._stream.ReadUInt<LittleEndianConverter>();

		if (this._header.Version != 2)
			throw new NotSupportedException($"Version {this._header.Version} not supported");

		//Chunk 0 Json
		uint jsonChunkLength = this._stream.ReadUInt<LittleEndianConverter>();
		string jsonChunkType = this._stream.ReadString(4);

		if (jsonChunkType != "JSON")
			throw new GltfReaderException("Chunk type does not match", this._stream.Position);

		string json = this._stream.ReadString((int)jsonChunkLength);
#if NET5_0_OR_GREATER
		//this._root = System.Text.Json.JsonSerializer.Deserialize<GltfRoot>(json);
#else
		//this._root = Newtonsoft.Json.JsonConvert.DeserializeObject<GltfRoot>(json);
#endif
		this._root = Newtonsoft.Json.JsonConvert.DeserializeObject<GltfRoot>(json);
		//Chunk 1 bin
		uint binChunkLength = this._stream.ReadUInt<LittleEndianConverter>();
		string binChunkType = this._stream.ReadString(4);

		//Check the chunk type
		if (binChunkType != "BIN\0")
			throw new GltfReaderException("Chunk type does not match", this._stream.Position);

		byte[] binChunk = this._stream.ReadBytes((int)binChunkLength);
		this._binaryStream = new StreamIO(binChunk);

		//var reader = GltfBinaryReaderBase.GetBynaryReader((int)this._header.Version, this._root, binChunk);
		//reader.OnNotification += onNotificationEvent;

		//return reader.Read();
		return null;
	}
}