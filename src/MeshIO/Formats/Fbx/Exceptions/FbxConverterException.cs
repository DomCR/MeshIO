using System;

namespace MeshIO.Formats.Fbx.Exceptions
{
	[Serializable]
	public class FbxConverterException : Exception
	{
		public FbxConverterException(string message) : base(message) { }

		public FbxConverterException(string message, Exception inner) : base(message, inner) { }
	}
}
