namespace MeshIO.GLTF
{
	/// <summary>
	/// Array of size accessor.sparse.count times number of components storing the displaced accessor attributes pointed by accessor.sparse.indices.
	/// </summary>
	internal class GltfValues : GltfBase
	{
		/// <summary>
		/// The index of the bufferView with sparse values. Referenced bufferView can't have ARRAY_BUFFER or ELEMENT_ARRAY_BUFFER target.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int bufferView { get; set; }
		/// <summary>
		/// The offset relative to the start of the bufferView in bytes. Must be aligned.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int byteOffset { get; set; }
	}
}