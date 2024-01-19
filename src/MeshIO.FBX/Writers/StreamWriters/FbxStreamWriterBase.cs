using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.FBX.Writers.StreamWriters
{
	internal abstract class FbxStreamWriterBase : IFbxStreamWriter
	{
		public static IFbxStreamWriter Create(FbxWriterOptions options, Stream stream)
		{
			if (options.IsBinaryFormat)
			{
				throw new NotImplementedException();
			}
			else
			{
				Encoding encoding = new UTF8Encoding(false);
				return new FbxAsciiStreamWriter(stream, encoding);
			}
		}

		public virtual void WriteComment(string comment)
		{
		}

		public abstract void WriteName(string value);

		public abstract void WriteOpenBracket();

		public abstract void WriteValue(string value);

		public abstract void WriteValue(int value);

		public abstract void WriteValue(long value);

		public abstract void WriteValue(ulong value);

		public abstract void WriteValue(char value);

		public abstract void WriteValue(double value);

		public abstract void WriteValue(float value);

		public abstract void WriteArray<T>(IEnumerable<T> arr);

		public abstract void WriteByteArray(IEnumerable<byte> arr);

		public abstract void WriteCloseBracket();

		public abstract void WriteEmptyLine();

		public void WriteValue(object value)
		{
			if (value == null)
			{
				WriteValue(string.Empty);
				return;
			}

			switch (value)
			{
				case bool b:
					WriteValue(b ? '1' : '0');
					return;
				case int i:
					WriteValue(i);
					return;
				case long l:
					WriteValue(l);
					return;
				case ulong ul:
					WriteValue(ul);
					return;
				case float f:
					WriteValue(f);
					return;
				case double d:
					WriteValue(d);
					return;
				case string s:
					WriteValue(s);
					return;
				default:
					throw new NotSupportedException($"Stream writer does not support {value.GetType().FullName}");
			}
		}

		public void WritePairNodeValue(string name, object[] arr)
		{
			WriteName(name);

			int num = 0;
			while (num < arr.Length)
			{
				object value = arr[num];
				WriteValue(value);
				num = num + 1;
			}
			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, double value)
		{
			WriteName(name);
			WriteValue(value);
			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, long value)
		{
			WriteName(name);
			WriteValue(value);
			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, char value)
		{
			WriteName(name);
			WriteValue(value);
			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, int value)
		{
			WriteName(name);
			WriteValue(value);
			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, string value)
		{
			WriteName(name);

			if (value == null)
			{
				WriteValue(string.Empty);
			}
			else
			{
				WriteValue(value);
			}

			WriteEmptyLine();
		}

		public void WritePairNodeValue(string name, byte[] value)
		{
			WriteName(name);
			WriteByteArray(value);
			WriteEmptyLine();
		}

		public void WritePairNodeValue<T>(string name, IEnumerable<T> value)
		{
			WriteName(name);
			WriteArray(value);
			WriteEmptyLine();
		}

		public abstract void Dispose();
	}
}
