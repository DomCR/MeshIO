namespace MeshIO.GLTF
{
	internal class GlbAttribute
	{
		/// <summary>
		/// XYZ vertex positions.
		/// </summary>
		public int? POSITION { get; set; }
		/// <summary>
		/// Normalized XYZ vertex normals.
		/// </summary>
		public int? NORMAL { get; set; }
		/// <summary>
		/// XYZW vertex tangents where the w component is a sign value (-1 or +1) indicating handedness of the tangent basis.
		/// </summary>
		public int? TANGENT { get; set; }
		/// <summary>
		/// UV texture coordinates for the first set.
		/// </summary>
		public int? TEXCOORD_0 { get; set; }
		/// <summary>
		/// UV texture coordinates for the second set.
		/// </summary>
		public int? TEXCOORD_1 { get; set; }
		/// <summary>
		/// RGB or RGBA vertex color.
		/// </summary>
		public int? COLOR_0 { get; set; }
		/// <summary>
		/// See Skinned Mesh Attributes.
		/// </summary>
		public int? JOINTS_0 { get; set; }
		/// <summary>
		/// See Skinned Mesh Attributes.
		/// </summary>
		public int? WEIGHTS_0 { get; set; }
	}
}
