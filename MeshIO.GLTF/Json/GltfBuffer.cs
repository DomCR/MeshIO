namespace MeshIO.GLTF
{
	/// <summary>
	/// A buffer points to binary geometry, animation, or skins.
	/// </summary>
	internal class GltfBuffer : GltfNamedAsset
	{
		/// <summary>
		/// The uri of the buffer. Relative paths are relative to the .gltf file.
		/// Instead of referencing an external file, the uri can also be a data-uri.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public string uri { get; set; }
		/// <summary>
		/// The length of the buffer in bytes.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int byteLength { get; set; }
	}
}
