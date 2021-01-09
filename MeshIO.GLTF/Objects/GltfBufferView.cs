namespace MeshIO.GLTF
{
	/// <summary>
	/// A view into a buffer generally representing a subset of the buffer.
	/// </summary>
	internal class GltfBufferView : GltfNamedAsset
	{
		/// <summary>
		/// The index of the buffer.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int buffer { get; set; }
		/// <summary>
		/// The offset into the buffer in bytes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int byteOffset { get; set; } = 0;
		/// <summary>
		/// The length of the bufferView in bytes.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int byteLength { get; set; }
		/// <summary>
		/// The stride, in bytes, between vertex attributes.
		/// When this is not defined, data is tightly packed. 
		/// When two or more accessors use the same bufferView, this field must be defined.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Min >= 4
		/// Max <= 252
		/// </value>
		public int? byteStride { get; set; }
		/// <summary>
		/// The target that the GPU buffer should be bound to.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int target { get; set; }
	}
}
