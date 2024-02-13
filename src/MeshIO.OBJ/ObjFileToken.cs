namespace MeshIO.OBJ
{
	internal enum ObjFileToken
	{
		Undefined = 0,
		/// <summary>
		/// o
		/// </summary>
		Object,
		/// <summary>
		/// v
		/// </summary>
		Vertice,
		/// <summary>
		/// vt
		/// </summary>
		TextureVertice,
		/// <summary>
		/// vn
		/// </summary>
		Normal,
		/// <summary>
		/// f
		/// </summary>
		Face,
		/// <summary>
		/// #
		/// </summary>
		Comment
	}
}
