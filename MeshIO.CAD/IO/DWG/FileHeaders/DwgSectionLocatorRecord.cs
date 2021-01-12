using System;
using System.Collections.Generic;

namespace MeshIO.CAD.IO
{
	public class DwgSectionLocatorRecord
	{
		public int Number { get; set; }
		public long Seeker { get; set; }
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
		public ulong Size { get; set; }
		public int PageCount { get; set; }
		public int DecompressedSize { get; set; } = 29696;
		public int Compressed
		{
			get => this.m_compressed;
			set => this.m_compressed = value == 1 || value == 2 ? value :
					throw new Exception();
		}
		private int m_compressed = 2;
		public int SectionId { get; set; }
		public int Encrypted { get; set; }
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
		public int CompressedSize { get; internal set; }
		public int PageNumber { get; internal set; }
		public int DecompressedSize { get; internal set; }
		public long Seeker { get; internal set; }
	}
}