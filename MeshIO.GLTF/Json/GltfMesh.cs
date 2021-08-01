using System.Collections.Generic;

namespace MeshIO.GLTF
{
	/// <summary>
	/// A set of primitives to be rendered. A node can contain one mesh. A node's transform places the mesh in the scene.
	/// </summary>
	internal class GltfMesh : GltfNamedAsset
	{
		/// <summary>
		/// An array of primitives, each defining geometry to be rendered with a material.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public List<GltfPrimitive> primitives { get; set; }
		/// <summary>
		/// Array of weights to be applied to the Morph Targets.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<float> weights { get; set; }
	}
}
