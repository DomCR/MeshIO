using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MeshIO.FBX.Writers.StreamWriter
{
	internal sealed class FbxAsciiStreamWriter : FbxStreamWriterBase
	{
		private int _indentation;

		private int _currItem;

		private bool _isEndSection;

		private System.IO.StreamWriter _streamWriter;

		public FbxAsciiStreamWriter(Stream stream, Encoding encoding)
		{
			_streamWriter = new System.IO.StreamWriter(stream, encoding);
		}

		public override void Dispose()
		{
			_streamWriter.Flush();
			_streamWriter.Dispose();
		}

		public override void WriteComment(string comment)
		{
			this.writeIndentation();

			this._streamWriter.Write(";");

			for (int i = 0; i < comment.Length; i++)
			{
				char c = comment[i];

				if (c != '\r' && c != '\n')
				{
					this._streamWriter.Write(c);
				}
			}
			this._streamWriter.WriteLine();
		}

		public override void WriteArray<T>(IEnumerable<T> arr)
		{
			_streamWriter.Write('*');
			_streamWriter.Write(arr.Count());

			WriteOpenBracket();

			WriteName("a");

			if (arr is IEnumerable<byte> bytes)
			{
				WriteByteArray(bytes);
			}

			foreach (T v in arr)
			{
				WriteValue(v);
			}

			WriteEmptyLine();
			WriteCloseBracket();
		}

		public override void WriteByteArray(IEnumerable<byte> arr)
		{
			//Bytes are writen different in fbx
			throw new NotImplementedException();
		}

		public override void WriteCloseBracket()
		{
			_indentation = _indentation - 1;
			writeIndentation();
			_streamWriter.WriteLine("}");
			_isEndSection = true;
		}

		public override void WriteEmptyLine()
		{
			if (!_isEndSection)
			{
				_streamWriter.WriteLine();
			}
		}

		public override void WriteName(string value)
		{
			if (_indentation == 0)
			{
				_streamWriter.WriteLine();
			}

			writeIndentation();
			_streamWriter.Write(value);
			_streamWriter.Write(": ");
			_isEndSection = false;
			_currItem = 0;
		}

		public override void WriteOpenBracket()
		{
			_streamWriter.WriteLine(" {");
			_indentation += 1;
		}

		public override void WriteValue(string value)
		{
			if (value == null)
			{
				WriteValue(string.Empty);
				return;
			}

			writeSeparator();

			if (_currItem > 1)
			{
				_streamWriter.Write(' ');
			}

			_streamWriter.Write('"');

			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				switch (c)
				{
					case '\r':
						_streamWriter.Write("&cr;");
						break;
					case '\n':
						_streamWriter.Write("&lf;");
						break;
					case '"':
						_streamWriter.Write("&quot;");
						break;
					default:
						_streamWriter.Write(c);
						break;
				}
			}

			_streamWriter.Write('"');
		}

		public override void WriteValue(int value)
		{
			writeSeparator();
			_streamWriter.Write(value);
		}

		public override void WriteValue(long value)
		{
			writeSeparator();
			_streamWriter.Write(value);
		}

		public override void WriteValue(ulong value)
		{
			writeSeparator();
			_streamWriter.Write(value);
		}

		public override void WriteValue(char value)
		{
			writeSeparator();
			_streamWriter.Write(value);
		}

		public override void WriteValue(double value)
		{
			this.writeSeparator();
			this._streamWriter.Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}

		public override void WriteValue(float value)
		{
			writeSeparator();
			_streamWriter.Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}

		private void writeIndentation()
		{
			_streamWriter.Write(new string('\t', _indentation));
		}

		private void writeSeparator()
		{
			if (_currItem > 0)
			{
				_streamWriter.Write(',');
			}

			_currItem = _currItem + 1;
		}
	}
}
