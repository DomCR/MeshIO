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
				if (value.X == 0 || value.Y == 0 || value.Z == 0)
					throw new ArgumentException();

				double x = new XYZ(this._matrix.m00, this._matrix.m01, this._matrix.m02).GetLength();
				double y = new XYZ(this._matrix.m10, this._matrix.m11, this._matrix.m12).GetLength();
				double z = new XYZ(this._matrix.m20, this._matrix.m21, this._matrix.m22).GetLength();

				this._matrix *= Matrix4.CreateScalingMatrix(1.0 / x * value.X, 1.0 / y * value.Y, 1.0 / z * value.Z);
			}
		}

		public XYZ Rotation
		{
			get { throw new NotImplementedException(); }
			set
			{
				_matrix *= Matrix4.CreateRotationMatrix(value.X, value.Y, value.Z);
			}
		}

		public Quaternion Quaternion
		{
			get { throw new NotImplementedException(); }
			set
			{
				//// Compute rotation matrix.
				//double x2 = value.X + value.X;
				//double y2 = value.Y + value.Y;
				//double z2 = value.Z + value.Z;

				//double wx2 = value.W * x2;
				//double wy2 = value.W * y2;
				//double wz2 = value.W * z2;
				//double xx2 = value.X * x2;
				//double xy2 = value.X * y2;
				//double xz2 = value.X * z2;
				//double yy2 = value.Y * y2;
				//double yz2 = value.Y * z2;
				//double zz2 = value.Z * z2;

				//double q11 = 1.0f - yy2 - zz2;
				//double q21 = xy2 - wz2;
				//double q31 = xz2 + wy2;

				//double q12 = xy2 + wz2;
				//double q22 = 1.0f - xx2 - zz2;
				//double q32 = yz2 - wx2;

				//double q13 = xz2 - wy2;
				//double q23 = yz2 + wx2;
				//double q33 = 1.0f - xx2 - yy2;

				//Matrix4 result = new Matrix4();

				//// First row
				//result.m00 = this._matrix.m00 * q11 + this._matrix.m10 * q21 + this._matrix.m20 * q31;
				//result.m10 = this._matrix.m00 * q12 + this._matrix.m10 * q22 + this._matrix.m20 * q32;
				//result.m20 = this._matrix.m00 * q13 + this._matrix.m10 * q23 + this._matrix.m20 * q33;
				//result.m30 = this._matrix.m30;

				//// Second row
				//result.m01 = this._matrix.m01 * q11 + this._matrix.m11 * q21 + this._matrix.m21 * q31;
				//result.m11 = this._matrix.m01 * q12 + this._matrix.m11 * q22 + this._matrix.m21 * q32;
				//result.m21 = this._matrix.m01 * q13 + this._matrix.m11 * q23 + this._matrix.m21 * q33;
				//result.m31 = this._matrix.m31;

				//// Third row
				//result.m02 = this._matrix.m02 * q11 + this._matrix.m12 * q21 + this._matrix.m22 * q31;
				//result.m12 = this._matrix.m02 * q12 + this._matrix.m12 * q22 + this._matrix.m22 * q32;
				//result.m22 = this._matrix.m02 * q13 + this._matrix.m12 * q23 + this._matrix.m22 * q33;
				//result.m32 = this._matrix.m32;

				//// Fourth row
				//result.m03 = this._matrix.m03 * q11 + this._matrix.m13 * q21 + this._matrix.m23 * q31;
				//result.m13 = this._matrix.m03 * q12 + this._matrix.m13 * q22 + this._matrix.m23 * q32;
				//result.m23 = this._matrix.m03 * q13 + this._matrix.m13 * q23 + this._matrix.m23 * q33;
				//result.m33 = this._matrix.m33;

				//this._matrix = result;
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

		public XYZ ApplyTransform(XYZ xyz)
		{
			return _matrix * xyz;
		}

		public static bool TryDecompose(Transform transform, out XYZ translation, out XYZ scaling, out Quaternion rotation)
		{
			Matrix4 matrix = transform._matrix;

			translation = new XYZ();
			scaling = new XYZ();
			rotation = new Quaternion();
			var XYZDouble = new XYZ();

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
				if (!Matrix4.Inverse(matrix, out Matrix4 inverse))
				{
					return false;
				}

				matrix.m03 = matrix.m13 = matrix.m23 = 0.0;
				matrix.m33 = 1.0;
			}

			translation.X = matrix.m30;
			matrix.m30 = 0.0;
			translation.Y = matrix.m31;
			matrix.m31 = 0.0;
			translation.Z = matrix.m32;
			matrix.m32 = 0.0;

			XYZ[] cols = new XYZ[3]
			{
			  new XYZ(matrix.m00, matrix.m01, matrix.m02),
			  new XYZ(matrix.m10, matrix.m11, matrix.m12),
			  new XYZ(matrix.m20, matrix.m21, matrix.m22)
			};

			//List<XYZM> cols = matrix.GetCols();

			scaling.X = cols[0].GetLength();
			cols[0] = cols[0].Normalize();
			XYZDouble.X = cols[0].Dot(cols[1]);
			cols[1] = cols[1] * 1 + cols[0] * -XYZDouble.X;

			scaling.Y = cols[1].GetLength();
			cols[1] = cols[1].Normalize();
			XYZDouble.Y = cols[0].Dot(cols[2]);
			cols[2] = cols[2] * 1 + cols[0] * -XYZDouble.Y;

			XYZDouble.Z = cols[1].Dot(cols[2]);
			cols[2] = cols[2] * 1 + cols[1] * -XYZDouble.Z;
			scaling.Z = cols[2].GetLength();
			cols[2] = cols[2].Normalize();

			XYZ rhs = XYZ.Cross(cols[1], cols[2]);
			if (cols[0].Dot(rhs) < 0.0)
			{
				for (int index = 0; index < 3; ++index)
				{
					scaling.X *= -1.0;
					cols[index].X *= -1.0;
					cols[index].Y *= -1.0;
					cols[index].Z *= -1.0;
				}
			}

			double trace = cols[0].X + cols[1].Y + cols[2].Z + 1.0;
			double qx;
			double qy;
			double qz;
			double qw;

			if (trace > 0)
			{
				double s = 0.5 / Math.Sqrt(trace);
				qx = (cols[2].Y - cols[1].Z) * s;
				qy = (cols[0].Z - cols[2].X) * s;
				qz = (cols[1].X - cols[0].Y) * s;
				qw = 0.25 / s;
			}
			else if (cols[0].X > cols[1].Y && cols[0].X > cols[2].Z)
			{
				double s = Math.Sqrt(1.0 + cols[0].X - cols[1].Y - cols[2].Z) * 2.0;
				qx = 0.25 * s;
				qy = (cols[0].Y + cols[1].X) / s;
				qz = (cols[0].Z + cols[2].X) / s;
				qw = (cols[2].Y - cols[1].Z) / s;
			}
			else if (cols[1].Y > cols[2].Z)
			{
				double s = Math.Sqrt(1.0 + cols[1].Y - cols[0].X - cols[2].Z) * 2.0;
				qx = (cols[0].Y + cols[1].X) / s;
				qy = 0.25 * s;
				qz = (cols[1].Z + cols[2].Y) / s;
				qw = (cols[0].Z - cols[2].X) / s;
			}
			else
			{
				double s = Math.Sqrt(1.0 + cols[2].Z - cols[0].X - cols[1].Y) * 2.0;
				qx = (cols[0].Z + cols[2].X) / s;
				qy = (cols[1].Z + cols[2].Y) / s;
				qz = 0.25 * s;
				qw = (cols[1].X - cols[0].Y) / s;
			}

			rotation.X = qx;
			rotation.Y = qy;
			rotation.Z = qz;
			rotation.W = -qw;

			return true;
		}
	}


}
