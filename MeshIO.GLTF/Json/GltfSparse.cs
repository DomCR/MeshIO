namespace MeshIO.GLTF
{
	/// <summary>
	/// Sparse storage of attributes that deviate from their initialization value.
	/// </summary>
	internal class GltfSparse : GltfBase
	{
		/// <summary>
		/// The number of attributes encoded in this sparse accessor.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int count { get; set; }
		/// <summary>
		/// Index array of size count that points to those accessor attributes that deviate from their initialization value. Indices must strictly increase.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfIndices indices { get; set; }
		/// <summary>
		/// Array of size count times number of components, storing the displaced accessor attributes pointed by indices. Substituted values must have the same componentType and number of components as the base accessor.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfValues values { get; set; }
	}
}