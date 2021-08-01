namespace MeshIO.GLTF
{
	/// <summary>
	/// A camera's projection. A node can reference a camera to apply a transform to place the camera in the scene.
	/// </summary>
	internal class GltfCamera : GltfNamedAsset
	{
		/// <summary>
		/// An orthographic camera containing properties to create an orthographic projection matrix.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public GltfOrthographicCamera orthographic { get; set; }
		/// <summary>
		/// A perspective camera containing properties to create a perspective projection matrix.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public GltfPerspectiveCamera perspective { get; set; }
		/// <summary>
		/// Specifies if the camera uses a perspective or orthographic projection. Based on this, either the camera's perspective or orthographic property will be defined.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		/// <value>
		/// "perspective", "orthographic"
		/// </value>
		public string type { get; set; }
	}
}