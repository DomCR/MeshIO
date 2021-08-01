namespace MeshIO.GLTF
{
	/// <summary>
	/// Reference to a texture.
	/// </summary>
	internal class GltfNormalTextureInfo : GltfTextureInfo
	{
		/// <summary>
		/// The scalar multiplier applied to each normal vector of the texture. This value scales the normal vector using the formula: scaledNormal = normalize((<sampled normal texture value> * 2.0 - 1.0) * vec3(<normal scale>, <normal scale>, 1.0)). This value is ignored if normalTexture is not specified. This value is linear.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public float? scale { get; set; }
	}
}