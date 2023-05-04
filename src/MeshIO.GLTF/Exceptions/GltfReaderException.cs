using System;

namespace MeshIO.GLTF.Exceptions
{
	[Serializable]
	public class GltfReaderException : Exception
	{
		public GltfReaderException(string message) : base(message) { }

		/// <summary>
		/// An error at a binary stream offset.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="position"></param>
		public GltfReaderException(string message, long position) :
			base($"{message}, near offset {position}")
		{ }

		public GltfReaderException(string message, int accIndex) :
			base($"{message}, accessor index: {accIndex}")
		{ }
	}
}
