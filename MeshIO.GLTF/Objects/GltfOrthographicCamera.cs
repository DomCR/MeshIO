namespace MeshIO.GLTF
{
	/// <summary>
	/// An orthographic camera containing properties to create an orthographic projection matrix.
	/// </summary>
	internal class GltfOrthographicCamera : GltfBase
	{
		/// <summary>
		/// The floating-point horizontal magnification of the view. Must not be zero.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public float xmag { get; set; }
		/// <summary>
		/// The floating-point vertical magnification of the view. Must not be zero.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public float ymag { get; set; }
		/// <summary>
		/// The floating-point distance to the far clipping plane. zfar must be greater than znear.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public float zfar { get; set; }
		/// <summary>
		/// The floating-point distance to the near clipping plane.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public float znear { get; set; }
	}
}