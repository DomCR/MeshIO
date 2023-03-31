namespace MeshIO.Entities.Geometries.Layers
{
	/// <summary>
	/// Defines how the element is referenced by
	/// </summary>
	public enum ReferenceMode
	{
		/// <summary>
		/// Elements are directly referenced
		/// </summary>
		Direct,

		/// <summary>
		/// Elements are referenced by index
		/// </summary>
		Index,

		/// <summary>
		/// Elements are referenced by index and accessed by index
		/// </summary>
		IndexToDirect,
	}
}