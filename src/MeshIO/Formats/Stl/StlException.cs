using System;

namespace MeshIO.Formats.Stl;

[Serializable]
public class StlException : Exception
{
	public StlException(string message) : base(message) { }
}
