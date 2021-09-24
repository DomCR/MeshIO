using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Camera : Element
	{
		#region Enums
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
		#endregion

		public XYZ Position { get; set; }

		public XYZ UpVector { get; set; }

		public double FieldOfView { get; set; }
		
		public double FieldOfViewX { get; set; }

		public double FieldOfViewY { get; set; }

		public Camera() : base() { }

		public Camera(string name) : base(name) { }
	}
}
