using MeshIO.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.FBX
{
	/// <summary>
	/// Base class for binary stream wrappers
	/// </summary>
	internal abstract class FbxBinary
	{
		/// <summary>
		/// Number of null bytes between the footer code and the version
		/// </summary>
		private const int footerZeroes1 = 20;
		/// <summary>
		/// Number of null bytes between the footer version and extension code
		/// </summary>
		private const int footerZeroes2 = 120;
		/// <summary>
		/// The size of the footer code
		/// </summary>
		protected const int footerCodeSize = 16;
		/// <summary>
		/// The namespace separator in the binary format (remember to reverse the identifiers)
		/// </summary>
		protected const string binarySeparator = "\0\x1";

		/// <summary>
		/// The namespace separator in the ASCII format and in object data
		/// </summary>
		protected const string asciiSeparator = "::";
		protected const string timePath1 = "FBXHeaderExtension";
		protected const string timePath2 = "CreationTimeStamp";

		/// <summary>
		/// Header string, found at the top of all compliant files
		/// </summary>
		private static readonly byte[] m_headerString = Encoding.ASCII.GetBytes("Kaydara FBX Binary  \0\x1a\0");
		private static readonly byte[] m_sourceId =
			{ 0x58, 0xAB, 0xA9, 0xF0, 0x6C, 0xA2, 0xD8, 0x3F, 0x4D, 0x47, 0x49, 0xA3, 0xB4, 0xB2, 0xE7, 0x3D };
		private static readonly byte[] m_key =
			{ 0xE2, 0x4F, 0x7B, 0x5F, 0xCD, 0xE4, 0xC8, 0x6D, 0xDB, 0xD8, 0xFB, 0xD7, 0x40, 0x58, 0xC6, 0x78 };
		private static readonly byte[] m_extension =
			{ 0xF8, 0x5A, 0x8C, 0x6A, 0xDE, 0xF5, 0xD9, 0x7E, 0xEC, 0xE9, 0x0C, 0xE3, 0x75, 0x8F, 0x29, 0x0B };
		private static readonly Stack<string> m_timePath = new Stack<string>(new[] { timePath1, timePath2 });

		private static bool allZero(byte[] array)
		{
			foreach (var b in array)
				if (b != 0)
					return false;
			return true;
		}
		/// <summary>
		/// Checks if the first part of 'data' matches 'original'
		/// </summary>
		/// <param name="data"></param>
		/// <param name="original"></param>
		/// <returns><c>true</c> if it does, otherwise <c>false</c></returns>
		protected static bool checkEqual(byte[] data, byte[] original)
		{
			for (int i = 0; i < original.Length; i++)
				if (data[i] != original[i])
					return false;
			return true;
		}
		/// <summary>
		/// Writes the FBX header string
		/// </summary>
		/// <param name="stream"></param>
		protected static void writeHeader(Stream stream)
		{
			stream.Write(m_headerString, 0, m_headerString.Length);
		}
		/// <summary>
		/// Reads the FBX header string
		/// </summary>
		/// <param name="stream"></param>
		/// <returns><c>true</c> if it's compliant</returns>
		protected static bool readHeader(Stream stream)
		{
			byte[] buf = new byte[m_headerString.Length];
			stream.Read(buf, 0, buf.Length);
			return checkEqual(buf, m_headerString);
		}
		/// <summary>
		/// Encrypt the footer.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		protected static void encrypt(byte[] a, byte[] b)
		{
			byte c = 64;
			for (int i = 0; i < footerCodeSize; i++)
			{
				a[i] = (byte)(a[i] ^ (byte)(c ^ b[i]));
				c = a[i];
			}
		}
		/// <summary>
		/// Gets a single timestamp component
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="element"></param>
		/// <returns></returns>
		protected static int getTimestampVar(FbxNode timestamp, string element)
		{
			var elementNode = timestamp[element];
			if (elementNode != null && elementNode.Properties.Count > 0)
			{
				var prop = elementNode.Properties[0];
				if (prop is int || prop is long)
					return (int)prop;
			}
			throw new FbxException(m_timePath, -1, "Timestamp has no " + element);
		}
		/// <summary>
		/// Generates the unique footer code based on the document's timestamp
		/// </summary>
		/// <param name="document"></param>
		/// <returns>A 16-byte code</returns>
		protected static byte[] generateFooterCode(FbxNodeCollection document)
		{
			var timestamp = document.GetRelative(timePath1 + "/" + timePath2);
			if (timestamp == null)
				throw new FbxException(m_timePath, -1, "No creation timestamp");
			try
			{
				return generateFooterCode(
					getTimestampVar(timestamp, "Year"),
					getTimestampVar(timestamp, "Month"),
					getTimestampVar(timestamp, "Day"),
					getTimestampVar(timestamp, "Hour"),
					getTimestampVar(timestamp, "Minute"),
					getTimestampVar(timestamp, "Second"),
					getTimestampVar(timestamp, "Millisecond")
					);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new FbxException(m_timePath, -1, "Invalid timestamp");
			}
		}
		/// <summary>
		/// Generates a unique footer code based on a timestamp
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="millisecond"></param>
		/// <returns>A 16-byte code</returns>
		protected static byte[] generateFooterCode(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			if (year < 0 || year > 9999)
				throw new ArgumentOutOfRangeException(nameof(year));
			if (month < 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));
			if (day < 0 || day > 31)
				throw new ArgumentOutOfRangeException(nameof(day));
			if (hour < 0 || hour >= 24)
				throw new ArgumentOutOfRangeException(nameof(hour));
			if (minute < 0 || minute >= 60)
				throw new ArgumentOutOfRangeException(nameof(minute));
			if (second < 0 || second >= 60)
				throw new ArgumentOutOfRangeException(nameof(second));
			if (millisecond < 0 || millisecond >= 1000)
				throw new ArgumentOutOfRangeException(nameof(millisecond));

			var str = (byte[])m_sourceId.Clone();
			var mangledTime = $"{second:00}{month:00}{hour:00}{day:00}{(millisecond / 10):00}{year:0000}{minute:00}";
			var mangledBytes = Encoding.ASCII.GetBytes(mangledTime);
			encrypt(str, mangledBytes);
			encrypt(str, m_key);
			encrypt(str, mangledBytes);
			return str;
		}
		/// <summary>
		/// Writes the FBX footer extension (NB - not the unique footer code)
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="version"></param>
		protected void writeFooter(BinaryWriter stream, int version)
		{
			var zeroes = new byte[Math.Max(footerZeroes1, footerZeroes2)];
			stream.Write(zeroes, 0, footerZeroes1);
			stream.Write(version);
			stream.Write(zeroes, 0, footerZeroes2);
			stream.Write(m_extension, 0, m_extension.Length);
		}
		/// <summary>
		/// Reads and checks the FBX footer extension (NB - not the unique footer code)
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="version"></param>
		/// <returns><c>true</c> if it's compliant</returns>
		protected bool checkFooter(BinaryReader stream, FbxVersion version)
		{
			var buffer = new byte[Math.Max(footerZeroes1, footerZeroes2)];
			stream.Read(buffer, 0, footerZeroes1);
			bool correct = allZero(buffer);
			var readVersion = stream.ReadInt32();
			correct &= (readVersion == (int)version);
			stream.Read(buffer, 0, footerZeroes2);
			correct &= allZero(buffer);
			stream.Read(buffer, 0, m_extension.Length);
			correct &= checkEqual(buffer, m_extension);
			return correct;
		}
	}
}
