namespace MeshIO.GLTF
{
	/// <summary>
	/// Metadata about the glTF asset.
	/// </summary>
	internal class GltfAsset : GltfBase
	{
		/// <summary>
		/// A copyright message suitable for display to credit the content creator.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public string copyright { get; set; }
		/// <summary>
		/// Tool that generated this glTF model. Useful for debugging.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public string generator { get; set; }
		/// <summary>
		/// The glTF version that this asset targets.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public string version { get; set; }
		/// <summary>
		/// The minimum glTF version that this asset targets.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public string minVersion { get; set; }
	}
}
