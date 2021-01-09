using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.Geometries
{
	public class Triangle : Polygon
	{
		public uint Index0 { get; set; }
		public uint Index1 { get; set; }
		public uint Index2 { get; set; }
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Triangle() { }
		/// <summary>
		/// Setup a triangle with the 3 indexes.
		/// </summary>
		/// <param name="i0"></param>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		public Triangle(uint i0, uint i1, uint i2)
		{
			Index0 = i0;
			Index1 = i1;
			Index2 = i2;
		}
	}
}