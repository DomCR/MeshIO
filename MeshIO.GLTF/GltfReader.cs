using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Core;
using MeshIO.Elements;
using MeshIO.GLTF.Exceptions;
using MeshIO.GLTF.Schema;
using MeshIO.GLTF.Schema.V2;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MeshIO.GLTF
{
	public class GltfReader : ReaderBase, IDisposable
	{
		private GlbHeader _header;
		private GltfRoot _root;
		private StreamIO _binaryStream;
		private readonly StreamIO _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="GltfReader"/> class for the specified file
		/// </summary>
		/// <param name="path">The complete file path to read to</param>
		/// <exception cref="ArgumentNullException"></exception>
		public GltfReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new StreamIO(new FileStream(path, FileMode.Open));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GltfReader"/> class for the specified stream
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public GltfReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = new StreamIO(stream);
		}

		/// <summary>
		/// Read gltf file
		/// </summary>
		/// <returns></returns>
		/// <exception cref="GltfReaderException"></exception>
		public Scene Read()
		{
			//The 12-byte header consists of three 4-byte entries:
			_header = new GlbHeader();
			//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
			_header.Magic = _stream.ReadUInt<LittleEndianConverter>();
			//version indicates the version of the Binary glTF container format. This specification defines version 2.
			_header.Version = _stream.ReadUInt<LittleEndianConverter>();
			//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
			_header.Length = _stream.ReadUInt<LittleEndianConverter>();

			if (_header.Version != 2)
				throw new NotImplementedException($"Version {_header.Version} not implemented");

			//Chunk 0 Json
			uint jsonChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string jsonChunkType = _stream.ReadString(4);

			if (jsonChunkType != "JSON")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			_root = JsonConvert.DeserializeObject<GltfRoot>(_stream.ReadString((int)jsonChunkLength));
			
			//Chunk 1 bin
			uint binChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string binChunkType = _stream.ReadString(4);

			//Check the chunk type
			if (binChunkType != "BIN\0")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			byte[] binChunk = _stream.ReadBytes((int)binChunkLength);
			_binaryStream = new StreamIO(binChunk);

			return GltfBinaryReaderBase.GetBynaryReader((int)_header.Version, _root, binChunk).Read(this.OnNotification);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_stream.Dispose();
			_binaryStream.Dispose();
		}
	}
}
