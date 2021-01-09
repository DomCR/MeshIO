namespace MeshIO.GLTF
{
	/// <summary>
	/// A perspective camera containing properties to create a perspective projection matrix.
	/// </summary>
	internal class GltfPerspectiveCamera : GltfBase
	{
		/// <summary>
		/// The floating-point aspect ratio of the field of view. When this is undefined, the aspect ratio of the canvas is used.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Minimum: > 0
		/// </value>
		public float? aspectRatio { get; set; }
		/// <summary>
		/// The floating-point vertical field of view in radians.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		/// <value>
		/// Minimum: > 0
		/// </value>
		public float yfov { get; set; }
		/// <summary>
		/// The floating-point distance to the far clipping plane. When defined, zfar must be greater than znear. If zfar is undefined, runtime must use infinite projection matrix.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// Minimum: > 0
		/// </value>
		public float? zfar { get; set; }
		/// <summary>
		/// The floating-point distance to the near clipping plane.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		/// <value>
		/// Minimum: > 0
		/// </value>
		public float znear { get; set; }
	}
}