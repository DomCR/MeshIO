using CSUtilities;
using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Formats.Gltf.Schema.V2;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Gltf.Schema;

internal class GltfHeader
{
	public byte[] BinData { get; set; }

	public byte[] JsonData { get; set; }

	public uint Length { get; set; }

	public uint Magic { get; set; }

	public GltfVersion Version { get; set; } = GltfVersion.Unknown;

	private GltfHeader()
	{
	}

	public static GltfHeader Read(StreamIO json, StreamIO bin)
	{
		var header = new GltfHeader();

		header.JsonData = json.ReadBytes((int)json.Stream.Length);
		header.BinData = bin.ReadBytes((int)bin.Stream.Length);

		return header;
	}

	public static GltfHeader Read(StreamIO reader)
	{
		//The 12-byte header consists of three 4-byte entries:
		var header = new GltfHeader();
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
		string json = Encoding.UTF8.GetString(JsonData);

		switch (this.Version)
		{
			case GltfVersion.Unknown:
				try
				{
					return JsonUtils.Deserialize<GltfRoot>(json);
				}
				catch (System.Exception)
				{
					return new GltfRoot(JsonUtils.Deserialize<Dictionary<string, object>>(json));
				}
			case GltfVersion.V1:
				return new GltfRoot(JsonUtils.Deserialize<Dictionary<string, object>>(json));
			case GltfVersion.V2:
				return JsonUtils.Deserialize<GltfRoot>(json);
			default:
				throw new System.NotSupportedException();
		}

	}

	private static void readV1Header(GltfHeader header, StreamIO reader)
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

	private static void readV2Heder(GltfHeader header, StreamIO reader)
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