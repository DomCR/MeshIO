namespace MeshIO.GLTF
{
	internal abstract class GltfTextureInfo : GltfAsset
	{
		/// <summary>
		/// The index of the texture.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int index { get; set; }
		/// <summary>
		/// This integer value is used to construct a string in the format TEXCOORD_<set index> which is a reference to a key in mesh.primitives.attributes (e.g. A value of 0 corresponds to TEXCOORD_0). Mesh must have corresponding texture coordinate attributes for the material to be applicable to it.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? texCoord { get; set; }
	}
}