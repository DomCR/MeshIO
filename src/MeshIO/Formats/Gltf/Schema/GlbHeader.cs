using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Formats.Gltf.Schema.V2;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Gltf.Schema;

internal class GlbHeader
{
	public byte[] BinData { get; set; }

	public byte[] JsonData { get; set; }

	public uint Length { get; set; }

	public uint Magic { get; set; }

	public uint Version { get; set; }

	public static GlbHeader Read(Stream stream)
	{
		var reader = new StreamIO(stream);

		//The 12-byte header consists of three 4-byte entries:
		var header = new GlbHeader();
		//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
		header.Magic = reader.ReadUInt<LittleEndianConverter>();
		//version indicates the version of the Binary glTF container format. This specification defines version 2.
		header.Version = reader.ReadUInt<LittleEndianConverter>();
		//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
		header.Length = reader.ReadUInt<LittleEndianConverter>();

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

		return header;
	}

	public T GetRoot<T>()
	{
		string json = Encoding.UTF8.GetString(JsonData);
		return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
	}
}