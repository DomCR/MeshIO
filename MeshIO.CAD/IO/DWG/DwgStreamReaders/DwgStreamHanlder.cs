using CSUtilities.Converters;
using CSUtilities.IO;
using CSUtilities.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.CAD.IO
{
	internal abstract class DwgStreamHanlder : StreamIO, IDwgStreamHandler
	{
		public Encoding Encoding { get; set; } = Encoding.Default;
		public Stream StreamToRead { get { return m_stream; } }
		public int BitShift { get; set; }
		public override long Position
		{
			get => m_stream.Position;
			set
			{
				m_stream.Position = value;
				BitShift = 0;
			}
		}
		protected byte m_lastByte;
		public DwgStreamHanlder(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		//*******************************************************************
		public static IDwgStreamHandler GetStreamHandler(ACadVersion version, Stream stream, bool resetPositon = false)
		{
			switch (version)
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
					return new DwgStreamHandlerAC12(stream, resetPositon);
				case ACadVersion.AC1015:
					return new DwgStreamHandlerAC15(stream, resetPositon);
				case ACadVersion.AC1018:
					return new DwgStreamHandlerAC18(stream, resetPositon);
				case ACadVersion.AC1021:
					return new DwgStreamHandlerAC21(stream, resetPositon);
				case ACadVersion.AC1024:
				case ACadVersion.AC1027:
				case ACadVersion.AC1032:
					return new DwgStreamHandlerAC24(stream, resetPositon);
				default:
					break;
			}

			return null;
		}
		//*******************************************************************
		/// <summary>
		/// Read a byte and store the value, apply the shift to correct the bit reading.
		/// </summary>
		/// <returns>Value fo the last byte.</returns>
		public override byte ReadByte()
		{
			if (BitShift == 0)
			{
				//No need to apply the shift
				m_lastByte = base.ReadByte();

				return m_lastByte;
			}

			//Get the last bits from the last readed byte
			byte lastValues = (byte)((uint)m_lastByte << BitShift);

			m_lastByte = base.ReadByte();

			return (byte)(lastValues | (uint)(byte)((uint)m_lastByte >> 8 - BitShift));
		}
		public override byte[] ReadBytes(int length)
		{
			byte[] numArray = new byte[length];
			applyShiftToArr(length, numArray);
			return numArray;
		}

		#region Read BIT CODES AND DATA DEFINITIONS
		/// <inheritdoc/>
		public bool ReadBit()
		{
			if (BitShift == 0)
			{
				AdvanceByte();
				bool result = (m_lastByte & 128) == 128;
				BitShift = 1;
				return result;
			}

			bool value = (m_lastByte << BitShift & 128) == 128;

			++BitShift;
			BitShift &= 7;

			return value;
		}
		/// <inheritdoc/>
		public short ReadBitAsShort()
		{
			return ReadBit() ? (short)1 : (short)0;
		}
		/// <inheritdoc/>
		public byte Read2Bits()
		{
			byte value;
			if (BitShift == 0)
			{
				AdvanceByte();
				value = (byte)((uint)m_lastByte >> 6);
				BitShift = 2;
			}
			else if (BitShift == 7)
			{
				byte lastValue = (byte)(m_lastByte << 1 & 2);
				AdvanceByte();
				value = (byte)(lastValue | (uint)(byte)((uint)m_lastByte >> 7));
				BitShift = 1;
			}
			else
			{
				value = (byte)(m_lastByte >> 6 - BitShift & 3);
				++BitShift;
				++BitShift;
				BitShift &= 7;
			}

			return value;
		}
		/// <inheritdoc/>
		public short ReadBitShort()
		{
			short value;
			switch (Read2Bits())
			{
				case 0:
					//00 : A short (2 bytes) follows, little-endian order (LSB first)
					value = ReadShort<LittleEndianConverter>();
					break;
				case 1:
					//01 : An unsigned char (1 byte) follows
					if (BitShift == 0)
					{
						AdvanceByte();
						value = m_lastByte;
						break;
					}
					value = applyShiftToLasByte();
					break;
				case 2:
					//10 : 0
					value = 0;
					break;
				case 3:
					//11 : 256
					value = 256;
					break;
				default:
					throw new Exception();
			}
			return value;
		}
		/// <inheritdoc/>
		public bool ReadBitShortAsBool()
		{
			return ReadBitShort() != 0;
		}
		/// <inheritdoc/>
		public int ReadBitLong()
		{
			int value;
			switch (Read2Bits())
			{
				case 0:
					//00 : A long (4 bytes) follows, little-endian order (LSB first)
					value = ReadInt<LittleEndianConverter>();
					break;
				case 1:
					//01 : An unsigned char (1 byte) follows
					if (BitShift == 0)
					{
						AdvanceByte();
						value = m_lastByte;
						break;
					}
					value = applyShiftToLasByte();
					break;
				case 2:
					//10 : 0
					value = 0;
					break;
				default:
					//11 : not used
					throw new Exception();
			}
			return value;
		}
		/// <inheritdoc/>
		public long ReadBitLongLong()
		{
			ulong value = 0;
			byte size = read3bits();

			for (int index = 0; index < size; ++index)
			{
				ulong b = ReadByte();
				value += b << (index << 3);
			}

			return (long)value;
		}
		/// <inheritdoc/>
		public double ReadBitDouble()
		{
			double value;
			switch (Read2Bits())
			{
				case 0:
					value = ReadDouble();
					break;
				case 1:
					value = 1.0;
					break;
				case 2:
					value = 0.0;
					break;
				default:
					throw new Exception();
			}

			return value;
		}

		/// <inheritdoc/>
		public XYZ Read3BitDouble()
		{
			return new XYZ(ReadBitDouble(), ReadBitDouble(), ReadBitDouble());
		}
		/// <inheritdoc/>
		public char ReadRawChar()
		{
			return (char)ReadByte();
		}
		/// <inheritdoc/>
		public long ReadRawLong()
		{
			return ReadInt();
		}
		/// <inheritdoc/>
		public XYZ Read2RawDouble()
		{
			return new XYZ(ReadDouble(), ReadDouble());
		}

		/// <inheritdoc/>
		public int ReadModularChar()
		{
			//Modular characters are a method of storing compressed integer values. They are used in the object map to
			//indicate both handle offsets and file location offsets.They consist of a stream of bytes, terminating when
			//the high bit of the byte is 0.
			int value;

			if (BitShift == 0)
			{
				//No shift, read normal
				AdvanceByte();

				//Check if the current byte
				if ((m_lastByte & 0b10000000) == 0) //Check the flag
				{
					//Drop the flags
					value = m_lastByte & 0b00111111;

					//Check the sign flag
					if ((m_lastByte & 0b01000000) > 0U)
						value = -value;
				}
				else
				{
					int totalShift = 0;
					int sum = m_lastByte & sbyte.MaxValue;
					while (true)
					{
						//Shift to apply
						totalShift += 7;
						AdvanceByte();

						//Check if the highest byte is 0
						if ((m_lastByte & 0b10000000) != 0)
							sum |= (m_lastByte & sbyte.MaxValue) << totalShift;
						else
							break;
					}

					//Drop the flags at the las byte, and add it's value
					value = sum | (m_lastByte & 0b00111111) << totalShift;

					//Check the sign flag
					if ((m_lastByte & 0b01000000) > 0U)
						value = -value;
				}
			}
			else
			{
				//Apply the shift to each byte
				byte lastByte = applyShiftToLasByte();
				if ((lastByte & 0b10000000) == 0)
				{
					//Drop the flags
					value = lastByte & 0b00111111;

					//Check the sign flag
					if ((lastByte & 0b01000000) > 0U)
						value = -value;
				}
				else
				{
					int totalShift = 0;
					int sum = lastByte & sbyte.MaxValue;
					byte currByte;
					while (true)
					{
						//Shift to apply
						totalShift += 7;
						currByte = applyShiftToLasByte();

						//Check if the highest byte is 0
						if ((currByte & 0b10000000) != 0)
							sum |= (currByte & sbyte.MaxValue) << totalShift;
						else
							break;
					}

					//Drop the flags at the las byte, and add it's value
					value = sum | (currByte & 0b00111111) << totalShift;

					//Check the sign flag
					if ((currByte & 0b01000000) > 0U)
						value = -value;
				}
			}
			return value;
		}

		#region Handle reference
		/// <inheritdoc/>
		public ulong HandleReference()
		{
			return HandleReference(false);
		}
		/// <inheritdoc/>
		public ulong HandleReference(bool storeReference)
		{
			return HandleReference(0UL, storeReference, out ReferenceType _);
		}
		/// <inheritdoc/>
		public ulong HandleReference(ulong referenceHandle, bool storeReference)
		{
			return HandleReference(referenceHandle, storeReference, out ReferenceType _);
		}
		/// <inheritdoc/>
		public ulong HandleReference(ulong referenceHandle, bool storeReference, out ReferenceType reference)
		{
			//|CODE (4 bits)|COUNTER (4 bits)|HANDLE or OFFSET|
			byte form = ReadByte();

			//CODE of the reference
			byte code = (byte)((uint)form >> 4);
			//COUNTER tells how many bytes of HANDLE follow.
			int counter = form & 0b00001111;

			//Get the reference type reading the last 2 bits
			reference = (ReferenceType)((uint)code & 0b0011);

			ulong initialPos;

			//0x2, 0x3, 0x4, 0x5	none - just read offset and use it as the result
			if (code <= 0x5)
				initialPos = readHandle(counter);
			//0x6	result is reference handle + 1 (length is 0 in this case)
			else if (code == 0x6)
				initialPos = ++referenceHandle;
			//0x8	result is reference handle – 1 (length is 0 in this case)
			else if (code == 0x8)
				initialPos = --referenceHandle;
			//0xA	result is reference handle plus offset
			else if (code == 0xA)
			{
				ulong offset = readHandle(counter);
				initialPos = referenceHandle + offset;
			}
			//0xC	result is reference handle minus offset
			else if (code == 0xC)
			{
				ulong offset = readHandle(counter);
				initialPos = referenceHandle - offset;
			}
			else
			{
				throw new Exception();
			}

			//TODO: implement the reference storage
			if (storeReference)
				throw new NotImplementedException();

			return initialPos;
		}

		private ulong readHandle(int length)
		{
			byte[] raw = new byte[length];
			byte[] arr = new byte[8];

			if (StreamToRead.Read(raw, 0, length) < length)
				throw new EndOfStreamException();

			if (BitShift == 0)
			{
				//Set the array backwards
				for (int i = 0; i < length; ++i)
					arr[length - 1 - i] = raw[i];
			}
			else
			{
				int shift = 8 - BitShift;
				for (int i = 0; i < length; ++i)
				{
					//Get the last byte value
					byte lastByteValue = (byte)((uint)m_lastByte << BitShift);
					//Save the last byte
					m_lastByte = raw[i];
					//Add the value of the next byte to the current
					byte value = (byte)(lastByteValue | (uint)(byte)((uint)m_lastByte >> shift));
					//Save the value into the array
					arr[length - 1 - i] = value;
				}
			}

			//Set the left bytes to 0
			for (int index = length; index < 8; ++index)
				arr[index] = 0;

			return LittleEndianConverter.Instance.ToUInt64(arr);
		}
		#endregion

		/// <inheritdoc/>
		public virtual string ReadTextUnicode()
		{
			int textLength = ReadShort();
			int encodingKey = ReadByte();
			string value;

			if (textLength == 0)
			{
				value = string.Empty;
			}
			else
			{
				value = ReadString(textLength, TextEncoding.GetListedEncoding((CodePage)encodingKey));
			}

			return value;
		}
		/// <inheritdoc/>
		public abstract string ReadVariableText();

		/// <inheritdoc/>
		public byte[] ReadSentinel()
		{
			return ReadBytes(16);
		}

		public Color ReadCmColor()
		{
			//CMC:
			//BS: color index(always 0)
			short colorIndex = ReadBitShort();
			//BL: RGB value
			int rgb = ReadBitLong();

			byte num = ReadByte();

			string colorName = string.Empty;
			//RC: Color Byte(&1 => color name follows(TV),
			if ((num & 1) == 1)
				colorName = ReadVariableText();

			string bookName = string.Empty;
			//&2 => book name follows(TV))
			if ((num & 2) == 2)
				bookName = ReadVariableText();

			return new Color();
		}

		/// <inheritdoc/>
		public virtual ObjectType ReadObjectType() => (ObjectType)ReadBitShort();
		#endregion

		#region Stream pointer control
		/// <inheritdoc/>
		public long PositionInBits()
		{
			long bitPosition = StreamToRead.Position * 8L;

			if ((uint)BitShift > 0U)
				bitPosition += BitShift - 8;

			return bitPosition;
		}
		/// <inheritdoc/>
		public void SetPositionInBits(long position)
		{
			Position = position >> 3;
			BitShift = (int)(position & 7L);

			if ((uint)BitShift <= 0U)
				return;

			AdvanceByte();
		}
		/// <inheritdoc/>
		public void AdvanceByte()
		{
			m_lastByte = base.ReadByte();
		}
		/// <inheritdoc/>
		public void Advance(int offset)
		{
			if (offset > 1)
				StreamToRead.Position += offset - 1;

			ReadByte();
		}
		/// <inheritdoc/>
		public ushort ResetShift()
		{
			//Reset the shift value
			if ((uint)BitShift > 0U)
				BitShift = 0;

			AdvanceByte();
			ushort num = m_lastByte;
			AdvanceByte();

			return (ushort)(num | (uint)(ushort)((uint)m_lastByte << 8));
		}
		#endregion
		//*******************************************************************
		protected byte applyShiftToLasByte()
		{
			byte value = (byte)((uint)m_lastByte << BitShift);

			AdvanceByte();

			return (byte)((uint)value | (byte)((uint)m_lastByte >> 8 - BitShift));
		}
		private void applyShiftToArr(int length, byte[] arr)
		{
			//Empty Stream
			if (StreamToRead.Read(arr, 0, length) != length)
				throw new EndOfStreamException();

			if ((uint)BitShift <= 0U)
				return;

			int shift = 8 - BitShift;
			for (int i = 0; i < length; ++i)
			{
				//Get the last byte value
				byte lastByteValue = (byte)((uint)m_lastByte << BitShift);
				//Save the last byte
				m_lastByte = arr[i];
				//Add the value of the next byte to the current
				byte value = (byte)(lastByteValue | (uint)(byte)((uint)m_lastByte >> shift));
				//Save the value into the array
				arr[i] = value;
			}
		}
		private byte read3bits()
		{
			byte b1 = 0;
			if (ReadBit())
				b1 = 1;
			byte b2 = (byte)((uint)b1 << 1);
			if (ReadBit())
				b2 |= 1;
			byte b3 = (byte)((uint)b2 << 1);
			if (ReadBit())
				b3 |= 1;
			return b3;
		}
	}
}
