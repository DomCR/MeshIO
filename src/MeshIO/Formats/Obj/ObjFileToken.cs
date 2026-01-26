namespace MeshIO.Formats.Obj;

internal enum ObjFileToken
{
	Undefined = 0,
	/// <summary>
	/// o
	/// </summary>
	Object,
	/// <summary>
	/// g
	/// </summary>
	Group,
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
	/// s
	/// </summary>
	SmoothShading,
	/// <summary>
	/// #
	/// </summary>
	Comment
}
