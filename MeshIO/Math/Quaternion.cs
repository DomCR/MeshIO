namespace MeshIO
{
	public struct Quaternion
	{
		/// <summary>
		/// Specifies the X-value of the vector component of the Quaternion.
		/// </summary>
		public double X;
		/// <summary>
		/// Specifies the Y-value of the vector component of the Quaternion.
		/// </summary>
		public double Y;
		/// <summary>
		/// Specifies the Z-value of the vector component of the Quaternion.
		/// </summary>
		public double Z;
		/// <summary>
		/// Specifies the rotation component of the Quaternion.
		/// </summary>
		public double W;

		/// <summary>
		/// Returns a Quaternion representing no rotation. 
		/// </summary>
		public static Quaternion Identity
		{
			get { return new Quaternion(0, 0, 0, 1); }
		}

		/// <summary>
		/// Constructs a Quaternion from the given components.
		/// </summary>
		/// <param name="x">The X component of the Quaternion.</param>
		/// <param name="y">The Y component of the Quaternion.</param>
		/// <param name="z">The Z component of the Quaternion.</param>
		/// <param name="w">The W component of the Quaternion.</param>
		public Quaternion(double x, double y, double z, double w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		/// <summary>
		/// Constructs a Quaternion from the given vector and rotation parts.
		/// </summary>
		/// <param name="vectorPart">The vector part of the Quaternion.</param>
		/// <param name="scalarPart">The rotation part of the Quaternion.</param>
		public Quaternion(XYZ vectorPart, double scalarPart)
		{
			X = vectorPart.X;
			Y = vectorPart.Y;
			Z = vectorPart.Z;
			W = scalarPart;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{X},{Y},{Z},{W}";
		}
	}
}
