using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.Geometries
{
	public class Quad : Polygon
	{
		public int Index0 { get; set; }
		public int Index1 { get; set; }
		public int Index2 { get; set; }
		public int Index3 { get; set; }

		public Quad(int i0, int i1, int i2, int i3)
		{
			this.Index0 = i0;
			this.Index1 = i1;
			this.Index2 = i2;
			this.Index3 = i3;
		}
	}
}
