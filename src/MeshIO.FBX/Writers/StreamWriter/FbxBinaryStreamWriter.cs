using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeshIO.FBX.Writers.StreamWriter
{
	internal sealed class FbxBinaryStreamWriter : FbxStreamWriterBase
	{
		public FbxBinaryStreamWriter(Stream stream, FbxWriterOptions options) : base(options)
		{
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void WriteArray<T>(IEnumerable<T> arr)
		{
			throw new NotImplementedException();
		}

		public override void WriteByteArray(IEnumerable<byte> arr)
		{
			throw new NotImplementedException();
		}

		public override void WriteCloseBracket()
		{
			throw new NotImplementedException();
		}

		public override void WriteEmptyLine()
		{
			throw new NotImplementedException();
		}

		public override void WriteName(string value)
		{
			throw new NotImplementedException();
		}

		public override void WriteOpenBracket()
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(string value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(int value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(long value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(ulong value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(char value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(double value)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(float value)
		{
			throw new NotImplementedException();
		}
	}
}
