using System;
using System.IO;
using System.Text;

namespace MeshIO.CAD.IO
{
	/*
	 NOTE: Unless otherwise stated, all data in this manual is in little-endian order, 
			with the least significant byte first.

		B : bit (1 or 0)
		BB : special 2 bit code (entmode in entities, for instance)
		3B : bit triplet (1-3 bits) (R24)
		BS : bitshort (16 bits)
		BL : bitlong (32 bits)
		BLL : bitlonglong (64 bits) (R24)
		BD : bitdouble
		2BD : 2D point (2 bitdoubles)
		3BD : 3D point (3 bitdoubles)
		RC : raw char (not compressed)
		RS : raw short (not compressed)
		RD : raw double (not compressed)
		RL : raw long (not compressed)
		2RD : 2 raw doubles
		3RD : 3 raw doubles
		MC : modular char
		MS : modular short
		H : handle reference (see the HANDLE REFERENCES section)
		T : text (bitshort length, followed by the string).
		TU : Unicode text (bitshort character length, followed by Unicode string, 
			2 bytes per character). Unicode text is read from the “string stream” 
			within the object data, see the main Object description section for details.
		TV : Variable text, T for 2004 and earlier files, TU for 2007+ files.
		X : special form
		U : unknown
		SN : 16 byte sentinel
		BE : BitExtrusion
		DD : BitDouble With Default
		BT : BitThickness
		3DD : 3D point as 3 DD, needing 3 default values
		CMC : CmColor value
		TC : True Color: this is the same format as CMC in R2004+.
		OT : Object type
	 */

	public interface IDwgStreamHandler
	{
		/// <summary>
		/// Encoding used to read the text.
		/// </summary>
		Encoding Encoding { get; set; }
		/// <summary>
		/// Stream that will be read.
		/// </summary>
		Stream StreamToRead { get; }
		//uint Length { get; set; }
		/// <summary>
		/// Shift to perform after reading a single bit.
		/// </summary>
		int BitShift { get; set; }
		/// <summary>
		/// Current stream position.
		/// </summary>
		long Position { get; set; }

		byte ReadByte();
		short ReadShort();
		int ReadInt();
		uint ReadUInt();

		double ReadDouble();
		byte[] ReadBytes(int length);

		#region Read BIT CODES AND DATA DEFINITIONS
		/// <summary>
		/// B : bit (1 or 0)
		/// </summary>
		/// <returns></returns>
		bool ReadBit();
		/// <summary>
		/// <see cref="IDwgStreamHandler.ReadBit"/> return the result as short.
		/// </summary>
		/// <returns></returns>
		short ReadBitAsShort();
		/// <summary>
		/// BB : special 2 bit code (entmode in entities, for instance)
		/// </summary>
		/// <returns></returns>
		byte Read2Bits();

		/// <summary>
		/// BS : bitshort (16 bits)
		/// </summary>
		/// <returns></returns>
		short ReadBitShort();
		/// <summary>
		/// <see cref="IDwgStreamHandler.ReadBitShort"/> return the result as bool.
		/// </summary>
		/// <returns></returns>
		bool ReadBitShortAsBool();
		/// <summary>
		/// BL : bitlong (32 bits)
		/// </summary>
		/// <returns></returns>
		int ReadBitLong();
		/// <summary>
		/// BLL : bitlonglong (64 bits) (R24)
		/// </summary>
		/// <returns></returns>
		long ReadBitLongLong();
		/// <summary>
		/// BD : bitdouble
		/// </summary>
		/// <returns></returns>
		double ReadBitDouble();

		/// <summary>
		/// 3BD : 3D point (3 bitdoubles)
		/// </summary>
		/// <returns></returns>
		XYZ Read3BitDouble();
		/// <summary>
		/// RC : raw char (not compressed)
		/// </summary>
		/// <returns></returns>
		char ReadRawChar();

		/// <summary>
		/// RL : raw long (not compressed) 
		/// </summary>
		/// <returns></returns>
		long ReadRawLong();
		/// <summary>
		/// 2RD : 2 raw doubles
		/// </summary>
		/// <returns></returns>
		XYZ Read2RawDouble();
		/// <summary>
		/// MC : modular char
		/// </summary>
		/// <returns></returns>
		int ReadModularChar();

		/// <summary>
		/// H : handle reference(see the HANDLE REFERENCES section)
		/// </summary>
		/// <returns></returns>
		ulong HandleReference();
		/// <summary>
		/// H : handle reference(see the HANDLE REFERENCES section)
		/// </summary>
		/// <param name="storeReference"></param>
		/// <returns></returns>
		ulong HandleReference(bool storeReference);
		/// <summary>
		/// H : handle reference(see the HANDLE REFERENCES section)
		/// </summary>
		/// <param name="referenceHandle"></param>
		/// <param name="storeReference"></param>
		/// <returns></returns>
		ulong HandleReference(ulong referenceHandle, bool storeReference);
		/// <summary>
		/// H : handle reference(see the HANDLE REFERENCES section)
		/// </summary>
		/// <param name="referenceHandle"></param>
		/// <param name="storeReference"></param>
		/// <param name="reference"></param>
		/// <returns></returns>
		ulong HandleReference(ulong referenceHandle, bool storeReference, out ReferenceType reference);
		/// <summary>
		/// T : text (bitshort length, followed by the string).
		/// </summary>
		/// <returns></returns>
		string ReadTextUnicode();

		/// <summary>
		/// TV : Variable text, T for 2004 and earlier files, TU for 2007+ files.
		/// </summary>
		/// <returns></returns>
		string ReadVariableText();

		/// <summary>
		/// SN : 16 byte sentinel
		/// </summary>
		/// <returns></returns>
		byte[] ReadSentinel();

		/// <summary>
		/// CMC : CmColor value
		/// </summary>
		/// <returns></returns>
		Color ReadCmColor();

		/// <summary>
		/// OT : Object type
		/// </summary>
		/// <returns></returns>
		ObjectType ReadObjectType();
		#endregion

		/// <summary>
		/// Get the absolute position in the stream in bits.
		/// </summary>
		/// <returns></returns>
		long PositionInBits();
		/// <summary>
		/// Set the position in the stream by bits.
		/// </summary>
		/// <param name="positon"></param>
		void SetPositionInBits(long positon);
		/// <summary>
		/// Advance one byte fordward, saves the last byte.
		/// </summary>
		void AdvanceByte();
		/// <summary>
		/// Advance an offset of bytes fordward, saves the last byte. 
		/// </summary>
		/// <param name="offset"></param>
		void Advance(int offset);
		/// <summary>
		/// Sets the shift displacement to 0.
		/// </summary>
		/// <returns></returns>
		ushort ResetShift();
	}
}