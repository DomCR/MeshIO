namespace MeshIO.GLTF
{
	/// <summary>
	/// Image data used to create a texture. Image can be referenced by URI or bufferView index. mimeType is required in the latter case.
	/// </summary>
	internal class GltfImage : GltfNamedAsset
	{
		/// <summary>
		/// The uri of the image. Relative paths are relative to the .gltf file. Instead of referencing an external file, the uri can also be a data-uri. The image format must be jpg or png.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Format: uriref
		/// </value>
		public string uri { get; set; }
		/// <summary>
		/// The image's MIME type. Required if bufferView is defined.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Allowed values: "image/jpeg", "image/png"
		/// </value>
		public string mimeType { get; set; }
		/// <summary>
		/// The index of the bufferView that contains the image. Use this instead of the image's uri property.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int bufferView { get; set; }
	}
}