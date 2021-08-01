namespace MeshIO.GLTF
{
	/// <summary>
	/// Reference to a texture.
	/// </summary>
	internal class GltfOcclusionTextureInfo : GltfTextureInfo
	{
		/// <summary>
		/// A scalar multiplier controlling the amount of occlusion applied. A value of 0.0 means no occlusion. A value of 1.0 means full occlusion. This value affects the resulting color using the formula: occludedColor = lerp(color, color * sampled occlusion texture value, occlusion strength). This value is ignored if the corresponding texture is not specified. This value is linear.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public float? strength { get; set; }
	}
}