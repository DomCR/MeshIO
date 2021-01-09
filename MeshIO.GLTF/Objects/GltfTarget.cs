namespace MeshIO.GLTF
{
	internal class GltfTarget : GltfBase
	{
		/// <summary>
		/// The index of the node to target.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int node { get; set; }
		/// <summary>
		/// The name of the node's TRS property to modify, or the "weights" of the Morph Targets it instantiates. For the "translation" property, the values that are provided by the sampler are the translation along the x, y, and z axes. For the "rotation" property, the values are a quaternion in the order (x, y, z, w), where w is the scalar. For the "scale" property, the values are the scaling factors along the x, y, and z axes.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		/// <value>
		/// "translation", "rotation", "scale", "weights"
		/// </value>
		public string path { get; set; }
	}
}