namespace MeshIO
{
	public struct XYZ : IVector<XYZ>
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

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>
		public XYZ(double value) : this(value, value, value) { }

		public XYZ(double[] components) : this(components[0], components[1], components[2]) { }

		/// <summary>
		/// Computes the cross product of two coordinates.
		/// </summary>
		/// <param name="xyz1">The first coordinate.</param>
		/// <param name="xyz2">The second coordinate.</param>
		/// <returns>The cross product.</returns>
		public static XYZ Cross(XYZ xyz1, XYZ xyz2)
		{
			return new XYZ(
				xyz1.Y * xyz2.Z - xyz1.Z * xyz2.Y,
				xyz1.Z * xyz2.X - xyz1.X * xyz2.Z,
				xyz1.X * xyz2.Y - xyz1.Y * xyz2.X);
		}

		public static XYZ FindNormal(XYZ point1, XYZ point2, XYZ point3)
		{
			XYZ a = point2 - point1;
			XYZ b = point3 - point1;

			// N = Cross(a, b)
			XYZ n = XYZ.Cross(a, b);
			XYZ normal = n.Normalize();

			return normal;
		}

		/// <inheritdoc/>
		public double[] GetComponents()
		{
			return new double[] { X, Y, Z };
		}

		/// <inheritdoc/>
		public XYZ SetComponents(double[] components)
		{
			return new XYZ(components);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (!(obj is XYZ other))
				return false;

			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{X},{Y},{Z}";
		}

		#region Operators

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
		public static XYZ operator +(XYZ left, XYZ right)
		{
			return left.Add(right);
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static XYZ operator -(XYZ left, XYZ right)
		{
			return left.Substract(right);
		}

		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
		public static XYZ operator *(XYZ left, XYZ right)
		{
			return left.Multiply(right);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="left">The source vector.</param>
		/// <param name="scalar">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZ operator *(XYZ left, double scalar)
		{
			return left * new XYZ(scalar);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="scalar">The scalar value.</param>
		/// <param name="vector">The source vector.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZ operator *(double scalar, XYZ vector)
		{
			return new XYZ(scalar) * vector;
		}

		/// <summary>
		/// Divides the first vector by the second.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
		public static XYZ operator /(XYZ left, XYZ right)
		{
			return left.Divide(right);
		}

		/// <summary>
		/// Divides the vector by the given scalar.
		/// </summary>
		/// <param name="xyz">The source vector.</param>
		/// <param name="value">The scalar value.</param>
		/// <returns>The result of the division.</returns>
		public static XYZ operator /(XYZ xyz, float value)
		{
			float invDiv = 1.0f / value;

			return new XYZ(xyz.X * invDiv,
							xyz.Y * invDiv,
							xyz.Z * invDiv);
		}

		/// <summary>
		/// Negates a given vector.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
		public static XYZ operator -(XYZ value)
		{
			return Zero.Substract(value);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
		public static bool operator ==(XYZ left, XYZ right)
		{
			return (left.X == right.X &&
					left.Y == right.Y &&
					left.Z == right.Z);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are not equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are not equal; False if they are equal.</returns>
		public static bool operator !=(XYZ left, XYZ right)
		{
			return (left.X != right.X ||
					left.Y != right.Y ||
					left.Z != right.Z);
		}
		#endregion
	}
}
