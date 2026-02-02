using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Formats.Gltf.Schema.V2;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Gltf.Schema;

internal class GlbHeader
{
	public byte[] BinData { get; set; }

	public byte[] JsonData { get; set; }

	public uint Length { get; set; }

	public uint Magic { get; set; }

	public GltfVersion Version { get; set; }

	public static GlbHeader Read(Stream stream)
	{
		var reader = new StreamIO(stream);

		//The 12-byte header consists of three 4-byte entries:
		var header = new GlbHeader();
		//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
		header.Magic = reader.ReadUInt<LittleEndianConverter>();
		//version indicates the version of the Binary glTF container format. This specification defines version 2.
		header.Version = (GltfVersion)reader.ReadUInt<LittleEndianConverter>();
		//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
		header.Length = reader.ReadUInt<LittleEndianConverter>();

		if (header.Version == GltfVersion.V1)
		{
			readV1Header(header, reader);
		}
		else if (header.Version == GltfVersion.V2)
		{
			readV2Heder(header, reader);
		}
		else
		{
			throw new System.NotSupportedException();
		}

		return header;
	}

	public GltfRoot GetRoot()
	{
		if (this.Version == GltfVersion.V1)
		{
			string json = Encoding.UTF8.GetString(JsonData);
			var map = JsonUtils.Deserialize<Dictionary<string, object>>(json);

			return new GltfRoot(map);
		}
		else
		{
			string json = Encoding.UTF8.GetString(JsonData);
			return JsonUtils.Deserialize<GltfRoot>(json);
		}
	}

	private static void readV1Header(GlbHeader header, StreamIO reader)
	{
		uint totalLength = header.Length;
		int jsonLength = reader.ReadInt<LittleEndianConverter>();
		uint contentFormat = reader.ReadUInt<LittleEndianConverter>();

		header.JsonData = new byte[jsonLength];
		int paddedOffset = 20 + jsonLength;
		int paddedLength = (paddedOffset & 3) == 0 ? paddedOffset : ((paddedOffset | 3) + 1);
		int binLength = (int)totalLength - paddedLength;

		int readJson = reader.Stream.Read(header.JsonData, 0, jsonLength);

		if (paddedLength > paddedOffset)
		{
			reader.Stream.Seek(paddedLength - paddedOffset, SeekOrigin.Current);
		}

		header.BinData = new byte[binLength];
		reader.Stream.Read(header.BinData, 0, binLength);
	}

	private static void readV2Heder(GlbHeader header, StreamIO reader)
	{
		while (reader.Position < header.Length)
		{
			var chunkLength = reader.ReadUInt<LittleEndianConverter>();
			ChunkType type = (ChunkType)reader.ReadUInt<LittleEndianConverter>();
			byte[] arr;
			switch (type)
			{
				case ChunkType.Json:
					header.JsonData = reader.ReadBytes((int)chunkLength);
					break;
				case ChunkType.Bin:
					header.BinData = reader.ReadBytes((int)chunkLength);
					break;
				default:
					reader.Stream.Seek(chunkLength, SeekOrigin.Current);
					continue;
			}
		}
	}
}