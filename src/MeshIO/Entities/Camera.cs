using CSMath;

namespace MeshIO.Entities
{
    public class Camera : Entity
    {
        /// <summary>
        /// Camera's projection types
        /// </summary>
        public enum ProjectionType
        {
            /// <summary>
            /// The camera uses perspective projection
            /// </summary>
            Perspective,

            /// <summary>
            /// The camera uses orthographic projection
            /// </summary>
            Orthographic,
        }

        public XYZ Position { get; set; }

        public XYZ UpVector { get; set; }

        public double FieldOfView { get; set; }

        public double FieldOfViewX { get; set; }

        public double FieldOfViewY { get; set; }

        public Camera() : base() { }

        public Camera(string name) : base(name) { }
    }
}
