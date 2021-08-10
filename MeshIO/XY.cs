using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	public interface IVector : IEnumerable<double>
	{

	}

	public struct XY
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

		public override string ToString()
		{
			return $"{X},{Y}";
		}
	}

	public struct XYZ
	{
		public readonly static XYZ Zero = new XYZ(0, 0, 0);
		public readonly static XYZ AxisX = new XYZ(1, 0, 0);
		public readonly static XYZ AxisY = new XYZ(0, 1, 0);
		public readonly static XYZ AxisZ = new XYZ(0, 0, 1);

		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public XYZ(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public double GetLength()
		{
			return GetLength(this);
		}

		public static double GetLength(XYZ xyz)
		{
			return GetLength(xyz.X, xyz.Y, xyz.Z);
		}

		public static double GetLength(double X, double Y, double Z)
		{
			return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
		}

		public override string ToString()
		{
			return $"{X},{Y},{Z}";
		}
	}

	public struct XYZM
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
		public override string ToString()
		{
			return $"{X},{Y},{Z},{M}";
		}
	}

	public static class VectorExtensions
	{
		public static double GetLength<T>(this T vector)
			where T : IVector
		{
			throw new NotImplementedException();
		}
	}
}
