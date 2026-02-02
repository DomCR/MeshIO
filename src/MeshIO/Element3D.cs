using System;

namespace MeshIO;

/// <summary>
/// Represents the base class for 3D elements, providing common properties and functionality for objects in a 3D
/// environment.
/// </summary>
/// <remarks><see cref="Element3D"/> serves as the foundational type for 3D objects, encapsulating identity, naming, and
/// extensible metadata through custom properties. Derived classes can leverage these features to represent specific 3D
/// entities. The class supports unique identification and property extension, making it suitable for scenarios where
/// objects require persistent identity and flexible metadata.</remarks>
public abstract class Element3D
{
	/// <summary>
	/// Gets the unique identifier associated with this instance, if available.
	/// </summary>
	public ulong? Id { get; internal set; } = null;

	/// <summary>
	/// Gets or sets the name associated with the object.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets the collection of custom properties associated with this object.
	/// </summary>
	/// <remarks>The returned collection provides access to key-value pairs that can be used to store and retrieve
	/// additional metadata. The collection is read-only; to modify its contents, use the methods provided by the <see
	/// cref="PropertyCollection"/> class.</remarks>
	public PropertyCollection Properties { get; }

	/// <summary>
	/// Initializes a new instance of the Element3D class with default settings.
	/// </summary>
	/// <remarks>This constructor creates an Element3D object with an empty name. Use this overload when you do not
	/// need to specify a name during initialization.</remarks>
	public Element3D() : this(string.Empty) { }

	/// <summary>
	/// Initializes a new instance of the Element3D class with the specified name.
	/// </summary>
	/// <param name="name">The name to assign to the element. This value is used to identify the element within the 3D model.</param>
	public Element3D(string name)
	{
		this.Name = name;
		this.Id = IdUtils.CreateId();
		this.Properties = new PropertyCollection(this);
	}

	/// <summary>
	/// Returns the object's identifier if set; otherwise, generates and assigns a new identifier, then returns it.
	/// </summary>
	/// <remarks>This method ensures that the returned identifier is always non-negative. If the identifier is not
	/// set, it is created using the default identifier generation logic and stored for future calls.</remarks>
	/// <returns>A non-negative 64-bit integer representing the object's identifier. If the identifier was not previously set, a new
	/// one is generated and assigned before returning.</returns>
	public long GetIdOrDefault()
	{
		if (!Id.HasValue)
		{
			this.Id = IdUtils.CreateId();
		}

		return Math.Abs((long)Id.Value);
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{this.GetType().FullName}:{this.Name}";
	}
}
