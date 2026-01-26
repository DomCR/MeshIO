using System;

namespace MeshIO.Formats.Gltf.Exceptions;

[Serializable]
public class GltfReaderException : Exception
{
	public GltfReaderException(string message) : base(message) { }
}
