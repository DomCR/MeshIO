using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	public class Transform
	{
		public XYZ Translation
		{
			get
			{
				return new XYZ(_matrix.m30, _matrix.m31, _matrix.m32);
			}
			set
			{
				_matrix.m30 = value.X;
				_matrix.m31 = value.Y;
				_matrix.m32 = value.Z;
			}
		}
		public XYZ Rotation { get; set; }
		public Quaternion Quaternion
		{
			get { throw new NotImplementedException(); }
			set
			{
				// Compute rotation matrix.
				double x2 = value.X + value.X;
				double y2 = value.Y + value.Y;
				double z2 = value.Z + value.Z;

				double wx2 = value.W * x2;
				double wy2 = value.W * y2;
				double wz2 = value.W * z2;
				double xx2 = value.X * x2;
				double xy2 = value.X * y2;
				double xz2 = value.X * z2;
				double yy2 = value.Y * y2;
				double yz2 = value.Y * z2;
				double zz2 = value.Z * z2;

				double q11 = 1.0f - yy2 - zz2;
				double q21 = xy2 - wz2;
				double q31 = xz2 + wy2;

				double q12 = xy2 + wz2;
				double q22 = 1.0f - xx2 - zz2;
				double q32 = yz2 - wx2;

				double q13 = xz2 - wy2;
				double q23 = yz2 + wx2;
				double q33 = 1.0f - xx2 - yy2;

				Matrix4 result;

				// First row
				result.m00 = this._matrix.m00 * q11 + this._matrix.m01 * q21 + this._matrix.m02 * q31;
				result.m01 = this._matrix.m00 * q12 + this._matrix.m01 * q22 + this._matrix.m02 * q32;
				result.m02 = this._matrix.m00 * q13 + this._matrix.m01 * q23 + this._matrix.m02 * q33;
				result.m03 = this._matrix.m03;

				// Second row
				result.m10 = this._matrix.m10 * q11 + this._matrix.m11 * q21 + this._matrix.m12 * q31;
				result.m11 = this._matrix.m10 * q12 + this._matrix.m11 * q22 + this._matrix.m12 * q32;
				result.m12 = this._matrix.m10 * q13 + this._matrix.m11 * q23 + this._matrix.m12 * q33;
				result.m13 = this._matrix.m13;

				// Third row
				result.m20 = this._matrix.m20 * q11 + this._matrix.m21 * q21 + this._matrix.m22 * q31;
				result.m21 = this._matrix.m20 * q12 + this._matrix.m21 * q22 + this._matrix.m22 * q32;
				result.m22 = this._matrix.m20 * q13 + this._matrix.m21 * q23 + this._matrix.m22 * q33;
				result.m23 = this._matrix.m23;

				// Fourth row
				result.m30 = this._matrix.m30 * q11 + this._matrix.m31 * q21 + this._matrix.m32 * q31;
				result.m31 = this._matrix.m30 * q12 + this._matrix.m31 * q22 + this._matrix.m32 * q32;
				result.m32 = this._matrix.m30 * q13 + this._matrix.m31 * q23 + this._matrix.m32 * q33;
				result.m33 = this._matrix.m33;

				_matrix = result;
			}
		}
		public XYZ Scale
		{
			get
			{
				double x = new XYZ(this._matrix.m00, this._matrix.m01, this._matrix.m02).GetLength();
				double y = new XYZ(this._matrix.m10, this._matrix.m11, this._matrix.m12).GetLength();
				double z = new XYZ(this._matrix.m20, this._matrix.m21, this._matrix.m22).GetLength();
				return new XYZ(x, y, z);
			}
			set
			{
				double x = new XYZ(this._matrix.m00, this._matrix.m01, this._matrix.m02).GetLength();
				double y = new XYZ(this._matrix.m10, this._matrix.m11, this._matrix.m12).GetLength();
				double z = new XYZ(this._matrix.m20, this._matrix.m21, this._matrix.m22).GetLength();
				this._matrix *= Matrix4.CreateScalingMatrix(1.0 / x * value.X, 1.0 / y * value.Y, 1.0 / z * value.Z);
				//this._matrix *= Matrix4.CreateScalingMatrix(value.X, value.Y, value.Z);
			}
		}

		private Matrix4 _matrix;

		public Transform()
		{
			_matrix = Matrix4.Identity;
			Translation = XYZ.Zero;
			Rotation = XYZ.Zero;
			Quaternion = Quaternion.Identity;
			Scale = new XYZ(1, 1, 1);
			_matrix.m33 = 1;
		}

		public Transform(Matrix4 matrix)
		{
			this._matrix = matrix;
		}

		public static bool DecomposeV1(Matrix4 matrix, out XYZ scale, out Quaternion rotation, out XYZ translation)
		{
			bool result = true;

			Matrix4 matTemp = Matrix4.Identity;
			//CanonicalBasis canonicalBasis = new CanonicalBasis();

			throw new NotImplementedException();
		}

		public static bool TryDecompose(Transform transform, out XYZ translation, out XYZ scaling, out Quaternion rotation)
		{
			Matrix4 matrix = transform._matrix;

			translation = new XYZ();
			scaling = new XYZ();
			rotation = new Quaternion();
			var XYZDouble = new XYZ();
			var vector4none = new XYZM();

			if (matrix.m33 == 0.0)
				return false;

			//matrix = matrix.Normalize();
			Matrix4 matrix4_3 = matrix;
			matrix4_3.m03 = 0.0;
			matrix4_3.m13 = 0.0;
			matrix4_3.m23 = 0.0;
			matrix4_3.m33 = 1.0;

			if (matrix4_3.GetDeterminant() == 0.0)
				return false;

			if (matrix.m03 != 0.0 || matrix.m13 != 0.0 || matrix.m23 != 0.0)
			{
				XYZM vector4_1 = new XYZM();
				vector4_1.X = matrix.m03;
				vector4_1.Y = matrix.m13;
				vector4_1.Z = matrix.m23;
				vector4_1.M = matrix.m33;

				if (!Matrix4.Inverse(matrix, out Matrix4 inverse))
				{
					return false;
				}

				XYZM vector4_2 = inverse.Transpose() * vector4_1;
				vector4none.X = vector4_2.X;
				vector4none.Y = vector4_2.Y;
				vector4none.Z = vector4_2.Z;
				vector4none.M = vector4_2.M;

				matrix.m03 = matrix.m13 = matrix.m23 = 0.0;
				matrix.m33 = 1.0;
			}
			else
			{
				vector4none = new XYZM();
				vector4none.M = 1.0;
			}

			translation.X = matrix.m30;
			matrix.m30 = 0.0;
			translation.Y = matrix.m31;
			matrix.m31 = 0.0;
			translation.Z = matrix.m32;
			matrix.m32 = 0.0;

			XYZ[] xyzMatrix = new XYZ[3]
			{
			  new XYZ(matrix.m00, matrix.m01, matrix.m02),
			  new XYZ(matrix.m10, matrix.m11, matrix.m12),
			  new XYZ(matrix.m20, matrix.m21, matrix.m22)
			};

			scaling.X = xyzMatrix[0].GetLength();
			normalizeTo(ref xyzMatrix[0], 1.0);

			XYZDouble.X = xyzMatrix[0].Dot(xyzMatrix[1]);
			vectorMultiplyAndSum(xyzMatrix[1], xyzMatrix[0], out xyzMatrix[1], 1.0, -XYZDouble.X);

			scaling.Y = xyzMatrix[1].GetLength();
			normalizeTo(ref xyzMatrix[1], 1.0);

			XYZDouble.X /= scaling.Y;

			XYZDouble.Y = xyzMatrix[0].Dot(xyzMatrix[2]);
			vectorMultiplyAndSum(xyzMatrix[2], xyzMatrix[0], out xyzMatrix[2], 1.0, -XYZDouble.Y);

			XYZDouble.Z = xyzMatrix[1].Dot(xyzMatrix[2]);
			vectorMultiplyAndSum(xyzMatrix[2], xyzMatrix[1], out xyzMatrix[2], 1.0, -XYZDouble.Z);

			scaling.Z = xyzMatrix[2].GetLength();
			normalizeTo(ref xyzMatrix[2], 1.0);

			XYZDouble.Y /= scaling.Z;
			XYZDouble.Z /= scaling.X;

			XYZ rhs = XYZ.Cross(xyzMatrix[1], xyzMatrix[2]);
			if (xyzMatrix[0].Dot(rhs) < 0.0)
			{
				for (int index = 0; index < 3; ++index)
				{
					scaling.X *= -1.0;
					xyzMatrix[index].X *= -1.0;
					xyzMatrix[index].Y *= -1.0;
					xyzMatrix[index].Z *= -1.0;
				}
			}

			double trace = xyzMatrix[0].X + xyzMatrix[1].Y + xyzMatrix[2].Z + 1.0;
			double qx;
			double qy;
			double qz;
			double qw;

			if (trace > 0)
			{
				double s = 0.5 / Math.Sqrt(trace);
				qx = (xyzMatrix[2].Y - xyzMatrix[1].Z) * s;
				qy = (xyzMatrix[0].Z - xyzMatrix[2].X) * s;
				qz = (xyzMatrix[1].X - xyzMatrix[0].Y) * s;
				qw = 0.25 / s;
			}
			else if (xyzMatrix[0].X > xyzMatrix[1].Y && xyzMatrix[0].X > xyzMatrix[2].Z)
			{
				double s = Math.Sqrt(1.0 + xyzMatrix[0].X - xyzMatrix[1].Y - xyzMatrix[2].Z) * 2.0;
				qx = 0.25 * s;
				qy = (xyzMatrix[0].Y + xyzMatrix[1].X) / s;
				qz = (xyzMatrix[0].Z + xyzMatrix[2].X) / s;
				qw = (xyzMatrix[2].Y - xyzMatrix[1].Z) / s;
			}
			else if (xyzMatrix[1].Y > xyzMatrix[2].Z)
			{
				double s = Math.Sqrt(1.0 + xyzMatrix[1].Y - xyzMatrix[0].X - xyzMatrix[2].Z) * 2.0;
				qx = (xyzMatrix[0].Y + xyzMatrix[1].X) / s;
				qy = 0.25 * s;
				qz = (xyzMatrix[1].Z + xyzMatrix[2].Y) / s;
				qw = (xyzMatrix[0].Z - xyzMatrix[2].X) / s;
			}
			else
			{
				double s = Math.Sqrt(1.0 + xyzMatrix[2].Z - xyzMatrix[0].X - xyzMatrix[1].Y) * 2.0;
				qx = (xyzMatrix[0].Z + xyzMatrix[2].X) / s;
				qy = (xyzMatrix[1].Z + xyzMatrix[2].Y) / s;
				qz = 0.25 * s;
				qw = (xyzMatrix[1].X - xyzMatrix[0].Y) / s;
			}

			rotation.X = -qx;
			rotation.Y = -qy;
			rotation.Z = -qz;
			rotation.W = qw;

			return true;
		}

		private static void vectorMultiplyAndSum(XYZ vector1, XYZ vector2, out XYZ result, double multiplier1, double multiplier2)
		{
			XYZ vector3_2 = vector1 * multiplier1 + vector2 * multiplier2;
			result = vector3_2;
		}

		private static void normalizeTo(ref XYZ vector, double amount)
		{
			double length = vector.GetLength();

			if (length == 0.0)
				return;

			double average = amount / length;

			vector.X *= average;
			vector.Y *= average;
			vector.Z *= average;
		}
	}

	public struct Matrix4
	{
		/// <summary>
		/// 4-dimensional double precision zero matrix.
		/// </summary>
		public static readonly Matrix4 Zero = new Matrix4(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
		/// <summary>
		/// 4-dimensional double precision identity matrix.
		/// </summary>
		public static readonly Matrix4 Identity = new Matrix4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);

		public double m00;
		public double m01;
		public double m02;
		public double m03;
		public double m10;
		public double m11;
		public double m12;
		public double m13;
		public double m20;
		public double m21;
		public double m22;
		public double m23;
		public double m30;
		public double m31;
		public double m32;
		public double m33;

		public Matrix4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			this.m00 = m00;
			this.m01 = m01;
			this.m02 = m02;
			this.m03 = m03;

			this.m10 = m10;
			this.m11 = m11;
			this.m12 = m12;
			this.m13 = m13;

			this.m20 = m20;
			this.m21 = m21;
			this.m22 = m22;
			this.m23 = m23;

			this.m30 = m30;
			this.m31 = m31;
			this.m32 = m32;
			this.m33 = m33;
		}

		public Matrix4(double[] elements)
		{
			this.m00 = elements[0];
			this.m01 = elements[1];
			this.m02 = elements[2];
			this.m03 = elements[3];
			this.m10 = elements[4];
			this.m11 = elements[5];
			this.m12 = elements[6];
			this.m13 = elements[7];
			this.m20 = elements[8];
			this.m21 = elements[9];
			this.m22 = elements[10];
			this.m23 = elements[11];
			this.m30 = elements[12];
			this.m31 = elements[13];
			this.m32 = elements[14];
			this.m33 = elements[15];
		}

		/// <summary>
		/// Calculates the determinant of the matrix.
		/// </summary>
		/// <returns>The determinant of the matrix.</returns>
		public double GetDeterminant()
		{
			// | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
			// | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
			// | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
			// | m n o p |
			//
			//   | f g h |
			// a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
			//   | n o p |
			//
			//   | e g h |     
			// b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
			//   | m o p |     
			//
			//   | e f h |
			// c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
			//   | m n p |
			//
			//   | e f g |
			// d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
			//   | m n o |
			//
			// Cost of operation
			// 17 adds and 28 muls.
			//
			// add: 6 + 8 + 3 = 17
			// mul: 12 + 16 = 28

			double a = m00, b = m01, c = m02, d = m03;
			double e = m10, f = m11, g = m12, h = m13;
			double i = m20, j = m21, k = m22, l = m23;
			double m = m30, n = m31, o = m32, p = m33;

			double kp_lo = k * p - l * o;
			double jp_ln = j * p - l * n;
			double jo_kn = j * o - k * n;
			double ip_lm = i * p - l * m;
			double io_km = i * o - k * m;
			double in_jm = i * n - j * m;

			return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
				   b * (e * kp_lo - g * ip_lm + h * io_km) +
				   c * (e * jp_ln - f * ip_lm + h * in_jm) -
				   d * (e * jo_kn - f * io_km + g * in_jm);
		}

		/// <summary>
		/// Normalizes this instance.
		/// </summary>
		/// <returns>Normalize matrix4</returns>
		public Matrix4 Normalize()
		{
			double factor = 1.0 / this.m33;
			return new Matrix4(this.m00 * factor, this.m01 * factor, this.m02 * factor, this.m03 * factor, this.m10 * factor, this.m11 * factor, this.m12 * factor, this.m13 * factor, this.m20 * factor, this.m21 * factor, this.m22 * factor, this.m23 * factor, this.m30 * factor, this.m31 * factor, this.m32 * factor, this.m33 * factor);
		}

		/// <summary>
		/// Attempts to calculate the inverse of the given matrix. If successful, result will contain the inverted matrix.
		/// </summary>
		/// <param name="matrix">The source matrix to invert.</param>
		/// <param name="result">If successful, contains the inverted matrix.</param>
		/// <returns>True if the source matrix could be inverted; False otherwise.</returns>
		public static bool Inverse(Matrix4 matrix, out Matrix4 result)
		{
			//                                       -1
			// If you have matrix M, inverse Matrix M   can compute
			//
			//     -1       1      
			//    M   = --------- A
			//            det(M)
			//
			// A is adjugate (adjoint) of M, where,
			//
			//      T
			// A = C
			//
			// C is Cofactor matrix of M, where,
			//           i + j
			// C   = (-1)      * det(M  )
			//  ij                    ij
			//
			//     [ a b c d ]
			// M = [ e f g h ]
			//     [ i j k l ]
			//     [ m n o p ]
			//
			// First Row
			//           2 | f g h |
			// C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
			//  11         | n o p |
			//
			//           3 | e g h |
			// C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
			//  12         | m o p |
			//
			//           4 | e f h |
			// C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
			//  13         | m n p |
			//
			//           5 | e f g |
			// C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
			//  14         | m n o |
			//
			// Second Row
			//           3 | b c d |
			// C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
			//  21         | n o p |
			//
			//           4 | a c d |
			// C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
			//  22         | m o p |
			//
			//           5 | a b d |
			// C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
			//  23         | m n p |
			//
			//           6 | a b c |
			// C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
			//  24         | m n o |
			//
			// Third Row
			//           4 | b c d |
			// C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
			//  31         | n o p |
			//
			//           5 | a c d |
			// C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
			//  32         | m o p |
			//
			//           6 | a b d |
			// C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
			//  33         | m n p |
			//
			//           7 | a b c |
			// C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
			//  34         | m n o |
			//
			// Fourth Row
			//           5 | b c d |
			// C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
			//  41         | j k l |
			//
			//           6 | a c d |
			// C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
			//  42         | i k l |
			//
			//           7 | a b d |
			// C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
			//  43         | i j l |
			//
			//           8 | a b c |
			// C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
			//  44         | i j k |
			//
			// Cost of operation
			// 53 adds, 104 muls, and 1 div.
			double a = matrix.m00, b = matrix.m01, c = matrix.m02, d = matrix.m03;
			double e = matrix.m10, f = matrix.m11, g = matrix.m12, h = matrix.m13;
			double i = matrix.m20, j = matrix.m21, k = matrix.m22, l = matrix.m23;
			double m = matrix.m30, n = matrix.m31, o = matrix.m32, p = matrix.m33;

			double kp_lo = k * p - l * o;
			double jp_ln = j * p - l * n;
			double jo_kn = j * o - k * n;
			double ip_lm = i * p - l * m;
			double io_km = i * o - k * m;
			double in_jm = i * n - j * m;

			double a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
			double a12 = -(e * kp_lo - g * ip_lm + h * io_km);
			double a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
			double a14 = -(e * jo_kn - f * io_km + g * in_jm);

			double det = a * a11 + b * a12 + c * a13 + d * a14;

			if (Math.Abs(det) < double.Epsilon)
			{
				result = new Matrix4(double.NaN, double.NaN, double.NaN, double.NaN,
									   double.NaN, double.NaN, double.NaN, double.NaN,
									   double.NaN, double.NaN, double.NaN, double.NaN,
									   double.NaN, double.NaN, double.NaN, double.NaN);
				return false;
			}

			double invDet = 1.0f / det;

			result.m00 = a11 * invDet;
			result.m10 = a12 * invDet;
			result.m20 = a13 * invDet;
			result.m30 = a14 * invDet;

			result.m01 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
			result.m11 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
			result.m21 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
			result.m31 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

			double gp_ho = g * p - h * o;
			double fp_hn = f * p - h * n;
			double fo_gn = f * o - g * n;
			double ep_hm = e * p - h * m;
			double eo_gm = e * o - g * m;
			double en_fm = e * n - f * m;

			result.m02 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
			result.m12 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
			result.m22 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
			result.m32 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

			double gl_hk = g * l - h * k;
			double fl_hj = f * l - h * j;
			double fk_gj = f * k - g * j;
			double el_hi = e * l - h * i;
			double ek_gi = e * k - g * i;
			double ej_fi = e * j - f * i;

			result.m03 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
			result.m13 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
			result.m23 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
			result.m33 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

			return true;
		}

		/// <summary>
		/// Transposes the rows and columns of this matrix.
		/// </summary>
		/// <returns>The transposed matrix.</returns>
		public Matrix4 Transpose()
		{
			Matrix4 result;

			result.m00 = this.m00;
			result.m01 = this.m10;
			result.m02 = this.m20;
			result.m03 = this.m30;
			result.m10 = this.m01;
			result.m11 = this.m11;
			result.m12 = this.m21;
			result.m13 = this.m31;
			result.m20 = this.m02;
			result.m21 = this.m12;
			result.m22 = this.m22;
			result.m23 = this.m32;
			result.m30 = this.m03;
			result.m31 = this.m13;
			result.m32 = this.m23;
			result.m33 = this.m33;

			return result;
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="position">The amount to translate in each axis.</param>
		/// <returns>The translation matrix.</returns>
		public static Matrix4 CreateTranslation(XYZ position)
		{
			return Matrix4.CreateTranslation(position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="xPosition">The amount to translate on the X-axis.</param>
		/// <param name="yPosition">The amount to translate on the Y-axis.</param>
		/// <param name="zPosition">The amount to translate on the Z-axis.</param>
		/// <returns>The translation matrix.</returns>
		public static Matrix4 CreateTranslation(double xPosition, double yPosition, double zPosition)
		{
			Matrix4 result;

			result.m00 = 1.0f;
			result.m01 = 0.0f;
			result.m02 = 0.0f;
			result.m03 = 0.0f;
			result.m10 = 0.0f;
			result.m11 = 1.0f;
			result.m12 = 0.0f;
			result.m13 = 0.0f;
			result.m20 = 0.0f;
			result.m21 = 0.0f;
			result.m22 = 1.0f;
			result.m23 = 0.0f;

			result.m30 = xPosition;
			result.m31 = yPosition;
			result.m32 = zPosition;
			result.m33 = 1.0f;

			return result;
		}

		/// <summary>
		/// Creates a scaling matrix.
		/// </summary>
		/// <param name="scales">The vector containing the amount to scale by on each axis.</param>
		/// <returns>The scaling matrix.</returns>
		public static Matrix4 CreateScale(XYZ scales)
		{
			return CreateScale(scales, XYZ.Zero);
		}

		/// <summary>
		/// Creates a scaling matrix with a center point.
		/// </summary>
		/// <param name="scales">The vector containing the amount to scale by on each axis.</param>
		/// <param name="centerPoint">The center point.</param>
		/// <returns>The scaling matrix.</returns>
		public static Matrix4 CreateScale(XYZ scales, XYZ centerPoint)
		{
			Matrix4 result;

			double tx = centerPoint.X * (1 - scales.X);
			double ty = centerPoint.Y * (1 - scales.Y);
			double tz = centerPoint.Z * (1 - scales.Z);

			result.m00 = scales.X;
			result.m01 = 0.0f;
			result.m02 = 0.0f;
			result.m03 = 0.0f;
			result.m10 = 0.0f;
			result.m11 = scales.Y;
			result.m12 = 0.0f;
			result.m13 = 0.0f;
			result.m20 = 0.0f;
			result.m21 = 0.0f;
			result.m22 = scales.Z;
			result.m23 = 0.0f;
			result.m30 = tx;
			result.m31 = ty;
			result.m32 = tz;
			result.m33 = 1.0f;

			return result;
		}

		/// <summary>
		/// Creates a uniform scaling matrix that scales equally on each axis.
		/// </summary>
		/// <param name="scale">The uniform scaling factor.</param>
		/// <returns>The scaling matrix.</returns>
		public static Matrix4 CreateScale(double scale)
		{
			return CreateScale(scale, XYZ.Zero);
		}

		/// <summary>
		/// Creates a uniform scaling matrix that scales equally on each axis with a center point.
		/// </summary>
		/// <param name="scale">The uniform scaling factor.</param>
		/// <param name="centerPoint">The center point.</param>
		/// <returns>The scaling matrix.</returns>
		public static Matrix4 CreateScale(double scale, XYZ centerPoint)
		{
			return CreateScale(new XYZ(scale), centerPoint);
		}

		/// <summary>
		/// Creates a matrix that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param>
		/// <param name="angle">The angle to rotate around the given axis, in radians.</param>
		/// <returns>The rotation matrix.</returns>
		public static Matrix4 CreateFromAxisAngle(XYZ axis, double angle)
		{
			// a: angle
			// x, y, z: unit vector for axis.
			//
			// Rotation matrix M can compute by using below equation.
			//
			//        T               T
			//  M = uu + (cos a)( I-uu ) + (sin a)S
			//
			// Where:
			//
			//  u = ( x, y, z )
			//
			//      [  0 -z  y ]
			//  S = [  z  0 -x ]
			//      [ -y  x  0 ]
			//
			//      [ 1 0 0 ]
			//  I = [ 0 1 0 ]
			//      [ 0 0 1 ]
			//
			//
			//     [  xx+cosa*(1-xx)   yx-cosa*yx-sina*z zx-cosa*xz+sina*y ]
			// M = [ xy-cosa*yx+sina*z    yy+cosa(1-yy)  yz-cosa*yz-sina*x ]
			//     [ zx-cosa*zx-sina*y zy-cosa*zy+sina*x   zz+cosa*(1-zz)  ]
			//
			double x = axis.X, y = axis.Y, z = axis.Z;
			double sa = (double)Math.Sin(angle), ca = (double)Math.Cos(angle);
			double xx = x * x, yy = y * y, zz = z * z;
			double xy = x * y, xz = x * z, yz = y * z;

			Matrix4 result = new Matrix4();

			result.m00 = xx + ca * (1.0f - xx);
			result.m01 = xy - ca * xy + sa * z;
			result.m02 = xz - ca * xz - sa * y;
			result.m03 = 0.0f;
			result.m10 = xy - ca * xy - sa * z;
			result.m11 = yy + ca * (1.0f - yy);
			result.m12 = yz - ca * yz + sa * x;
			result.m13 = 0.0f;
			result.m20 = xz - ca * xz + sa * y;
			result.m21 = yz - ca * yz - sa * x;
			result.m22 = zz + ca * (1.0f - zz);
			result.m23 = 0.0f;
			result.m30 = 0.0f;
			result.m31 = 0.0f;
			result.m32 = 0.0f;
			result.m33 = 1.0f;

			return result;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return string.Format("{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
								 m00, m01, m02, m03,
								 m10, m11, m12, m13,
								 m20, m21, m22, m23,
								 m30, m31, m32, m33);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return m00.GetHashCode() + m01.GetHashCode() + m02.GetHashCode() + m03.GetHashCode() +
				   m10.GetHashCode() + m11.GetHashCode() + m12.GetHashCode() + m13.GetHashCode() +
				   m20.GetHashCode() + m21.GetHashCode() + m22.GetHashCode() + m23.GetHashCode() +
				   m30.GetHashCode() + m31.GetHashCode() + m32.GetHashCode() + m33.GetHashCode();
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (!(obj is Matrix4 other))
				return false;

			return m00 == other.m00 && m11 == other.m11 && m22 == other.m22 && m33 == other.m33 &&
				m01 == other.m01 && m02 == other.m02 && m03 == other.m03 &&
				m10 == other.m10 && m12 == other.m12 && m13 == other.m13 &&
				m20 == other.m20 && m21 == other.m21 && m23 == other.m23 &&
				m30 == other.m30 && m31 == other.m31 && m32 == other.m32;
		}

		/// <summary>
		/// Multiplies two matrices.
		/// </summary>
		/// <returns>A new instance containing the result.</returns>
		public static Matrix4 Multiply(Matrix4 a, Matrix4 b)
		{
			return new Matrix4(a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
								a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
								a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
								a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,
								a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
								a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
								a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
								a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
								a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
								a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
								a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
								a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
								a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
								a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
								a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
								a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33);
		}

		/// <summary>
		/// Builds a matrix that scales along the x-axis, y-axis, and z-axis.
		/// </summary>
		public static Matrix4 CreateScalingMatrix(double x, double y, double z)
		{
			return new Matrix4(x, 0.0, 0.0, 0.0, 0.0, y, 0.0, 0.0, 0.0, 0.0, z, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		/// <summary>
		/// Multiplies two matrices.
		/// </summary>
		/// <returns>A new instance containing the result.</returns>
		public static Matrix4 operator *(Matrix4 a, Matrix4 b)
		{
			return Matrix4.Multiply(a, b);
		}

		/// <summary>Multiply the matrix and a coordinate</summary>
		/// <param name="matrix"></param>
		/// <param name="xyz"></param>
		/// <returns>Result matrix</returns>
		public static XYZ operator *(Matrix4 matrix, XYZ xyz)
		{
			XYZ result = new XYZ();

			double num2 = 1.0 / (matrix.m03 * xyz.X + matrix.m13 * xyz.Y + matrix.m23 * xyz.Z + matrix.m33);

			result.X = (matrix.m00 * xyz.X + matrix.m10 * xyz.Y + matrix.m20 * xyz.Z + matrix.m30) * num2;
			result.Y = (matrix.m01 * xyz.X + matrix.m11 * xyz.Y + matrix.m21 * xyz.Z + matrix.m31) * num2;
			result.Z = (matrix.m02 * xyz.X + matrix.m12 * xyz.Y + matrix.m22 * xyz.Z + matrix.m32) * num2;

			return result;
		}

		/// <summary>Multiply the matrix and vector4</summary>
		/// <param name="lhs">Lhs.</param>
		/// <param name="v">V.</param>
		/// <returns>Result matrix</returns>
		public static XYZM operator *(Matrix4 lhs, XYZM v)
		{
			Matrix4 matrix4_1 = lhs;
			if (true)
				;
			double m00 = matrix4_1.m00;
			XYZM XYZM = v;
			if (true)
				;
			double x = XYZM.X;
			double num1 = m00 * x;
			Matrix4 matrix4_2 = lhs;
			if (true)
				;
			double num2 = matrix4_2.m10 * v.Y;
			return new XYZM(num1 + num2 + lhs.m20 * v.Z + lhs.m30 * v.M, lhs.m01 * v.X + lhs.m11 * v.Y + lhs.m21 * v.Z + lhs.m31 * v.M, lhs.m02 * v.X + lhs.m12 * v.Y + lhs.m22 * v.Z + lhs.m32 * v.M, lhs.m03 * v.X + lhs.m13 * v.Y + lhs.m23 * v.Z + lhs.m33 * v.M);
		}
	}
}
