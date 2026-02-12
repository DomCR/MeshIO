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
	/// <summary>
	/// Gets or sets the aspect ratio of the content, defined as the ratio of width to height.
	/// </summary>
	public double AspectRatio { get; set; }

	public double FarPlane { get; set; } = 1;

	/// <summary>
	/// Gets or sets the camera's field of view angle, in degrees.
	/// </summary>
	/// <remarks>A larger field of view allows more of the scene to be visible, while a smaller value provides a
	/// more zoomed-in perspective. Typical values range from 30 to 120 degrees, depending on the application.</remarks>
	public double FieldOfView { get; set; }

	/// <summary>
	/// Gets or sets the target position in 3D space that the camera or object should face toward.
	/// </summary>
	public XYZ LookAt { get; set; }

	/// <summary>
	/// Gets or sets the distance to the near clipping plane for the camera or view frustum.
	/// </summary>
	/// <remarks>The near plane determines the closest distance at which objects are rendered. Setting this value
	/// too low may result in rendering artifacts due to depth buffer precision limitations.</remarks>
	public double NearPlane { get; set; } = 0;

	public XY OrthographicZoom { get; set; }

	/// <summary>
	/// Gets or sets the position represented by this instance.
	/// </summary>
	public XYZ Position { get; set; }

	/// <summary>
	/// Gets or sets the type of projection used for rendering the scene.
	/// </summary>
	/// <remarks>Use this property to specify whether the scene should be rendered using a perspective or
	/// orthographic projection. Changing the projection type affects how objects are displayed in terms of depth and
	/// scale.</remarks>
	public ProjectionType ProjectionType { get; set; } = ProjectionType.Perspective;

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