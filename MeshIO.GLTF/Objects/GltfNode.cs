using System.Collections.Generic;

namespace MeshIO.GLTF
{
	/// <summary>
	/// A node in the node hierarchy.
	/// When the node contains skin, all mesh.primitives must contain 
	/// JOINTS_0 and WEIGHTS_0 attributes. A node can have either a matrix or any combination 
	/// of translation/rotation/scale (TRS) properties. TRS properties are converted to matrices
	/// and postmultiplied in the T * R * S order to compose the transformation matrix; first the
	/// scale is applied to the vertices, then the rotation, and then the translation. If none are
	/// provided, the transform is the identity. When a node is targeted for animation (referenced
	/// by an animation.channel.target), only TRS properties may be present; matrix will not be present.
	/// </summary>
	internal class GltfNode : GltfNamedAsset
	{
		/// <summary>
		/// The index of the camera referenced by this node.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? camera { get; set; }
		/// <summary>
		/// The indices of this node's children.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Each element in the array must be unique.
		/// Each element in the array must be greater than or equal to 0.
		/// </value>
		public List<int> children { get; set; }
		/// <summary>
		/// The index of the skin referenced by this node. When a skin is referenced by a node within a scene, all joints used by the skin must belong to the same scene.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? skin { get; set; }
		/// <summary>
		/// A floating-point 4x4 transformation matrix stored in column-major order.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// array [16]
		/// </value>
		public List<float> matrix { get; set; } = new List<float>(new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 });
		/// <summary>
		/// The index of the mesh in this node.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? mesh { get; set; }
		/// <summary>
		/// The node's unit quaternion rotation in the order (x, y, z, w), where w is the scalar.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// array [4]
		/// </value>
		public List<float> rotation { get; set; } = new List<float>(new float[] { 0, 0, 0, 1 });
		/// <summary>
		/// The node's non-uniform scale, given as the scaling factors along the x, y, and z axes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// array [3]
		/// </value>
		public List<float> scale { get; set; } = new List<float>(new float[] { 1, 1, 1 });
		/// <summary>
		/// The node's translation along the x, y, and z axes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// array [3]
		/// </value>
		public List<float> translation { get; set; } = new List<float>(new float[] { 0, 0, 0 });
		/// <summary>
		/// The weights of the instantiated Morph Target. Number of elements must match number of Morph Targets of used mesh.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// array [1-*]
		/// </value>
		public List<float> weights { get; set; } 
	}
}
