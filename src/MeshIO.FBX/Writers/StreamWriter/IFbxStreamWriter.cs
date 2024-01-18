using System;
using System.Collections.Generic;

namespace MeshIO.FBX.Writers
{
	internal interface IFbxStreamWriter : IDisposable
	{
		void WriteArray<T>(IEnumerable<T> arr);
		void WriteByteArray(IEnumerable<byte> arr);
		void WriteCloseBracket();
		void WriteComment(string comment);
		void WriteEmptyLine();
		void WriteName(string value);
		void WriteOpenBracket();
		void WritePairNodeValue(string name, object[] arr);
		void WritePairNodeValue(string name, double value);
		void WritePairNodeValue(string name, long value);
		void WritePairNodeValue(string name, char value);
		void WritePairNodeValue(string name, int value);
		void WritePairNodeValue(string name, string value);
		void WritePairNodeValue(string name, byte[] value);
		void WritePairNodeValue<T>(string name, IEnumerable<T> value);
		void WriteValue(string value);
		void WriteValue(int value);
		void WriteValue(long value);
		void WriteValue(ulong value);
		void WriteValue(char value);
		void WriteValue(double value);
		void WriteValue(float value);
		void WriteValue(object value);
	}
}
