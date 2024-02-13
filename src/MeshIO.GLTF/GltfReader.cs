using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Core;
using MeshIO.GLTF.Exceptions;
using MeshIO.GLTF.Schema;
using MeshIO.GLTF.Schema.V2;
using System;
using System.IO;

namespace MeshIO.GLTF
{
	public class GltfReader : ReaderBase
	{
		private GlbHeader _header;
		private GltfRoot _root;
		private StreamIO _binaryStream;

		private readonly StreamIO _streamIO;

		public GltfReader(string path) : this(new FileStream(path, FileMode.Open)) { }

		public GltfReader(Stream stream) : base(stream) 
		{
			this._streamIO = new StreamIO(this._stream);
		}

		/// <summary>
		/// Read the GLTF file
		/// </summary>
		public override Scene Read()
		{
			//The 12-byte header consists of three 4-byte entries:
			this._header = new GlbHeader();
			//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
			this._header.Magic = this._streamIO.ReadUInt<LittleEndianConverter>();
			//version indicates the version of the Binary glTF container format. This specification defines version 2.
			this._header.Version = this._streamIO.ReadUInt<LittleEndianConverter>();
			//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
			this._header.Length = this._streamIO.ReadUInt<LittleEndianConverter>();

			if (this._header.Version != 2)
				throw new NotSupportedException($"Version {this._header.Version} not supported");

			//Chunk 0 Json
			uint jsonChunkLength = this._streamIO.ReadUInt<LittleEndianConverter>();
			string jsonChunkType = this._streamIO.ReadString(4);

			if (jsonChunkType != "JSON")
				throw new GltfReaderException("Chunk type does not match", this._streamIO.Position);

			string json = this._streamIO.ReadString((int)jsonChunkLength);

#if NETFRAMEWORK || NETSTANDARD
			_root = Newtonsoft.Json.JsonConvert.DeserializeObject<GltfRoot>(json);
#else
			this._root = Newtonsoft.Json.JsonConvert.DeserializeObject<GltfRoot>(json);
#endif

			//Chunk 1 bin
			uint binChunkLength = this._streamIO.ReadUInt<LittleEndianConverter>();
			string binChunkType = this._streamIO.ReadString(4);

			//Check the chunk type
			if (binChunkType != "BIN\0")
				throw new GltfReaderException("Chunk type does not match", this._streamIO.Position);

			byte[] binChunk = this._streamIO.ReadBytes((int)binChunkLength);
			this._binaryStream = new StreamIO(binChunk);

			var reader = GltfBinaryReaderBase.GetBynaryReader((int)this._header.Version, this._root, binChunk);
			reader.OnNotification += onNotificationEvent;

			return reader.Read();
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			this._binaryStream?.Dispose();
		}
	}
}
