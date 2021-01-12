using CSUtilities.Converters;
using CSUtilities.IO;
using CSUtilities.Text;
using MeshIO.CAD.IO;
using MeshIO.CAD.IO.DWG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.CAD
{
	public class DwgReader : IDisposable
	{
		private string m_filename;
		private StreamIO m_fileStream;

		private DwgFileHeader m_fileHeader;
		private CadDocument m_documentToRead;

		private CadHeader m_cadHeader;

		/// <summary>
		/// Initializes a new instance of the <see cref="DwgReader" /> class.
		/// </summary>
		/// <param name="filename">The filename of the file to open.</param>
		public DwgReader(string filename)
		{
			this.m_filename = filename;
			this.m_fileStream = new StreamIO(filename);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DwgReader" /> class.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		public DwgReader(Stream stream)
		{
			this.m_fileStream = new StreamIO(stream);
		}
		//**************************************************************************
		public void Read()
		{
			//Setup the model
			this.m_documentToRead = new CadDocument();
			this.m_documentToRead.Filename = this.m_filename;


		}
		public DwgFileHeader ReadFileHeader()
		{
			//Reset the stream position at the begining
			m_fileStream.Position = 0L;

			//0x00	6	“ACXXXX” version string
			ACadVersion version = CadUtils.GetVersionFromName(m_fileStream.ReadString(6, Encoding.ASCII));
			this.m_fileHeader = DwgFileHeader.GetFileHeader(version);

			//Get the stream reader
			IDwgStreamHandler sreader = DwgStreamHanlder.GetStreamReader(m_fileHeader.AcadVersion, this.m_fileStream.Stream);

			//Read the file header
			switch (m_fileHeader.AcadVersion)
			{
				case ACadVersion.Unknown:
					throw new Exception();
				case ACadVersion.MC0_0:
				case ACadVersion.AC1_2:
				case ACadVersion.AC1_4:
				case ACadVersion.AC1_50:
				case ACadVersion.AC2_10:
				case ACadVersion.AC1002:
				case ACadVersion.AC1003:
				case ACadVersion.AC1004:
				case ACadVersion.AC1006:
				case ACadVersion.AC1009:
					throw new NotSupportedException();
				case ACadVersion.AC1012:
				case ACadVersion.AC1014:
				case ACadVersion.AC1015:
					readFileHeaderAC15(m_fileHeader as DwgFileHeader15, sreader);
					break;
				case ACadVersion.AC1018:
					readFileHeaderAC18(m_fileHeader as DwgFileHeader18, sreader);
					break;
				case ACadVersion.AC1021:
					readFileHeaderAC21(m_fileHeader as DwgFileHeader21, sreader);
					break;
				case ACadVersion.AC1024:
				case ACadVersion.AC1027:
				case ACadVersion.AC1032:
					//Check if it works...
					readFileHeaderAC18(m_fileHeader as DwgFileHeader18, sreader);
					break;
				default:
					break;
			}

			return m_fileHeader;
		}
		/// <inheritdoc/>
		public void Dispose()
		{
			this.m_fileStream.Dispose();
		}
		//**************************************************************************
		/// <summary>
		/// Read the file header for the AC1012 to AC1015 (R13-R15) versions of the header.
		/// </summary>
		/// <param name="fileheader">File header to read</param>
		/// <param name="sreader"></param>
		private void readFileHeaderAC15(DwgFileHeader15 fileheader, IDwgStreamHandler sreader)
		{
			//The next 7 starting at offset 0x06 are to be six bytes of 0 
			//(in R14, 5 0’s and the ACADMAINTVER variable) and a byte of 1.
			sreader.ReadBytes(7);
			//At 0x0D is a seeker (4 byte long absolute address) for the beginning sentinel of the image data.
			fileheader.PreviewAddress = sreader.ReadInt();

			//Bytes at 0x13 and 0x14 are a raw short indicating the value of the code page for this drawing file.
			sreader.ReadBytes(2);

			fileheader.DrawingCodePage = (CodePage)sreader.ReadShort();
			sreader.Encoding = TextEncoding.GetListedEncoding(fileheader.DrawingCodePage);

			int nRecords = (int)sreader.ReadRawLong();
			for (int i = 0; i < nRecords; ++i)
			{
				//Record number (raw byte) | Seeker (raw long) | Size (raw long)
				DwgSectionLocatorRecord record = new DwgSectionLocatorRecord();
				record.Number = (int)sreader.ReadByte();
				record.Seeker = sreader.ReadRawLong();
				record.Size = sreader.ReadRawLong();

				fileheader.Records.Add(record.Number, record);
			}

			sreader.ResetShift();
		}
		/// <summary>
		/// Read the file header for the AC1018 (2004-2006) version of the header.
		/// </summary>
		/// <param name="fileheader">File header to read</param>
		/// <param name="sreader"></param>
		private void readFileHeaderAC18(DwgFileHeader18 fileheader, IDwgStreamHandler sreader)
		{
			readFileMetaData(fileheader, sreader);

			//0x80	0x6C	Encrypted Data
			//Metadata:
			//The encrypted data at 0x80 can be decrypted by exclusive or’ing the 0x6c bytes of data 
			//from the file with the following magic number sequence:

			//29 23 BE 84 E1 6C D6 AE 52 90 49 F1 F1 BB E9 EB
			//B3 A6 DB 3C 87 0C 3E 99 24 5E 0D 1C 06 B7 47 DE
			//B3 12 4D C8 43 BB 8B A6 1F 03 5A 7D 09 38 25 1F
			//5D D4 CB FC 96 F5 45 3B 13 0D 89 0A 1C DB AE 32
			//20 9A 50 EE 40 78 36 FD 12 49 32 F6 9E 7D 49 DC
			//AD 4F 14 F2 44 40 66 D0 6B C4 30 B7

			CRC32StreamHandler crc32 = new CRC32StreamHandler(sreader.ReadBytes(0x6C), 0U); //108

			sreader.ReadBytes(20);  //CHECK IF IS USEFUL
			#region Read header encrypted data

			StreamIO headerStream = new StreamIO(crc32);

			//0x00	12	“AcFssFcAJMB” file ID string
			string fileId = headerStream.ReadString(12, TextEncoding.GetListedEncoding(CodePage.Windows1252));
			if (fileId != "AcFssFcAJMB\0")
				throw new Exception();

			//0x0C	4	0x00(long)
			headerStream.ReadInt<LittleEndianConverter>();
			//0x10	4	0x6c(long)
			headerStream.ReadInt<LittleEndianConverter>();
			//0x14	4	0x04(long)
			headerStream.ReadInt<LittleEndianConverter>();
			//0x18	4	Root tree node gap	
			fileheader.RootTreeNodeGap = headerStream.ReadInt<LittleEndianConverter>();
			//0x1C	4	Lowermost left tree node gap
			fileheader.LeftGap = headerStream.ReadInt<LittleEndianConverter>();
			//0x20	4	Lowermost right tree node gap
			fileheader.RigthGap = headerStream.ReadInt<LittleEndianConverter>();
			//0x24	4	Unknown long(ODA writes 1)	
			headerStream.ReadInt<LittleEndianConverter>();
			//0x28	4	Last section page Id
			fileheader.LastPageId = headerStream.ReadInt<LittleEndianConverter>();
			//0x2C	8	Last section page end address
			fileheader.LastSectionAddr = (long)headerStream.ReadULong<LittleEndianConverter>();
			//0x34	8	Second header data address pointing to the repeated header data at the end of the file
			fileheader.SecondHeaderAddr = headerStream.ReadULong<LittleEndianConverter>();
			//0x3C	4	Gap amount
			fileheader.GapAmount = (int)headerStream.ReadUInt<LittleEndianConverter>();
			//0x40	4	Section page amount
			fileheader.SectionAmount = (int)headerStream.ReadUInt<LittleEndianConverter>();
			//0x44	4	0x20(long)
			headerStream.ReadInt<LittleEndianConverter>();
			//0x48	4	0x80(long)
			headerStream.ReadInt<LittleEndianConverter>();
			//0x4C	4	0x40(long)	
			headerStream.ReadInt<LittleEndianConverter>();
			//0x50	4	Section Page Map Id
			fileheader.SectionPageMapId = headerStream.ReadUInt<LittleEndianConverter>();
			//0x54	8	Section Page Map address(add 0x100 to this value)
			fileheader.PageMapAddress = headerStream.ReadULong<LittleEndianConverter>() + 256UL;
			//0x5C	4	Section Map Id	
			fileheader.SectionMapId = headerStream.ReadUInt<LittleEndianConverter>();
			//0x60	4	Section page array size
			fileheader.SectionArrayPageSize = headerStream.ReadUInt<LittleEndianConverter>();
			//0x64	4	Gap array size
			fileheader.GapArraySize = (int)headerStream.ReadUInt<LittleEndianConverter>();
			//0x68	4	CRC32(long).See paragraph 2.14.2 for the 32 - bit CRC calculation, 
			//			the seed is zero.Note that the CRC 
			//			calculation is done including the 4 CRC bytes that are 
			//			initially zero! So the CRC calculation takes into account 
			//			all of the 0x6c bytes of the data in this table.
			fileheader.CRCSeed = headerStream.ReadUInt();
			#endregion

			#region Read page map of the file
			sreader.Position = (long)fileheader.PageMapAddress;

			//Get the page size
			getPageHeaderData(sreader, out _, out long decompressedSize, out _, out _, out _);
			//Get the descompressed stream to read the records
			StreamIO decompressed = new StreamIO(
				Dwg2004LZ77.Decompress(sreader.StreamToRead, decompressedSize));

			//Section size
			int num = 0x100;
			while (decompressed.Position < decompressed.Length)
			{
				DwgSectionLocatorRecord record = new DwgSectionLocatorRecord();
				//0x00	4	Section page number, starts at 1, page numbers are unique per file.
				record.Number = decompressed.ReadInt();
				//0x04	4	Section size
				record.Size = decompressed.ReadInt();

				if (record.Number >= 0)
				{
					record.Seeker = num;
					fileheader.Records.Add(record.Number, record);
				}
				else
				{
					//If the section number is negative, this represents a gap in the sections (unused data). 
					//For a negative section number, the following data will be present after the section size:

					//0x00	4	Parent
					decompressed.ReadInt();
					//0x04	4	Left
					decompressed.ReadInt();
					//0x08	4	Right
					decompressed.ReadInt();
					//0x0C	4	0x00
					decompressed.ReadInt();
				}

				num += (int)record.Size;
			}
			#endregion

			#region Read the data section map
			//Set the positon of the map
			sreader.Position = fileheader.Records[(int)fileheader.SectionMapId].Seeker;
			//Get the page size
			getPageHeaderData(sreader, out _, out decompressedSize, out _, out _, out _);
			StreamIO streamIO = new StreamIO(
				Dwg2004LZ77.Decompress(sreader.StreamToRead, decompressedSize));

			//0x00	4	Number of section descriptions(NumDescriptions)
			int ndescriptions = streamIO.ReadInt<LittleEndianConverter>();
			//0x04	4	0x02 (long)
			streamIO.ReadInt<LittleEndianConverter>();
			//0x08	4	0x00007400 (long)
			streamIO.ReadInt<LittleEndianConverter>();
			//0x0C	4	0x00 (long)
			streamIO.ReadInt<LittleEndianConverter>();
			//0x10	4	Unknown (long), ODA writes NumDescriptions here.
			streamIO.ReadInt<LittleEndianConverter>();

			for (int i = 0; i < ndescriptions; ++i)
			{
				DwgSectionDescriptor descriptor = new DwgSectionDescriptor();
				//0x00	8	Size of section(OdUInt64)
				descriptor.Size = streamIO.ReadULong();
				/*0x08	4	Page count(PageCount). Note that there can be more pages than PageCount,
							as PageCount is just the number of pages written to file.
							If a page contains zeroes only, that page is not written to file.
							These “zero pages” can be detected by checking if the page’s start 
							offset is bigger than it should be based on the sum of previously read pages 
							decompressed size(including zero pages).After reading all pages, if the total 
							decompressed size of the pages is not equal to the section’s size, add more zero 
							pages to the section until this condition is met.
				*/
				descriptor.PageCount = streamIO.ReadInt<LittleEndianConverter>();
				//0x0C	4	Max Decompressed Size of a section page of this type(normally 0x7400)
				descriptor.DecompressedSize = streamIO.ReadInt<LittleEndianConverter>();
				//0x10	4	Unknown(long)
				streamIO.ReadInt<LittleEndianConverter>();
				//0x14	4	Compressed(1 = no, 2 = yes, normally 2)
				descriptor.Compressed = streamIO.ReadInt<LittleEndianConverter>();
				//0x18	4	Section Id(starts at 0). The first section(empty section) is numbered 0, consecutive sections are numbered descending from(the number of sections – 1) down to 1.
				descriptor.SectionId = streamIO.ReadInt<LittleEndianConverter>();
				//0x1C	4	Encrypted(0 = no, 1 = yes, 2 = unknown)
				descriptor.Encrypted = streamIO.ReadInt<LittleEndianConverter>();
				//0x20	64	Section Name(string)
				descriptor.Name = streamIO.ReadString(64, TextEncoding.GetListedEncoding(CodePage.Windows1252)).Replace("\0", "");

				ulong currPosition = 0;
				//Following this, the following (local) section page map data will be present
				for (int j = 0; j < descriptor.PageCount; ++j)
				{
					DwgLocalSectionMap localmap = new DwgLocalSectionMap();
					//0x00	4	Page number(index into SectionPageMap), starts at 1
					localmap.PageNumber = streamIO.ReadInt<LittleEndianConverter>();
					//0x04	4	Data size for this page(compressed size).
					localmap.CompressedSize = streamIO.ReadInt<LittleEndianConverter>();
					//0x08	8	Start offset for this page(OdUInt64).If this start offset is smaller than the sum of the decompressed size of all previous pages, then this page is to be preceded by zero pages until this condition is met.
					localmap.Offset = streamIO.ReadULong();

					//same decompressed size and seeker
					localmap.DecompressedSize = descriptor.DecompressedSize;
					localmap.Seeker = fileheader.Records[localmap.PageNumber].Seeker;

					//Maximum section page size appears to be 0x7400 bytes in the normal case.
					//If a logical section of the file (the database objects, for example) exceeds this size, then it is broken up into pages of size 0x7400.

					//Add empty local section to fill the gap between them
					for (; currPosition < localmap.Offset; currPosition += (ulong)descriptor.DecompressedSize)
					{
						DwgLocalSectionMap emptySection = new DwgLocalSectionMap();
						emptySection.IsEmpty = true;
						emptySection.PageNumber = 0;
						emptySection.CompressedSize = 0;
						emptySection.Offset = currPosition;
						emptySection.DecompressedSize = descriptor.DecompressedSize;
						descriptor.LocalSections.Add(emptySection);
					}

					descriptor.LocalSections.Add(localmap);
					currPosition += (ulong)descriptor.DecompressedSize;
				}

				//Add empty local section to fill the gap between the descriptors
				for (; currPosition < descriptor.Size; currPosition += (ulong)descriptor.DecompressedSize)
				{
					DwgLocalSectionMap emptySection = new DwgLocalSectionMap();
					emptySection.IsEmpty = true;
					emptySection.PageNumber = 0;
					emptySection.CompressedSize = 0;
					emptySection.Offset = currPosition;
					emptySection.DecompressedSize = descriptor.DecompressedSize;
					descriptor.LocalSections.Add(emptySection);
				}

				//Get the final size for the local section
				uint sizeLeft = (uint)(descriptor.Size % (ulong)descriptor.DecompressedSize);
				if (sizeLeft > 0U && descriptor.LocalSections.Count > 0)
					descriptor.LocalSections[descriptor.LocalSections.Count - 1].DecompressedSize = (int)sizeLeft;

				fileheader.Descriptors.Add(descriptor.Name, descriptor);
			}
			#endregion
		}
		private void getPageHeaderData(IDwgStreamHandler sreader, 
			out long sectionType, 
			out long decompressedSize,
			out long compressedSize,
			out long compressionType,
			out long checksum
			)
		{
			//0x00	4	Section page type:
			//Section page map: 0x41630e3b
			//Section map: 0x4163003b
			sectionType = sreader.ReadRawLong();
			//0x04	4	Decompressed size of the data that follows
			decompressedSize = sreader.ReadRawLong();
			//0x08	4	Compressed size of the data that follows(CompDataSize)
			compressedSize = sreader.ReadRawLong();

			//0x0C	4	Compression type(0x02)
			compressionType = sreader.ReadRawLong();
			//0x10	4	Section page checksum
			checksum = sreader.ReadRawLong();
		}
		/// <summary>
		/// Read the file header for the AC1021 (2007-2009) version of the header.
		/// </summary>
		/// <param name="fileheader">File header to read</param>
		/// <param name="sreader"></param>
		private void readFileHeaderAC21(DwgFileHeader21 fileheader, IDwgStreamHandler sreader)
		{
			readFileMetaData(fileheader, sreader);

			//The last 0x28 bytes of this section consists of check data, 
			//containing 5 Int64 values representing CRC’s and related numbers 
			//(starting from 0x3D8 until the end). The first 0x3D8 bytes 
			//should be decoded using Reed-Solomon (255, 239) decoding, with a factor of 3.
			byte[] compressedData = sreader.ReadBytes(0x400);
			byte[] decodedData = this.reedSolomonDecoding(compressedData, 3, 239);

			//0x00	8	CRC
			long crc = LittleEndianConverter.Instance.ToInt64(decodedData, 0);
			//0x08	8	Unknown key
			long unknownKey = LittleEndianConverter.Instance.ToInt64(decodedData, 8);
			//0x10	8	Compressed Data CRC
			long compressedDataCRC = LittleEndianConverter.Instance.ToInt64(decodedData, 16);
			//0x18	4	ComprLen
			int comprLen = LittleEndianConverter.Instance.ToInt32(decodedData, 24);
			//0x1C	4	Length2
			int length2 = LittleEndianConverter.Instance.ToInt32(decodedData, 28);

			//The decompressed size is a fixed 0x110.
			byte[] buffer = new byte[0x110];
			//If ComprLen is negative, then Data is not compressed (and data length is ComprLen).
			if (comprLen < 0)
			{
				//buffer = decodedData
				throw new NotImplementedException();
			}
			//If ComprLen is positive, the ComprLen bytes of data are compressed
			else
			{
				DwgR21LZ77.Decompress(decodedData, 32U, (uint)comprLen, buffer);
			}

			//Get the descompressed stream to read the records
			StreamIO decompressed = new StreamIO(buffer);

			//Read the compressed data
			fileheader.CompressedMetadata = new Dwg21CompressedMetadata()
			{
				//0x00	8	Header size (normally 0x70)
				HeaderSize = decompressed.ReadULong(),  //debug: 112
														//0x08	8	File size
				FileSize = decompressed.ReadULong(),
				//0x10	8	PagesMapCrcCompressed
				PagesMapCrcCompressed = decompressed.ReadULong(),
				//0x18	8	PagesMapCorrectionFactor
				PagesMapCorrectionFactor = decompressed.ReadULong(),
				//0x20	8	PagesMapCrcSeed
				PagesMapCrcSeed = decompressed.ReadULong(),
				//0x28	8	Pages map2offset(relative to data page map 1, add 0x480 to get stream position)
				Map2Offset = decompressed.ReadULong(),
				//0x30	8	Pages map2Id
				Map2Id = decompressed.ReadULong(),
				//0x38	8	PagesMapOffset(relative to data page map 1, add 0x480 to get stream position)
				PagesMapOffset = decompressed.ReadULong(),
				//0x40	8	PagesMapId
				PagesMapId = decompressed.ReadULong(),
				//0x48	8	Header2offset(relative to page map 1 address, add 0x480 to get stream position)
				Header2offset = decompressed.ReadULong(),
				//0x50	8	PagesMapSizeCompressed
				PagesMapSizeCompressed = decompressed.ReadULong(),
				//0x58	8	PagesMapSizeUncompressed
				PagesMapSizeUncompressed = decompressed.ReadULong(),
				//0x60	8	PagesAmount
				PagesAmount = decompressed.ReadULong(),
				//0x68	8	PagesMaxId
				PagesMaxId = decompressed.ReadULong(),
				//0x70	8	Unknown(normally 0x20, 32)
				Unknow0x20 = decompressed.ReadULong(),
				//0x78	8	Unknown(normally 0x40, 64)
				Unknow0x40 = decompressed.ReadULong(),
				//0x80	8	PagesMapCrcUncompressed
				PagesMapCrcUncompressed = decompressed.ReadULong(),
				//0x88	8	Unknown(normally 0xf800, 63488)
				Unknown0x800 = decompressed.ReadULong(),
				//0x90	8	Unknown(normally 4)
				Unknown4 = decompressed.ReadULong(),
				//0x98	8	Unknown(normally 1)
				Unknown1 = decompressed.ReadULong(),
				//0xA0	8	SectionsAmount(number of sections + 1)
				SectionsAmount = decompressed.ReadULong(),
				//0xA8	8	SectionsMapCrcUncompressed
				SectionsMapCrcUncompressed = decompressed.ReadULong(),
				//0xB0	8	SectionsMapSizeCompressed
				SectionsMapSizeCompressed = decompressed.ReadULong(),
				//0xB8	8	SectionsMap2Id
				SectionsMap2Id = decompressed.ReadULong(),
				//0xC0	8	SectionsMapId
				SectionsMapId = decompressed.ReadULong(),
				//0xC8	8	SectionsMapSizeUncompressed
				SectionsMapSizeUncompressed = decompressed.ReadULong(),
				//0xD0	8	SectionsMapCrcCompressed
				SectionsMapCrcCompressed = decompressed.ReadULong(),
				//0xD8	8	SectionsMapCorrectionFactor
				SectionsMapCorrectionFactor = decompressed.ReadULong(),
				//0xE0	8	SectionsMapCrcSeed
				SectionsMapCrcSeed = decompressed.ReadULong(),
				//0xE8	8	StreamVersion(normally 0x60100)
				StreamVersion = decompressed.ReadULong(),
				//0xF0	8	CrcSeed
				CrcSeed = decompressed.ReadULong(),
				//0xF8	8	CrcSeedEncoded
				CrcSeedEncoded = decompressed.ReadULong(),
				//0x100	8	RandomSeed
				RandomSeed = decompressed.ReadULong(),
				//0x108	8	Header CRC64
				HeaderCRC64 = decompressed.ReadULong()
			};
		}
		/// <summary>
		/// Read the metadata from the file.
		/// </summary>
		/// <param name="fileheader">File header where the data will be stored</param>
		/// <param name="sreader"></param>
		private void readFileMetaData(DwgFileHeader18 fileheader, IDwgStreamHandler sreader)
		{
			//5 bytes of 0x00 
			sreader.Advance(5);

			//0x0B	1	Maintenance release version
			fileheader.AcadMaintenanceVersion = (int)sreader.ReadByte();
			//0x0C	1	Byte 0x00, 0x01, or 0x03
			sreader.Advance(1);
			//0x0D	4	Preview address(long), points to the image page + page header size(0x20).
			fileheader.PreviewAddress = sreader.ReadRawLong();
			//0x11	1	Dwg version (Acad version that writes the file)
			fileheader.DwgVersion = sreader.ReadByte();
			//0x12	1	Application maintenance release version(Acad maintenance version that writes the file)
			fileheader.AppReleaseVersion = sreader.ReadByte();

			//0x13	2	Codepage
			fileheader.DrawingCodePage = (CodePage)sreader.ReadShort();
			sreader.Encoding = TextEncoding.GetListedEncoding(fileheader.DrawingCodePage);

			//Advance empty bytes 
			//0x15	3	3 0x00 bytes
			sreader.Advance(3);

			//0x18	4	SecurityType (long), see R2004 meta data, the definition is the same, paragraph 4.1.
			fileheader.SecurityType = sreader.ReadRawLong();
			//0x1C	4	Unknown long
			sreader.ReadRawLong();
			//0x20	4	Summary info Address in stream
			fileheader.SummaryInfoAddr = sreader.ReadRawLong();
			//0x24	4	VBA Project Addr(0 if not present)
			fileheader.VbaProjectAddr = sreader.ReadRawLong();

			//0x28	4	0x00000080
			sreader.ReadRawLong();

			//0x2C	0x54	0x00 bytes
			sreader.ReadRawLong();
			//Get to offset 0x80
			sreader.Advance(80);
		}
		/// <summary>
		/// Apply a simple reed Solomon decoding to a byte array.
		/// </summary>
		/// <param name="encoded"></param>
		/// <param name="factor"></param>
		/// <param name="blockSize"></param>
		private byte[] reedSolomonDecoding(byte[] encoded, int factor, int blockSize)
		{
			byte[] decoded = new byte[factor * blockSize];
			int index = 0;
			int n = 0;
			int length = decoded.Length;
			for (int i = 0; i < factor; ++i)
			{
				int cindex = n;
				if (n < encoded.Length)
				{
					int size = System.Math.Min(length, blockSize);
					length -= size;
					int offset = index + size;
					while (index < offset)
					{
						decoded[index] = encoded[cindex];
						++index;
						cindex += factor;
					}
				}
				++n;
			}

			return decoded;
		}
	}
}
