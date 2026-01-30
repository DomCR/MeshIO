using CSMath;

namespace MeshIO.Entities;

/// <summary>
/// Represents a virtual camera that defines the viewpoint, orientation, and projection settings for rendering a 3D
/// scene.
/// </summary>
/// <remarks>The Camera class encapsulates properties such as position, orientation, field of view, and projection
/// type, which are commonly used to control how a 3D scene is visualized. It can be used to configure the perspective
/// or orthographic projection and to specify the direction and up vector for the camera's orientation.</remarks>
public class Camera : Entity
{
	public double FieldOfView { get; set; }

	public double FieldOfViewX { get; set; }

	public double FieldOfViewY { get; set; }

	public XYZ LookAt { get; set; }

	/// <summary>
	/// Gets or sets the position represented by this instance.
	/// </summary>
	public XYZ Position { get; set; }

	public ProjectionType ProjectionType { get; set; }

	/// <summary>
	/// Gets or sets the up direction vector for the coordinate system.
	/// </summary>
	/// <remarks>The up vector typically defines the vertical orientation in 3D space and is used in calculations
	/// involving camera orientation, object alignment, or coordinate transformations. Changing this value affects how 'up'
	/// is interpreted in related operations.</remarks>
	public XYZ UpVector { get; set; }

	public Camera() : base()
	{
	}

	public Camera(string name) : base(name)
	{
	}
}