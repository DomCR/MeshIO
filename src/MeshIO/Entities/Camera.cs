using CSMath;

namespace MeshIO.Entities
{
	public class Camera : Entity
	{
		public XYZ Position { get; set; }

		public XYZ UpVector { get; set; }

		/// <summary>
		/// Camera field of view
		/// </summary>
		public double FieldOfView { get; set; }

		public double FieldOfViewX { get; set; }

		public double FieldOfViewY { get; set; }

		public ProjectionType ProjectionType { get; set; }

		public Camera() : base() { }

		public Camera(string name) : base(name) { }
	}
}
