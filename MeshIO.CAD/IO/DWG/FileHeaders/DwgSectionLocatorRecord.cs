using System;
using System.Collections.Generic;

namespace MeshIO.CAD.IO
{
	public enum DwgSectionHash
	{
		AcDb_Unknown = 0x00000000,
		AcDb_Security = 0x4a0204ea,
		AcDb_FileDepList = 0x6c4205ca,
		AcDb_VBAProject = 0x586e0544,
		AcDb_AppInfo = 0x3fa0043e,
		AcDb_Preview = 0x40aa0473,
		AcDb_SummaryInfo = 0x717a060f,
		AcDb_RevHistory = 0x60a205b3,
		AcDb_AcDbObjects = 0x674c05a9,
		AcDb_ObjFreeSpace = 0x77e2061f,
		AcDb_Template = 0x4a1404ce,
		AcDb_Handles = 0x3f6e0450,
		AcDb_Classes = 0x3f54045f,
		AcDb_AuxHeader = 0x54f0050a,
		AcDb_Header = 0x32b803d9,
		AcDb_Signature = -0x00000001,
	}

	public static class DwgSectionUtils
	{
		public static int GetSectionLocatorByName(string name)
		{
			switch (name)
			{
				case "AcDb:Header":
					return 0;
				case "AcDb:Classes":
					return 1;
				case "AcDb:Handles":
					return 2;
				case "AcDb:ObjFreeSpace":
					return 3;
				case "AcDb:Template":
					return 4;
				//No record id for this sections
				case "AcDb:SummaryInfo":
				case "AcDb:AcDbObjects":
				case "AcDb:FileDepList":
				default:
					return -1;
			}
		}
		public static string GetPageNameByRecordId(int id)
		{
			switch (id)
			{
				case 0:
					return "AcDb:Header";
				default:
					break;
			}

			throw new NotImplementedException();
		}
	}

	public class DwgSectionLocatorRecord
	{
		/// <summary>
		/// Number of the record or id.
		/// </summary>
		public int Number { get; set; }
		/// <summary>
		/// Offset where the record is.
		/// </summary>
		public long Seeker { get; set; }
		/// <summary>
		/// Size in bytes of this record.
		/// </summary>
		public long Size { get; set; }
		public DwgSectionLocatorRecord() { }
		public DwgSectionLocatorRecord(int number, int offset, int size)
		{
			this.Number = number;
			this.Seeker = offset;
			this.Size = size;
		}
		/// <summary>
		/// Check if the position is in the record.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public bool IsInTheRecord(int position)
		{
			return position >= this.Seeker && position < this.Seeker + this.Size;
		}
	}

	public class DwgSectionDescriptor
	{
		public string Name { get; set; }
		public ulong CompressedSize { get; set; }
		public int PageCount { get; set; }
		public ulong DecompressedSize { get; set; } = 29696;
		/// <remarks>
		/// Is only used for the version <see cref="ACadVersion.AC1018"/> and <see cref="ACadVersion.AC1024"/> or above.
		/// </remarks>
		public int CompressedCode
		{
			get => this.m_compressed;
			set => this.m_compressed = value == 1 || value == 2 ? value :
					throw new Exception();
		}
		private int m_compressed = 2;
		/// <remarks>
		/// Is only used for the version <see cref="ACadVersion.AC1018"/> and <see cref="ACadVersion.AC1024"/> or above.
		/// </remarks>
		public bool IsCompressed { get { return CompressedCode == 2; } }
		public int SectionId { get; set; }
		public int Encrypted { get; set; }

		public ulong? HashCode { get; internal set; }
		public ulong? Encoding { get; internal set; }

		public List<DwgLocalSectionMap> LocalSections { get; set; } = new List<DwgLocalSectionMap>();

		public DwgSectionDescriptor() { }
		public DwgSectionDescriptor(string name)
		{
			Name = name;
		}
	}

	public class DwgLocalSectionMap
	{
		public bool IsEmpty { get; internal set; }
		public ulong Offset { get; internal set; }
		public ulong CompressedSize { get; internal set; }
		public int PageNumber { get; internal set; }
		public ulong DecompressedSize { get; internal set; }
		public long Seeker { get; internal set; }
		public long Size { get; internal set; }
		public ulong Checksum { get; internal set; }
		public ulong CRC { get; internal set; }
		public long PageSize { get; internal set; }
	}
}