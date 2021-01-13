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
			throw new NotImplementedException();
		}
	}
	internal class DwgStreamHandlerAC15 : DwgStreamHandlerAC12
	{
		public DwgStreamHandlerAC15(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadVariableText()
		{
			throw new NotImplementedException();
		}
	}
	internal class DwgStreamHandlerAC18 : DwgStreamHandlerAC15
	{
		public DwgStreamHandlerAC18(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadTextUnicode()
		{
			short textLength = this.ReadShort<LittleEndianConverter>();
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
		public override string ReadVariableText()
		{
			return this.ReadTextUnicode();
		}
	}
	internal class DwgStreamHandlerAC21 : DwgStreamHandlerAC18
	{
		public DwgStreamHandlerAC21(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
		public override string ReadTextUnicode()
		{
			short textLength = this.ReadShort<LittleEndianConverter>();
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
		public override string ReadVariableText()
		{
			return this.ReadTextUnicode();
		}
	}
	internal class DwgStreamHandlerAC24 : DwgStreamHandlerAC21
	{
		public DwgStreamHandlerAC24(Stream stream, bool resetPosition) : base(stream, resetPosition) { }
	}
}
