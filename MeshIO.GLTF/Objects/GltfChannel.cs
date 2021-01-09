namespace MeshIO.GLTF
{
	/// <summary>
	/// Targets an animation's sampler at a node's property.
	/// </summary>
	internal class GltfChannel :GltfBase
	{
		/// <summary>
		/// The index of a sampler in this animation used to compute the value for the target, e.g., a node's translation, rotation, or scale (TRS).
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int sampler { get; set; }
		/// <summary>
		/// The index of the node and TRS property to target.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfTarget target { get; set; }
	}
}