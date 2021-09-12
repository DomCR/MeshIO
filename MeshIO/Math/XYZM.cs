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

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>
		public XYZM(double value) : this(value, value, value, value) { }

		public XYZM(double[] components) : this(components[0], components[1], components[2], components[3]) { }

		/// <inheritdoc/>
		public double[] GetComponents()
		{
			return new double[] { X, Y, Z, M };
		}

		/// <inheritdoc/>
		public XYZM SetComponents(double[] components)
		{
			return new XYZM(components);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (!(obj is XYZM other))
				return false;

			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.M == other.M;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.M.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{X},{Y},{Z},{M}";
		}

		#region Operators
		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
		public static XYZM operator +(XYZM left, XYZM right)
		{
			return left.Add(right);
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static XYZM operator -(XYZM left, XYZM right)
		{
			return left.Substract(right);
		}

		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
		public static XYZM operator *(XYZM left, XYZM right)
		{
			return left.Multiply(right);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="left">The source vector.</param>
		/// <param name="scalar">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZM operator *(XYZM left, double scalar)
		{
			return left * new XYZM(scalar);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="scalar">The scalar value.</param>
		/// <param name="vector">The source vector.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZM operator *(double scalar, XYZM vector)
		{
			return new XYZM(scalar) * vector;
		}

		/// <summary>
		/// Divides the first vector by the second.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
		public static XYZM operator /(XYZM left, XYZM right)
		{
			return left.Divide(right);
		}

		/// <summary>
		/// Divides the vector by the given scalar.
		/// </summary>
		/// <param name="xyzm">The source vector.</param>
		/// <param name="value">The scalar value.</param>
		/// <returns>The result of the division.</returns>
		public static XYZM operator /(XYZM xyzm, double value)
		{
			return xyzm.Divide(new XYZM(value));
		}

		/// <summary>
		/// Negates a given vector.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
		public static XYZM operator -(XYZM value)
		{
			return Zero.Substract(value);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
		public static bool operator ==(XYZM left, XYZM right)
		{
			return left.IsEqual(right);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are not equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are not equal; False if they are equal.</returns>
		public static bool operator !=(XYZM left, XYZM right)
		{
			return !left.IsEqual(right);
		}
		#endregion
	}
}
