namespace MeshIO
{
	public interface IVector<T>
	{
		/// <summary>
		/// Get the diferent components of a dimensional vector.
		/// </summary>
		/// <returns>Array with the vector components.</returns>
		double[] GetComponents();

		/// <summary>
		/// Create a new instance of the same type with the given components.
		/// </summary>
		/// <param name="components">Components to create the new IVector</param>
		/// <returns>A new instance of a dimensional vector.</returns>
		T SetComponents(double[] components);
	}
}
