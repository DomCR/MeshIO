using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.Geometries.Primitives
{
	public class Sphere : Mesh, IPrimitive
	{
		/// <summary>
		/// Vertices in the <see cref="Sphere"/> will be computed with the get operation.
		/// </summary>
		/// <remarks>
		/// The vertices cannot be set in the <see cref="Sphere"/> class.
		/// </remarks>
		public override List<XYZ> Vertices
		{
			get
			{
				m_vertices = ComputeVertices();
				return m_vertices;
			}
			set { m_vertices = value; }
		}
		private List<XYZ> m_vertices;

		public double Radius { get; set; }

		public List<XYZ> ComputeVertices()
		{
			throw new NotImplementedException();
		}
	}
}
