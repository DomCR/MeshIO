using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO
{
	public class Color
	{
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
		public Color()
		{
			R = 0;
			G = 0;
			B = 0;
		}
		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
	}
}
