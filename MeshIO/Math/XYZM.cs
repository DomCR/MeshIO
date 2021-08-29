namespace MeshIO
{
	public struct XYZM : IVector<XYZM>
	{
		public readonly static XYZM Zero = new XYZM(0, 0, 0, 0);
		public readonly static XYZM AxisX = new XYZM(1, 0, 0, 0);
		public readonly static XYZM AxisY = new XYZM(0, 1, 0, 0);
		public readonly static XYZM AxisZ = new XYZM(0, 0, 1, 0);
		public readonly static XYZM AxisM = new XYZM(0, 0, 0, 1);

		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public double M { get; set; }

		public XYZM(double x, double y, double z, double m)
		{
			X = x;
			Y = y;
			Z = z;
			M = m;
		}

		public XYZM(double[] components) : this(components[0], components[1], components[2], components[3]) { }

		public double[] GetComponents()
		{
			return new double[] { X, Y, Z, M };
		}

		public XYZM SetComponents(double[] components)
		{
			return new XYZM(components);
		}

		public override string ToString()
		{
			return $"{X},{Y},{Z},{M}";
		}
	}
}
