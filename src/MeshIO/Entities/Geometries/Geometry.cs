using CSMath;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries;

/// <summary>
/// Represents a geometric entity that defines shape, visibility, and shadow properties within a scene.
/// </summary>
/// <remarks>The Geometry class provides properties for controlling rendering behavior, such as visibility and
/// shadow casting, and exposes collections for managing layers and vertices. It serves as a base for objects that
/// require geometric representation in a scene or drawing context.</remarks>
public class Geometry : Entity
{
	/// <summary>
	/// Gets or sets a value indicating whether the element is visible.
	/// </summary>
	public bool IsVisible { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether the object casts shadows when rendered.
	/// </summary>
	public bool CastShadows { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether the object receives shadows from other objects in the scene.
	/// </summary>
	public bool ReceiveShadows { get; set; } = true;

	/// <summary>
	/// Gets the collection of layers contained in this object.
	/// </summary>
	/// <remarks>The returned collection provides access to all layers, allowing enumeration and retrieval of
	/// individual layers.</remarks>
	public LayerCollection Layers { get; }

	/// <summary>
	/// Gets the collection of vertices that define the shape or geometry.
	/// </summary>
	public List<XYZ> Vertices { get; } = new List<XYZ>();

	public Geometry() : this(string.Empty) { }

	public Geometry(string name) : base(name)
	{
		this.Layers = new LayerCollection(this);
	}
}
