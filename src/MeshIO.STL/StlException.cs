using System;

namespace MeshIO.STL
{
	[Serializable]
	public class StlException : Exception
	{
		public StlException(string message) : base(message) { }
	}
}
