namespace MeshIO
{
	public interface IVector
	{
		/// <summary>
		/// Get the diferent components of a dimensional vector.
		/// </summary>
		/// <returns>Array with the vector components.</returns>
		double[] GetComponents();

		/// <summary>
		/// Create a new <see cref="IVector"/>  with the given components.
		/// </summary>
		/// <param name="components">Components to create the new <see cref="IVector"/></param>
		/// <returns>A new instance of a dimensional vector.</returns>
		IVector SetComponents(double[] components);
	}
}
