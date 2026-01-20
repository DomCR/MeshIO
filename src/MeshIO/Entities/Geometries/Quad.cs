using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries
{
	[Obsolete]
	public class Quad : Polygon
	{
		public int Index0 { get; set; }
		public int Index1 { get; set; }
		public int Index2 { get; set; }
		public int Index3 { get; set; }

		public Quad(int i0, int i1, int i2, int i3)
		{
			Index0 = i0;
			Index1 = i1;
			Index2 = i2;
			Index3 = i3;
		}

		public override int[] ToArray()
		{
			return new int[] { Index0, Index1, Index2, Index3 };
		}

		public IEnumerable<Triangle> ToTriangles()
		{
			List<Triangle> list = new List<Triangle>();
			list.Add(new Triangle(Index0, Index1, Index2));
			list.Add(new Triangle(Index2, Index3, Index0));
			return list;
		}
	}
}
