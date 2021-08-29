using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	public struct XY : IVector<XY>
	{
		public readonly static XY Zero = new XY(0, 0);
		public readonly static XY AxisX = new XY(1, 0);
		public readonly static XY AxisY = new XY(0, 1);

		public double X { get; set; }
		public double Y { get; set; }

		public XY(double x, double y)
		{
			X = x;
			Y = y;
		}

		public XY(double[] components) : this(components[0], components[1]) { }

		public double[] GetComponents()
		{
			return new double[] { X, Y };
		}

		public XY SetComponents(double[] components)
		{
			return new XY(components);
		}

		public override string ToString()
		{
			return $"{X},{Y}";
		}
	}
}
