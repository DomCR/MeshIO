namespace MeshIO.GLTF
{
	/// <summary>
	/// Indices of those attributes that deviate from their initialization value.
	/// </summary>
	internal class GltfIndices : GltfBase
	{
		/// <summary>
		/// The index of the bufferView with sparse indices. Referenced bufferView can't have ARRAY_BUFFER or ELEMENT_ARRAY_BUFFER target.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int bufferView { get; set; }
		/// <summary>
		/// The offset relative to the start of the bufferView in bytes. Must be aligned.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int byteOffset { get; set; }
		/// <summary>
		/// The indices data type. Valid values correspond to WebGL enums: 5121 (UNSIGNED_BYTE), 5123 (UNSIGNED_SHORT), 5125 (UNSIGNED_INT).
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfComponentType componentType { get; set; }
	}
}