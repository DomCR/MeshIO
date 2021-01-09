using System.Collections.Generic;

namespace MeshIO.GLTF
{
	/// <summary>
	/// A keyframe animation.
	/// </summary>
	internal class GltfAnimation : GltfNamedAsset
	{
		/// <summary>
		/// An array of channels, each of which targets an animation's sampler at a node's property. Different channels of the same animation can't have equal targets.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public List<GltfChannel> channels { get; set; }
		/// <summary>
		/// An array of samplers that combines input and output accessors with an interpolation algorithm to define a keyframe graph (but not its target).
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public List<GltfSampler> samplers { get; set; }
	}
}