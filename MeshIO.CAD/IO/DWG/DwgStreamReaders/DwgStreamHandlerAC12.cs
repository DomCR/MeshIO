using CSUtilities.Converters;
using CSUtilities.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.CAD.IO
{
	internal class DwgStreamHandlerAC12 : DwgStreamHanlder
	{
		public DwgStreamHandlerAC12(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadVariableText()
		{
			short length = ReadBitShort();
			string str;
			if (length > 0)
			{
				str = ReadString(length, Encoding).Replace("\0", "");
			}
			else
				str = string.Empty;
			return str;
		}
	}
	internal class DwgStreamHandlerAC15 : DwgStreamHandlerAC12
	{
		public DwgStreamHandlerAC15(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
	}
	internal class DwgStreamHandlerAC18 : DwgStreamHandlerAC15
	{
		public DwgStreamHandlerAC18(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadTextUnicode()
		{
			short textLength = ReadShort<LittleEndianConverter>();
			string value;
			if (textLength == 0)
			{
				value = string.Empty;
			}
			else
			{
				//Read the string and get rid of the empty bytes
				value = ReadString(textLength,
					TextEncoding.GetListedEncoding(CodePage.Windows1252))
					.Replace("\0", "");
			}
			return value;
		}
	}
	internal class DwgStreamHandlerAC21 : DwgStreamHandlerAC18
	{
		public DwgStreamHandlerAC21(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadTextUnicode()
		{
			short textLength = ReadShort<LittleEndianConverter>();
			string value;
			if (textLength > 0)
			{
				value = string.Empty;
			}
			else
			{
				//Correct the text length by shifting 1 bit
				short length = (short)(textLength << 1);
				//Read the string and get rid of the empty bytes
				value = ReadString(length, Encoding.Unicode).Replace("\0", "");
			}
			return value;
		}
		public override string ReadVariableText()
		{
			int textLength = (int)ReadBitShort();
			string value;
			if (textLength == 0)
			{
				value = string.Empty;
			}
			else
			{
				//Correct the text length by shifting 1 bit
				short length = (short)(textLength << 1);
				//Read the string and get rid of the empty bytes
				value = ReadString(length, Encoding.Unicode).Replace("\0", "");
			}
			return value;
		}
	}
	internal class DwgStreamHandlerAC24 : DwgStreamHandlerAC21
	{
		public DwgStreamHandlerAC24(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
	}
	/// <summary>
	/// Enables to handle the different offsets for the objects, text and references.
	/// The versions <see cref="ACadVersion.AC1021"/> and above, have different offsets for 
	/// the main data like system variables and handles or text, which are separated in a 
	/// "handle-section" and "string-data-section".
	/// This class makes it easier to handle the different stream offsets.
	/// </summary>
	internal class DwgMultipleHandlerAC21 : IDwgStreamHandler
	{
		public Encoding Encoding
		{
			get => m_defaultHandler.Encoding; set
			{
				m_defaultHandler.Encoding = value;
				m_textHandler.Encoding = value;
				m_referenceHander.Encoding = value;
			}
		}
		public Stream StreamToRead => throw new InvalidOperationException();
		public int BitShift { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }
		public long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }
		public bool IsEmpty { get; } = false;

		private IDwgStreamHandler m_defaultHandler;
		private IDwgStreamHandler m_textHandler;
		private IDwgStreamHandler m_referenceHander;

		public DwgMultipleHandlerAC21(IDwgStreamHandler defaultHandler, IDwgStreamHandler textHandler, IDwgStreamHandler referenceHandler)
		{
			m_defaultHandler = defaultHandler;
			m_textHandler = textHandler;
			m_referenceHander = referenceHandler;
		}

		public void Advance(int offset)
		{
			throw new InvalidOperationException();
		}

		public void AdvanceByte()
		{
			throw new InvalidOperationException();
		}

		public ulong HandleReference()
		{
			return m_referenceHander.HandleReference();
		}

		public ulong HandleReference(ulong referenceHandle)
		{
			return m_referenceHander.HandleReference(referenceHandle);
		}

		public ulong HandleReference(ulong referenceHandle, out ReferenceType reference)
		{
			return m_referenceHander.HandleReference(referenceHandle, out reference);
		}

		public long PositionInBits()
		{
			throw new InvalidOperationException();
		}

		public byte Read2Bits()
		{
			return m_defaultHandler.Read2Bits();
		}

		public XYZ Read2RawDouble()
		{
			return m_defaultHandler.Read2RawDouble();
		}

		public XYZ Read3BitDouble()
		{
			return m_defaultHandler.Read3BitDouble();
		}

		public bool ReadBit()
		{
			return m_defaultHandler.ReadBit();
		}

		public short ReadBitAsShort()
		{
			return m_defaultHandler.ReadBitAsShort();
		}

		public double ReadBitDouble()
		{
			return m_defaultHandler.ReadBitDouble();
		}

		public int ReadBitLong()
		{
			return m_defaultHandler.ReadBitLong();
		}

		public long ReadBitLongLong()
		{
			throw new NotImplementedException();
		}

		public short ReadBitShort()
		{
			return m_defaultHandler.ReadBitShort();
		}

		public bool ReadBitShortAsBool()
		{
			return m_defaultHandler.ReadBitShortAsBool();
		}

		public byte ReadByte()
		{
			return m_defaultHandler.ReadByte();
		}

		public byte[] ReadBytes(int length)
		{
			throw new NotImplementedException();
		}

		public Color ReadCmColor()
		{
			//To read a color in this version file needs to access to the
			//string data section at the same time.

			//CMC:
			//BS: color index(always 0)
			short colorIndex = ReadBitShort();
			//BL: RGB value
			int rgb = ReadBitLong();

			//RC : Color Byte
			byte id = ReadByte();

			string colorName = string.Empty;
			//RC: Color Byte(&1 => color name follows(TV),
			if ((id & 1) == 1)
				colorName = ReadVariableText();

			string bookName = string.Empty;
			//&2 => book name follows(TV))
			if ((id & 2) == 2)
				bookName = ReadVariableText();

			return new Color();
		}

		public DateTime ReadDateTime()
		{
			return m_defaultHandler.ReadDateTime();
		}

		public double ReadDouble()
		{
			throw new NotImplementedException();
		}

		public int ReadInt()
		{
			throw new NotImplementedException();
		}

		public int ReadModularChar()
		{
			throw new NotImplementedException();
		}

		public ObjectType ReadObjectType()
		{
			throw new NotImplementedException();
		}

		public char ReadRawChar()
		{
			return m_defaultHandler.ReadRawChar();
		}

		public long ReadRawLong()
		{
			throw new NotImplementedException();
		}

		public byte[] ReadSentinel()
		{
			return m_defaultHandler.ReadSentinel();
		}

		public short ReadShort()
		{
			throw new NotImplementedException();
		}

		public short ReadShort<T>() where T : IEndianConverter, new()
		{
			throw new NotImplementedException();
		}

		public string ReadTextUnicode()
		{
			//Handle the text section if is empty
			if (m_textHandler.IsEmpty)
				return string.Empty;

			return m_textHandler.ReadTextUnicode();
		}

		public TimeSpan ReadTimeSpan()
		{
			return m_defaultHandler.ReadTimeSpan();
		}

		public uint ReadUInt()
		{
			throw new NotImplementedException();
		}

		public string ReadVariableText()
		{
			//Handle the text section if is empty
			if (m_textHandler.IsEmpty)
				return string.Empty;

			return m_textHandler.ReadVariableText();
		}

		public ushort ResetShift()
		{
			throw new InvalidOperationException();
		}

		public void SetPositionInBits(long positon)
		{
			throw new InvalidOperationException();
		}

		public long SetPositionByFlag(long position)
		{
			throw new InvalidOperationException();
		}
	}
}
