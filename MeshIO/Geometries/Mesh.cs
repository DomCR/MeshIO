using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshIO.Geometries
{
	/// <summary>
	/// Stores the 3D information of an object.
	/// </summary>
	public class Mesh
	{
		/// <summary>
		/// Max vertices allowed in a single mesh.
		/// </summary>
		/// <remarks>
		/// This limit is based on 3D max and Unity 3D.
		/// </remarks>
		public const uint MaxVertices = 65535;

		/// <summary>
		/// Name of this element.
		/// </summary>
		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(m_name))
					m_name = Guid.NewGuid().ToString("N").ToUpper();

				return m_name;
			}
			set
			{
				m_name = value;
			}
		}
		private string m_name;
		/// <summary>
		/// Is not valid if the vertices are empty or above max.
		/// </summary>
		public bool IsValid
		{
			get
			{
				if (Vertices.Count > MaxVertices)
					return false;
				if (!Vertices.Any())
					return false;

				return true;
			}
		}
		public virtual List<XYZ> Vertices { get; set; } = new List<XYZ>();
		public virtual List<Polygon> Polygons { get; set; } = new List<Polygon>();
		/// <summary> 
		/// Return the central point for this geometry.
		/// </summary>
		public XYZ Centroid
		{
			get
			{
				double xcom = 0;
				double ycom = 0;
				double zcom = 0;

				foreach (XYZ pt in Vertices)
				{
					xcom += pt.X;
					ycom += pt.Y;
					zcom += pt.Z;
				}

				if (Vertices.Count > 0)
					return new XYZ(xcom, ycom, zcom) / Vertices.Count;
				else
					return new XYZ();
			}
		}
		/// <summary>
		/// Material for this geometry.
		/// </summary>
		public Material Material { get; set; }
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Mesh() { }
	}
}
