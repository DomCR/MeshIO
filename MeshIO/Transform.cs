using System;
using System.Collections.Generic;
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
		public XYZ Scale
		{
			get
			{
				double x = XYZ.GetLength(this._matrix.m00, this._matrix.m01, this._matrix.m02);
				double y = XYZ.GetLength(this._matrix.m10, this._matrix.m11, this._matrix.m12);
				double z = XYZ.GetLength(this._matrix.m20, this._matrix.m21, this._matrix.m22);
				return new XYZ(x, y, z);
			}
			set
			{
				double x = XYZ.GetLength(this._matrix.m00, this._matrix.m01, this._matrix.m02);
				double y = XYZ.GetLength(this._matrix.m10, this._matrix.m11, this._matrix.m12);
				double z = XYZ.GetLength(this._matrix.m20, this._matrix.m21, this._matrix.m22);
				this._matrix *= Matrix4.CreateScalingMatrix(1.0 / x * value.X, 1.0 / y * value.Y, 1.0 / z * value.Z);
			}
		}

		private Matrix4 _matrix;

		public Transform()
		{
			Translation = XYZ.Zero;
			Rotation = XYZ.Zero;
			Scale = new XYZ(1, 1, 1);
			_matrix.m33 = 1;
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

		/// <summary>
		/// Multiplies two matrices.
		/// </summary>
		/// <returns>A new instance containing the result.</returns>
		public static Matrix4 Multiply(Matrix4 a, Matrix4 b) => new Matrix4(a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30, a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31, a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32, a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33, a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30, a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31, a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32, a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33, a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30, a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31, a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32, a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33, a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30, a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31, a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32, a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33);

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
		public static Matrix4 operator *(Matrix4 a, Matrix4 b) => Matrix4.Multiply(a, b);
	}
}
