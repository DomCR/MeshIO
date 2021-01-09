using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace MeshIO
{
	/// <summary>
	/// XYZ values in a 3D dimensional space.
	/// </summary>
	public class XYZ : IEquatable<XYZ>
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		/// <summary>
		/// Default constructor, initializes the point into origin.
		/// </summary>
		public XYZ()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}
		/// <summary>
		/// Constructor in the plane XY, with Z at 0.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public XYZ(double x, double y) : this()
		{
			X = x;
			Y = y;
		}
		/// <summary>
		/// 3D point constructor.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public XYZ(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		//****************************************************************
		public static XYZ operator *(XYZ xyz, double i)
		{
			return new XYZ(xyz.X * i, xyz.Y * i, xyz.Z * i);
		}
		public static XYZ operator /(XYZ xyz, double i)
		{
			return new XYZ(xyz.X / i, xyz.Y / i, xyz.Z / i);
		}
		//****************************************************************
		/// <inheritdoc/>
		public override bool Equals(object other)
		{
			if (other == null)
				return false;
			if (!(other is XYZ))
				return false;

			return Equals((XYZ)other);
		}
		/// <inheritdoc/>
		public bool Equals(XYZ other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}
		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return new { X, Y, Z }.GetHashCode();
		}
		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.InvariantCulture)}, {Z.ToString(CultureInfo.InvariantCulture)}";
		}
	}
}
