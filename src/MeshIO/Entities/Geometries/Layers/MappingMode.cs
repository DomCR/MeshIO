namespace MeshIO.Entities.Geometries.Layers
{
	/// <summary>
	/// Determines how the element is mapped to a surface
	/// </summary>
	public enum MappingMode
	{
		NoMappingInformation,

		/// <summary>
		/// Each element is mapped to the a vertex of the geometry
		/// </summary>
		ByVertex,

		/// <summary>
		/// Elements are mapped to each polygon vertex
		/// </summary>
		ByPolygonVertex,

		/// <summary>
		/// Elements are mapped to each polygon
		/// </summary>
		ByPolygon,

		/// <summary>
		/// Element is mapped to each edge
		/// </summary>
		ByEdge,

		/// <summary>
		/// One element is mapped to the whole geometry
		/// </summary>
		AllSame,
	}
}