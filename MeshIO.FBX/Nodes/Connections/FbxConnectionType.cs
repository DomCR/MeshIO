namespace MeshIO.FBX.Nodes.Connections
{
	/// <summary>
	/// Connection types.
	/// </summary>
	public enum FbxConnectionType
	{
		/// <summary>
		/// Object(source) to Object(destination).
		/// </summary>
		OO,
		/// <summary>
		/// Object(source) to Property(destination).
		/// </summary>
		OP,
		/// <summary>
		/// Property(source) to Object(destination).
		/// </summary>
		PO,
		/// <summary>
		/// Property(source) to Property(destination).
		/// </summary>
		PP
	}
}