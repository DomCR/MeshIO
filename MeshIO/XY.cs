using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	public struct XY : IVector
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

		public IVector SetComponents(double[] components)
		{
			return new XY(components);
		}

		public override string ToString()
		{
			return $"{X},{Y}";
		}
	}

	public struct XYZ : IVector
	{
		public readonly static XYZ Zero = new XYZ(0, 0, 0);
		public readonly static XYZ AxisX = new XYZ(1, 0, 0);
		public readonly static XYZ AxisY = new XYZ(0, 1, 0);
		public readonly static XYZ AxisZ = new XYZ(0, 0, 1);

		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>
		public XYZ(double value) : this(value, value, value) { }

		public XYZ(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

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
		public IVector SetComponents(double[] components)
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
			return new XYZ(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static XYZ operator -(XYZ left, XYZ right)
		{
			return new XYZ(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
		public static XYZ operator *(XYZ left, XYZ right)
		{
			return new XYZ(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZ operator *(XYZ left, double right)
		{
			return left * new XYZ(right);
		}

		/// <summary>
		/// Multiplies a vector by the given scalar.
		/// </summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>
		public static XYZ operator *(double left, XYZ right)
		{
			return new XYZ(left) * right;
		}

		/// <summary>
		/// Divides the first vector by the second.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
		public static XYZ operator /(XYZ left, XYZ right)
		{
			return new XYZ(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
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
			return Zero - value;
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
		public Quaternion(float x, float y, float z, float w)
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
		public Quaternion(XYZ vectorPart, float scalarPart)
		{
			X = vectorPart.X;
			Y = vectorPart.Y;
			Z = vectorPart.Z;
			W = scalarPart;
		}

		/// <summary>
		/// Creates a Quaternion from the given rotation matrix.
		/// </summary>
		/// <param name="matrix">The rotation matrix.</param>
		/// <returns>The created Quaternion.</returns>
		public static Quaternion CreateFromRotationMatrix(Matrix4 matrix)
		{
			double trace = matrix.m11 + matrix.m22 + matrix.m33;

			Quaternion q = new Quaternion();

			if (trace > 0.0f)
			{
				float s = (float)Math.Sqrt(trace + 1.0f);
				q.W = s * 0.5f;
				s = 0.5f / s;
				q.X = (matrix.m23 - matrix.m32) * s;
				q.Y = (matrix.m31 - matrix.m13) * s;
				q.Z = (matrix.m12 - matrix.m21) * s;
			}
			else
			{
				if (matrix.m11 >= matrix.m22 && matrix.m11 >= matrix.m33)
				{
					float s = (float)Math.Sqrt(1.0f + matrix.m11 - matrix.m22 - matrix.m33);
					float invS = 0.5f / s;
					q.X = 0.5f * s;
					q.Y = (matrix.m12 + matrix.m21) * invS;
					q.Z = (matrix.m13 + matrix.m31) * invS;
					q.W = (matrix.m23 - matrix.m32) * invS;
				}
				else if (matrix.m22 > matrix.m33)
				{
					float s = (float)Math.Sqrt(1.0f + matrix.m22 - matrix.m11 - matrix.m33);
					float invS = 0.5f / s;
					q.X = (matrix.m21 + matrix.m12) * invS;
					q.Y = 0.5f * s;
					q.Z = (matrix.m32 + matrix.m23) * invS;
					q.W = (matrix.m31 - matrix.m13) * invS;
				}
				else
				{
					float s = (float)Math.Sqrt(1.0f + matrix.m33 - matrix.m11 - matrix.m22);
					float invS = 0.5f / s;
					q.X = (matrix.m31 + matrix.m13) * invS;
					q.Y = (matrix.m32 + matrix.m23) * invS;
					q.Z = 0.5f * s;
					q.W = (matrix.m12 - matrix.m21) * invS;
				}
			}

			return q;
		}
	}

	public static class VectorExtensions
	{
		public static double GetLength<T>(this T vector)
			where T : IVector
		{
			double length = 0;

			foreach (var item in vector.GetComponents())
			{
				length += item * item;
			}

			return Math.Sqrt(length);
		}

		public static T Normalize<T>(this T vector)
			where T : IVector
		{
			double length = vector.GetLength();
			double[] components = vector.GetComponents();

			for (int i = 0; i < components.Length; i++)
			{
				components[i] /= length;
			}

			return (T)vector.SetComponents(components);
		}

		public static double Dot<T>(this T vector1, T vector2)
			where T : IVector
		{
			var components1 = vector1.GetComponents();
			var components2 = vector2.GetComponents();
			double result = 0;

			for (int i = 0; i < components1.Length; i++)
			{
				result += components1[i] * components2[i];
			}

			return result;
		}
	}
}
