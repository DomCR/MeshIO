namespace MeshIO;

/// <summary>
/// Represents a 3D element that is part of a scene.
/// </summary>
/// <remarks>SceneElement serves as a base class for objects that are contained within a Scene. It provides access
/// to the associated Scene and inherits from Element3D. Derived classes should implement specific behaviors or visual
/// representations within the scene.</remarks>
public abstract class SceneElement : Element3D	//TODO: is it needed? scene never assigned
{
	/// <summary>
	/// Gets the scene associated with this instance.
	/// </summary>
	public Scene Scene { get; }

	/// <summary>
	/// Initializes a new instance of the SceneElement class.
	/// </summary>
	public SceneElement() : base() { }

	/// <summary>
	/// Initializes a new instance of the SceneElement class with the specified name.
	/// </summary>
	/// <param name="name">The name to assign to the scene element.</param>
	public SceneElement(string name) : base(name) { }
}
