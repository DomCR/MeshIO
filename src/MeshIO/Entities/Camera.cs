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

	public XYZ Position { get; set; }

	public ProjectionType ProjectionType { get; set; }

	public XYZ UpVector { get; set; }

	public Camera() : base()
	{
	}

	public Camera(string name) : base(name)
	{
	}
}