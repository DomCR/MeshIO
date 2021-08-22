//using System;
//using System.Globalization;

//namespace MeshIO
//{
//	/// <summary>
//	/// A structure encapsulating a 4x4 matrix.
//	/// </summary>
//	public struct Matrix4x4 : IEquatable<Matrix4x4>
//	{
//		#region Public Fields
//		/// <summary>
//		/// Value at row 1, column 1 of the matrix.
//		/// </summary>
//		public float m00;
//		/// <summary>
//		/// Value at row 1, column 2 of the matrix.
//		/// </summary>
//		public float m01;
//		/// <summary>
//		/// Value at row 1, column 3 of the matrix.
//		/// </summary>
//		public float m02;
//		/// <summary>
//		/// Value at row 1, column 4 of the matrix.
//		/// </summary>
//		public float m03;

//		/// <summary>
//		/// Value at row 2, column 1 of the matrix.
//		/// </summary>
//		public float m10;
//		/// <summary>
//		/// Value at row 2, column 2 of the matrix.
//		/// </summary>
//		public float m11;
//		/// <summary>
//		/// Value at row 2, column 3 of the matrix.
//		/// </summary>
//		public float m12;
//		/// <summary>
//		/// Value at row 2, column 4 of the matrix.
//		/// </summary>
//		public float m13;

//		/// <summary>
//		/// Value at row 3, column 1 of the matrix.
//		/// </summary>
//		public float m20;
//		/// <summary>
//		/// Value at row 3, column 2 of the matrix.
//		/// </summary>
//		public float m21;
//		/// <summary>
//		/// Value at row 3, column 3 of the matrix.
//		/// </summary>
//		public float m22;
//		/// <summary>
//		/// Value at row 3, column 4 of the matrix.
//		/// </summary>
//		public float m23;

//		/// <summary>
//		/// Value at row 4, column 1 of the matrix.
//		/// </summary>
//		public float m30;
//		/// <summary>
//		/// Value at row 4, column 2 of the matrix.
//		/// </summary>
//		public float m31;
//		/// <summary>
//		/// Value at row 4, column 3 of the matrix.
//		/// </summary>
//		public float m32;
//		/// <summary>
//		/// Value at row 4, column 4 of the matrix.
//		/// </summary>
//		public float m33;
//		#endregion Public Fields

//		/// <summary>
//		/// Returns the multiplicative identity matrix.
//		/// </summary>
//		public static Matrix4x4 Identity { get; } = new Matrix4x4
//		(
//			1f, 0f, 0f, 0f,
//			0f, 1f, 0f, 0f,
//			0f, 0f, 1f, 0f,
//			0f, 0f, 0f, 1f
//		);

//		/// <summary>
//		/// Gets or sets the translation component of this matrix.
//		/// </summary>
//		public XYZ Translation
//		{
//			get
//			{
//				return new XYZ(m30, m31, m32);
//			}
//			set
//			{
//				m30 = (float)value.X;
//				m31 = (float)value.Y;
//				m32 = (float)value.Z;
//			}
//		}

//		/// <summary>
//		/// Constructs a Matrix4x4 from the given components.
//		/// </summary>
//		public Matrix4x4(float m11, float m12, float m13, float m14,
//						 float m21, float m22, float m23, float m24,
//						 float m31, float m32, float m33, float m34,
//						 float m41, float m42, float m43, float m44)
//		{
//			this.m00 = m11;
//			this.m01 = m12;
//			this.m02 = m13;
//			this.m03 = m14;

//			this.m10 = m21;
//			this.m11 = m22;
//			this.m12 = m23;
//			this.m13 = m24;

//			this.m20 = m31;
//			this.m21 = m32;
//			this.m22 = m33;
//			this.m23 = m34;

//			this.m30 = m41;
//			this.m31 = m42;
//			this.m32 = m43;
//			this.m33 = m44;
//		}

//		///// <summary>
//		///// Constructs a Matrix4x4 from the given Matrix3x2.
//		///// </summary>
//		///// <param name="value">The source Matrix3x2.</param>
//		//public Matrix4x4(Matrix3x2 value)
//		//{
//		//	m00 = value.M11;
//		//	m01 = value.M12;
//		//	m02 = 0f;
//		//	m03 = 0f;
//		//	m10 = value.M21;
//		//	m11 = value.M22;
//		//	m12 = 0f;
//		//	m13 = 0f;
//		//	m20 = 0f;
//		//	m21 = 0f;
//		//	m22 = 1f;
//		//	m23 = 0f;
//		//	m30 = value.M31;
//		//	m31 = value.M32;
//		//	m32 = 0f;
//		//	m33 = 1f;
//		//}

//		///// <summary>
//		///// Creates a spherical billboard that rotates around a specified object position.
//		///// </summary>
//		///// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
//		///// <param name="cameraPosition">Position of the camera.</param>
//		///// <param name="cameraUpVector">The up vector of the camera.</param>
//		///// <param name="cameraForwardVector">The forward vector of the camera.</param>
//		///// <returns>The created billboard matrix</returns>
//		//public static Matrix4x4 CreateBillboard(XYZ objectPosition, XYZ cameraPosition, XYZ cameraUpVector, XYZ cameraForwardVector)
//		//{
//		//	const float epsilon = 1e-4f;

//		//	XYZ zaxis = new XYZ(
//		//		objectPosition.X - cameraPosition.X,
//		//		objectPosition.Y - cameraPosition.Y,
//		//		objectPosition.Z - cameraPosition.Z);

//		//	float norm = zaxis.LengthSquared();

//		//	if (norm < epsilon)
//		//	{
//		//		zaxis = -cameraForwardVector;
//		//	}
//		//	else
//		//	{
//		//		zaxis = XYZ.Multiply(zaxis, 1.0f / (float)Math.Sqrt(norm));
//		//	}

//		//	XYZ xaxis = XYZ.Normalize(XYZ.Cross(cameraUpVector, zaxis));

//		//	XYZ yaxis = XYZ.Cross(zaxis, xaxis);

//		//	Matrix4x4 result;

//		//	result.m00 = (float)xaxis.X;
//		//	result.m01 = (float)xaxis.Y;
//		//	result.m02 = (float)xaxis.Z;
//		//	result.m03 = 0.0f;
//		//	result.m10 = (float)yaxis.X;
//		//	result.m11 = (float)yaxis.Y;
//		//	result.m12 = (float)yaxis.Z;
//		//	result.m13 = 0.0f;
//		//	result.m20 = (float)zaxis.X;
//		//	result.m21 = (float)zaxis.Y;
//		//	result.m22 = (float)zaxis.Z;
//		//	result.m23 = 0.0f;

//		//	result.m30 = (float)objectPosition.X;
//		//	result.m31 = (float)objectPosition.Y;
//		//	result.m32 = (float)objectPosition.Z;
//		//	result.m33 = 1.0f;

//		//	return result;
//		//}

//		///// <summary>
//		///// Creates a cylindrical billboard that rotates around a specified axis.
//		///// </summary>
//		///// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
//		///// <param name="cameraPosition">Position of the camera.</param>
//		///// <param name="rotateAxis">Axis to rotate the billboard around.</param>
//		///// <param name="cameraForwardVector">Forward vector of the camera.</param>
//		///// <param name="objectForwardVector">Forward vector of the object.</param>
//		///// <returns>The created billboard matrix.</returns>
//		//public static Matrix4x4 CreateConstrainedBillboard(XYZ objectPosition, XYZ cameraPosition, XYZ rotateAxis, XYZ cameraForwardVector, XYZ objectForwardVector)
//		//{
//		//	const float epsilon = 1e-4f;
//		//	const float minAngle = 1.0f - (0.1f * ((float)Math.PI / 180.0f)); // 0.1 degrees

//		//	// Treat the case when object and camera positions are too close.
//		//	XYZ faceDir = new XYZ(
//		//		objectPosition.X - cameraPosition.X,
//		//		objectPosition.Y - cameraPosition.Y,
//		//		objectPosition.Z - cameraPosition.Z);

//		//	float norm = faceDir.LengthSquared();

//		//	if (norm < epsilon)
//		//	{
//		//		faceDir = -cameraForwardVector;
//		//	}
//		//	else
//		//	{
//		//		faceDir = XYZ.Multiply(faceDir, (1.0f / (float)Math.Sqrt(norm)));
//		//	}

//		//	XYZ yaxis = rotateAxis;
//		//	XYZ xaxis;
//		//	XYZ zaxis;

//		//	// Treat the case when angle between faceDir and rotateAxis is too close to 0.
//		//	float dot = XYZ.Dot(rotateAxis, faceDir);

//		//	if (Math.Abs(dot) > minAngle)
//		//	{
//		//		zaxis = objectForwardVector;

//		//		// Make sure passed values are useful for compute.
//		//		dot = XYZ.Dot(rotateAxis, zaxis);

//		//		if (Math.Abs(dot) > minAngle)
//		//		{
//		//			zaxis = (Math.Abs(rotateAxis.Z) > minAngle) ? new XYZ(1, 0, 0) : new XYZ(0, 0, -1);
//		//		}

//		//		xaxis = XYZ.Normalize(XYZ.Cross(rotateAxis, zaxis));
//		//		zaxis = XYZ.Normalize(XYZ.Cross(xaxis, rotateAxis));
//		//	}
//		//	else
//		//	{
//		//		xaxis = XYZ.Normalize(XYZ.Cross(rotateAxis, faceDir));
//		//		zaxis = XYZ.Normalize(XYZ.Cross(xaxis, yaxis));
//		//	}

//		//	Matrix4x4 result;

//		//	result.m00 = (float)xaxis.X;
//		//	result.m01 = (float)xaxis.Y;
//		//	result.m02 = (float)xaxis.Z;
//		//	result.m03 = 0.0f;
//		//	result.m10 = (float)yaxis.X;
//		//	result.m11 = (float)yaxis.Y;
//		//	result.m12 = (float)yaxis.Z;
//		//	result.m13 = 0.0f;
//		//	result.m20 = (float)zaxis.X;
//		//	result.m21 = (float)zaxis.Y;
//		//	result.m22 = (float)zaxis.Z;
//		//	result.m23 = 0.0f;

//		//	result.m30 = (float)objectPosition.X;
//		//	result.m31 = (float)objectPosition.Y;
//		//	result.m32 = (float)objectPosition.Z;
//		//	result.m33 = 1.0f;

//		//	return result;
//		//}

//		/// <summary>
//		/// Creates a translation matrix.
//		/// </summary>
//		/// <param name="position">The amount to translate in each axis.</param>
//		/// <returns>The translation matrix.</returns>
//		public static Matrix4x4 CreateTranslation(XYZ position)
//		{
//			Matrix4x4 result;

//			result.m00 = 1.0f;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;

//			result.m10 = 0.0f;
//			result.m11 = 1.0f;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;

//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = 1.0f;
//			result.m23 = 0.0f;

//			result.m30 = (float)position.X;
//			result.m31 = (float)position.Y;
//			result.m32 = (float)position.Z;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a translation matrix.
//		/// </summary>
//		/// <param name="xPosition">The amount to translate on the X-axis.</param>
//		/// <param name="yPosition">The amount to translate on the Y-axis.</param>
//		/// <param name="zPosition">The amount to translate on the Z-axis.</param>
//		/// <returns>The translation matrix.</returns>
//		public static Matrix4x4 CreateTranslation(float xPosition, float yPosition, float zPosition)
//		{
//			Matrix4x4 result;

//			result.m00 = 1.0f;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = 1.0f;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = 1.0f;
//			result.m23 = 0.0f;

//			result.m30 = xPosition;
//			result.m31 = yPosition;
//			result.m32 = zPosition;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a scaling matrix.
//		/// </summary>
//		/// <param name="xScale">Value to scale by on the X-axis.</param>
//		/// <param name="yScale">Value to scale by on the Y-axis.</param>
//		/// <param name="zScale">Value to scale by on the Z-axis.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale)
//		{
//			Matrix4x4 result;

//			result.m00 = xScale;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = yScale;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = zScale;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a scaling matrix.
//		/// </summary>
//		/// <param name="scales">The vector containing the amount to scale by on each axis.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(XYZ scales)
//		{
//			Matrix4x4 result;

//			result.m00 = (float)scales.X;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = (float)scales.Y;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = (float)scales.Z;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a scaling matrix with a center point.
//		/// </summary>
//		/// <param name="xScale">Value to scale by on the X-axis.</param>
//		/// <param name="yScale">Value to scale by on the Y-axis.</param>
//		/// <param name="zScale">Value to scale by on the Z-axis.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			double tx = centerPoint.X * (1 - xScale);
//			double ty = centerPoint.Y * (1 - yScale);
//			double tz = centerPoint.Z * (1 - zScale);

//			result.m00 = xScale;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = yScale;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = zScale;
//			result.m23 = 0.0f;
//			result.m30 = (float)tx;
//			result.m31 = (float)ty;
//			result.m32 = (float)tz;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a scaling matrix with a center point.
//		/// </summary>
//		/// <param name="scales">The vector containing the amount to scale by on each axis.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(XYZ scales, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			float tx = (float)(centerPoint.X * (1 - scales.X));
//			float ty = (float)(centerPoint.Y * (1 - scales.Y));
//			float tz = (float)(centerPoint.Z * (1 - scales.Z));

//			result.m00 = (float)scales.X;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = (float)scales.Y;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = (float)scales.Z;
//			result.m23 = 0.0f;
//			result.m30 = tx;
//			result.m31 = ty;
//			result.m32 = tz;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a uniform scaling matrix that scales equally on each axis.
//		/// </summary>
//		/// <param name="scale">The uniform scaling factor.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(float scale)
//		{
//			Matrix4x4 result;

//			result.m00 = scale;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = scale;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = scale;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a uniform scaling matrix that scales equally on each axis with a center point.
//		/// </summary>
//		/// <param name="scale">The uniform scaling factor.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The scaling matrix.</returns>
//		public static Matrix4x4 CreateScale(float scale, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			float tx = (float)(centerPoint.X * (1 - scale));
//			float ty = (float)(centerPoint.Y * (1 - scale));
//			float tz = (float)(centerPoint.Z * (1 - scale));

//			result.m00 = scale;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = scale;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = scale;
//			result.m23 = 0.0f;
//			result.m30 = tx;
//			result.m31 = ty;
//			result.m32 = tz;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the X-axis.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the X-axis.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationX(float radians)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			// [  1  0  0  0 ]
//			// [  0  c  s  0 ]
//			// [  0 -s  c  0 ]
//			// [  0  0  0  1 ]
//			result.m00 = 1.0f;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = c;
//			result.m12 = s;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = -s;
//			result.m22 = c;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the X-axis, from a center point.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the X-axis.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationX(float radians, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			float y = (float)(centerPoint.Y * (1 - c) + centerPoint.Z * s);
//			float z = (float)(centerPoint.Z * (1 - c) - centerPoint.Y * s);

//			// [  1  0  0  0 ]
//			// [  0  c  s  0 ]
//			// [  0 -s  c  0 ]
//			// [  0  y  z  1 ]
//			result.m00 = 1.0f;
//			result.m01 = 0.0f;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = c;
//			result.m12 = s;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = -s;
//			result.m22 = c;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = y;
//			result.m32 = z;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the Y-axis.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationY(float radians)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			// [  c  0 -s  0 ]
//			// [  0  1  0  0 ]
//			// [  s  0  c  0 ]
//			// [  0  0  0  1 ]
//			result.m00 = c;
//			result.m01 = 0.0f;
//			result.m02 = -s;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = 1.0f;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = s;
//			result.m21 = 0.0f;
//			result.m22 = c;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the Y-axis, from a center point.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationY(float radians, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			float x = (float)(centerPoint.X * (1 - c) - centerPoint.Z * s);
//			float z = (float)(centerPoint.Z * (1 - c) + centerPoint.X * s);

//			// [  c  0 -s  0 ]
//			// [  0  1  0  0 ]
//			// [  s  0  c  0 ]
//			// [  x  0  z  1 ]
//			result.m00 = c;
//			result.m01 = 0.0f;
//			result.m02 = -s;
//			result.m03 = 0.0f;
//			result.m10 = 0.0f;
//			result.m11 = 1.0f;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = s;
//			result.m21 = 0.0f;
//			result.m22 = c;
//			result.m23 = 0.0f;
//			result.m30 = x;
//			result.m31 = 0.0f;
//			result.m32 = z;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the Z-axis.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationZ(float radians)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			// [  c  s  0  0 ]
//			// [ -s  c  0  0 ]
//			// [  0  0  1  0 ]
//			// [  0  0  0  1 ]
//			result.m00 = c;
//			result.m01 = s;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = -s;
//			result.m11 = c;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = 1.0f;
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix for rotating points around the Z-axis, from a center point.
//		/// </summary>
//		/// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
//		/// <param name="centerPoint">The center point.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateRotationZ(float radians, XYZ centerPoint)
//		{
//			Matrix4x4 result;

//			float c = (float)Math.Cos(radians);
//			float s = (float)Math.Sin(radians);

//			float x = (float)(centerPoint.X * (1 - c) + centerPoint.Y * s);
//			float y = (float)(centerPoint.Y * (1 - c) - centerPoint.X * s);

//			// [  c  s  0  0 ]
//			// [ -s  c  0  0 ]
//			// [  0  0  1  0 ]
//			// [  x  y  0  1 ]
//			result.m00 = c;
//			result.m01 = s;
//			result.m02 = 0.0f;
//			result.m03 = 0.0f;
//			result.m10 = -s;
//			result.m11 = c;
//			result.m12 = 0.0f;
//			result.m13 = 0.0f;
//			result.m20 = 0.0f;
//			result.m21 = 0.0f;
//			result.m22 = 1.0f;
//			result.m23 = 0.0f;
//			result.m30 = x;
//			result.m31 = y;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a matrix that rotates around an arbitrary vector.
//		/// </summary>
//		/// <param name="axis">The axis to rotate around.</param>
//		/// <param name="angle">The angle to rotate around the given axis, in radians.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateFromAxisAngle(XYZ axis, float angle)
//		{
//			// a: angle
//			// x, y, z: unit vector for axis.
//			//
//			// Rotation matrix M can compute by using below equation.
//			//
//			//        T               T
//			//  M = uu + (cos a)( I-uu ) + (sin a)S
//			//
//			// Where:
//			//
//			//  u = ( x, y, z )
//			//
//			//      [  0 -z  y ]
//			//  S = [  z  0 -x ]
//			//      [ -y  x  0 ]
//			//
//			//      [ 1 0 0 ]
//			//  I = [ 0 1 0 ]
//			//      [ 0 0 1 ]
//			//
//			//
//			//     [  xx+cosa*(1-xx)   yx-cosa*yx-sina*z zx-cosa*xz+sina*y ]
//			// M = [ xy-cosa*yx+sina*z    yy+cosa(1-yy)  yz-cosa*yz-sina*x ]
//			//     [ zx-cosa*zx-sina*y zy-cosa*zy+sina*x   zz+cosa*(1-zz)  ]
//			//
//			float x = (float)axis.X, y = (float)axis.Y, z = (float)axis.Z;
//			float sa = (float)Math.Sin(angle), ca = (float)Math.Cos(angle);
//			float xx = x * x, yy = y * y, zz = z * z;
//			float xy = x * y, xz = x * z, yz = y * z;

//			Matrix4x4 result;

//			result.m00 = xx + ca * (1.0f - xx);
//			result.m01 = xy - ca * xy + sa * z;
//			result.m02 = xz - ca * xz - sa * y;
//			result.m03 = 0.0f;
//			result.m10 = xy - ca * xy - sa * z;
//			result.m11 = yy + ca * (1.0f - yy);
//			result.m12 = yz - ca * yz + sa * x;
//			result.m13 = 0.0f;
//			result.m20 = xz - ca * xz + sa * y;
//			result.m21 = yz - ca * yz - sa * x;
//			result.m22 = zz + ca * (1.0f - zz);
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane distances. 
//		/// </summary>
//		/// <param name="fieldOfView">Field of view in the y direction, in radians.</param>
//		/// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
//		/// <param name="nearPlaneDistance">Distance to the near view plane.</param>
//		/// <param name="farPlaneDistance">Distance to the far view plane.</param>
//		/// <returns>The perspective projection matrix.</returns>
//		public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
//		{
//			if (fieldOfView <= 0.0f || fieldOfView >= Math.PI)
//				throw new ArgumentOutOfRangeException("fieldOfView");

//			if (nearPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			if (farPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("farPlaneDistance");

//			if (nearPlaneDistance >= farPlaneDistance)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			float yScale = 1.0f / (float)Math.Tan(fieldOfView * 0.5f);
//			float xScale = yScale / aspectRatio;

//			Matrix4x4 result;

//			result.m00 = xScale;
//			result.m01 = result.m02 = result.m03 = 0.0f;

//			result.m11 = yScale;
//			result.m10 = result.m12 = result.m13 = 0.0f;

//			result.m20 = result.m21 = 0.0f;
//			result.m22 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
//			result.m23 = -1.0f;

//			result.m30 = result.m31 = result.m33 = 0.0f;
//			result.m32 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

//			return result;
//		}

//		/// <summary>
//		/// Creates a perspective projection matrix from the given view volume dimensions.
//		/// </summary>
//		/// <param name="width">Width of the view volume at the near view plane.</param>
//		/// <param name="height">Height of the view volume at the near view plane.</param>
//		/// <param name="nearPlaneDistance">Distance to the near view plane.</param>
//		/// <param name="farPlaneDistance">Distance to the far view plane.</param>
//		/// <returns>The perspective projection matrix.</returns>
//		public static Matrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
//		{
//			if (nearPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			if (farPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("farPlaneDistance");

//			if (nearPlaneDistance >= farPlaneDistance)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			Matrix4x4 result;

//			result.m00 = 2.0f * nearPlaneDistance / width;
//			result.m01 = result.m02 = result.m03 = 0.0f;

//			result.m11 = 2.0f * nearPlaneDistance / height;
//			result.m10 = result.m12 = result.m13 = 0.0f;

//			result.m22 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
//			result.m20 = result.m21 = 0.0f;
//			result.m23 = -1.0f;

//			result.m30 = result.m31 = result.m33 = 0.0f;
//			result.m32 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

//			return result;
//		}

//		/// <summary>
//		/// Creates a customized, perspective projection matrix.
//		/// </summary>
//		/// <param name="left">Minimum x-value of the view volume at the near view plane.</param>
//		/// <param name="right">Maximum x-value of the view volume at the near view plane.</param>
//		/// <param name="bottom">Minimum y-value of the view volume at the near view plane.</param>
//		/// <param name="top">Maximum y-value of the view volume at the near view plane.</param>
//		/// <param name="nearPlaneDistance">Distance to the near view plane.</param>
//		/// <param name="farPlaneDistance">Distance to of the far view plane.</param>
//		/// <returns>The perspective projection matrix.</returns>
//		public static Matrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
//		{
//			if (nearPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			if (farPlaneDistance <= 0.0f)
//				throw new ArgumentOutOfRangeException("farPlaneDistance");

//			if (nearPlaneDistance >= farPlaneDistance)
//				throw new ArgumentOutOfRangeException("nearPlaneDistance");

//			Matrix4x4 result;

//			result.m00 = 2.0f * nearPlaneDistance / (right - left);
//			result.m01 = result.m02 = result.m03 = 0.0f;

//			result.m11 = 2.0f * nearPlaneDistance / (top - bottom);
//			result.m10 = result.m12 = result.m13 = 0.0f;

//			result.m20 = (left + right) / (right - left);
//			result.m21 = (top + bottom) / (top - bottom);
//			result.m22 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
//			result.m23 = -1.0f;

//			result.m32 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
//			result.m30 = result.m31 = result.m33 = 0.0f;

//			return result;
//		}

//		/// <summary>
//		/// Creates an orthographic perspective matrix from the given view volume dimensions.
//		/// </summary>
//		/// <param name="width">Width of the view volume.</param>
//		/// <param name="height">Height of the view volume.</param>
//		/// <param name="zNearPlane">Minimum Z-value of the view volume.</param>
//		/// <param name="zFarPlane">Maximum Z-value of the view volume.</param>
//		/// <returns>The orthographic projection matrix.</returns>
//		public static Matrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
//		{
//			Matrix4x4 result;

//			result.m00 = 2.0f / width;
//			result.m01 = result.m02 = result.m03 = 0.0f;

//			result.m11 = 2.0f / height;
//			result.m10 = result.m12 = result.m13 = 0.0f;

//			result.m22 = 1.0f / (zNearPlane - zFarPlane);
//			result.m20 = result.m21 = result.m23 = 0.0f;

//			result.m30 = result.m31 = 0.0f;
//			result.m32 = zNearPlane / (zNearPlane - zFarPlane);
//			result.m33 = 1.0f;

//			return result;
//		}

//		/// <summary>
//		/// Builds a customized, orthographic projection matrix.
//		/// </summary>
//		/// <param name="left">Minimum X-value of the view volume.</param>
//		/// <param name="right">Maximum X-value of the view volume.</param>
//		/// <param name="bottom">Minimum Y-value of the view volume.</param>
//		/// <param name="top">Maximum Y-value of the view volume.</param>
//		/// <param name="zNearPlane">Minimum Z-value of the view volume.</param>
//		/// <param name="zFarPlane">Maximum Z-value of the view volume.</param>
//		/// <returns>The orthographic projection matrix.</returns>
//		public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
//		{
//			Matrix4x4 result;

//			result.m00 = 2.0f / (right - left);
//			result.m01 = result.m02 = result.m03 = 0.0f;

//			result.m11 = 2.0f / (top - bottom);
//			result.m10 = result.m12 = result.m13 = 0.0f;

//			result.m22 = 1.0f / (zNearPlane - zFarPlane);
//			result.m20 = result.m21 = result.m23 = 0.0f;

//			result.m30 = (left + right) / (left - right);
//			result.m31 = (top + bottom) / (bottom - top);
//			result.m32 = zNearPlane / (zNearPlane - zFarPlane);
//			result.m33 = 1.0f;

//			return result;
//		}

//		///// <summary>
//		///// Creates a view matrix.
//		///// </summary>
//		///// <param name="cameraPosition">The position of the camera.</param>
//		///// <param name="cameraTarget">The target towards which the camera is pointing.</param>
//		///// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
//		///// <returns>The view matrix.</returns>
//		//public static Matrix4x4 CreateLookAt(XYZ cameraPosition, XYZ cameraTarget, XYZ cameraUpVector)
//		//{
//		//	XYZ zaxis = XYZ.Normalize(cameraPosition - cameraTarget);
//		//	XYZ xaxis = XYZ.Normalize(XYZ.Cross(cameraUpVector, zaxis));
//		//	XYZ yaxis = XYZ.Cross(zaxis, xaxis);

//		//	Matrix4x4 result;

//		//	result.m00 = (float)xaxis.X;
//		//	result.m01 = (float)yaxis.X;
//		//	result.m02 = (float)zaxis.X;
//		//	result.m03 = 0.0f;
//		//	result.m10 = (float)xaxis.Y;
//		//	result.m11 = (float)yaxis.Y;
//		//	result.m12 = (float)zaxis.Y;
//		//	result.m13 = 0.0f;
//		//	result.m20 = (float)xaxis.Z;
//		//	result.m21 = (float)yaxis.Z;
//		//	result.m22 = (float)zaxis.Z;
//		//	result.m23 = 0.0f;
//		//	result.m30 = -XYZ.Dot(xaxis, cameraPosition);
//		//	result.m31 = -XYZ.Dot(yaxis, cameraPosition);
//		//	result.m32 = -XYZ.Dot(zaxis, cameraPosition);
//		//	result.m33 = 1.0f;

//		//	return result;
//		//}

//		///// <summary>
//		///// Creates a world matrix with the specified parameters.
//		///// </summary>
//		///// <param name="position">The position of the object; used in translation operations.</param>
//		///// <param name="forward">Forward direction of the object.</param>
//		///// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
//		///// <returns>The world matrix.</returns>
//		//public static Matrix4x4 CreateWorld(XYZ position, XYZ forward, XYZ up)
//		//{
//		//	XYZ zaxis = XYZ.Normalize(-forward);
//		//	XYZ xaxis = XYZ.Normalize(XYZ.Cross(up, zaxis));
//		//	XYZ yaxis = XYZ.Cross(zaxis, xaxis);

//		//	Matrix4x4 result;

//		//	result.m00 = (float)xaxis.X;
//		//	result.m01 = (float)xaxis.Y;
//		//	result.m02 = (float)xaxis.Z;
//		//	result.m03 = 0.0f;
//		//	result.m10 = (float)yaxis.X;
//		//	result.m11 = (float)yaxis.Y;
//		//	result.m12 = (float)yaxis.Z;
//		//	result.m13 = 0.0f;
//		//	result.m20 = (float)zaxis.X;
//		//	result.m21 = (float)zaxis.Y;
//		//	result.m22 = (float)zaxis.Z;
//		//	result.m23 = 0.0f;
//		//	result.m30 = (float)position.X;
//		//	result.m31 = (float)position.Y;
//		//	result.m32 = (float)position.Z;
//		//	result.m33 = 1.0f;

//		//	return result;
//		//}

//		/// <summary>
//		/// Creates a rotation matrix from the given Quaternion rotation value.
//		/// </summary>
//		/// <param name="quaternion">The source Quaternion.</param>
//		/// <returns>The rotation matrix.</returns>
//		public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion)
//		{
//			Matrix4x4 result;

//			float xx = (float)(quaternion.X * quaternion.X);
//			float yy = (float)(quaternion.Y * quaternion.Y);
//			float zz = (float)(quaternion.Z * quaternion.Z);

//			float xy = (float)(quaternion.X * quaternion.Y);
//			float wz = (float)(quaternion.Z * quaternion.W);
//			float xz = (float)(quaternion.Z * quaternion.X);
//			float wy = (float)(quaternion.Y * quaternion.W);
//			float yz = (float)(quaternion.Y * quaternion.Z);
//			float wx = (float)(quaternion.X * quaternion.W);

//			result.m00 = 1.0f - 2.0f * (yy + zz);
//			result.m01 = 2.0f * (xy + wz);
//			result.m02 = 2.0f * (xz - wy);
//			result.m03 = 0.0f;
//			result.m10 = 2.0f * (xy - wz);
//			result.m11 = 1.0f - 2.0f * (zz + xx);
//			result.m12 = 2.0f * (yz + wx);
//			result.m13 = 0.0f;
//			result.m20 = 2.0f * (xz + wy);
//			result.m21 = 2.0f * (yz - wx);
//			result.m22 = 1.0f - 2.0f * (yy + xx);
//			result.m23 = 0.0f;
//			result.m30 = 0.0f;
//			result.m31 = 0.0f;
//			result.m32 = 0.0f;
//			result.m33 = 1.0f;

//			return result;
//		}

//		///// <summary>
//		///// Creates a rotation matrix from the specified yaw, pitch, and roll.
//		///// </summary>
//		///// <param name="yaw">Angle of rotation, in radians, around the Y-axis.</param>
//		///// <param name="pitch">Angle of rotation, in radians, around the X-axis.</param>
//		///// <param name="roll">Angle of rotation, in radians, around the Z-axis.</param>
//		///// <returns>The rotation matrix.</returns>
//		//public static Matrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
//		//{
//		//	Quaternion q = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);

//		//	return Matrix4x4.CreateFromQuaternion(q);
//		//}

//		///// <summary>
//		///// Creates a Matrix that flattens geometry into a specified Plane as if casting a shadow from a specified light source.
//		///// </summary>
//		///// <param name="lightDirection">The direction from which the light that will cast the shadow is coming.</param>
//		///// <param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
//		///// <returns>A new Matrix that can be used to flatten geometry onto the specified plane from the specified direction.</returns>
//		//public static Matrix4x4 CreateShadow(XYZ lightDirection, Plane plane)
//		//{
//		//	Plane p = Plane.Normalize(plane);

//		//	float dot = p.Normal.X * lightDirection.X + p.Normal.Y * lightDirection.Y + p.Normal.Z * lightDirection.Z;
//		//	float a = -p.Normal.X;
//		//	float b = -p.Normal.Y;
//		//	float c = -p.Normal.Z;
//		//	float d = -p.D;

//		//	Matrix4x4 result;

//		//	result.m00 = (float)(a * lightDirection.X + dot);
//		//	result.m10 = (float)(b * lightDirection.X);
//		//	result.m20 = (float)(c * lightDirection.X);
//		//	result.m30 = (float)(d * lightDirection.X);

//		//	result.m01 = (float)(a * lightDirection.Y);
//		//	result.m11 = (float)(b * lightDirection.Y + dot);
//		//	result.m21 = (float)(c * lightDirection.Y);
//		//	result.m31 = (float)(d * lightDirection.Y);

//		//	result.m02 = (float)(a * lightDirection.Z);
//		//	result.m12 = (float)(b * lightDirection.Z);
//		//	result.m22 = (float)(c * lightDirection.Z + dot);
//		//	result.m32 = (float)(d * lightDirection.Z);

//		//	result.m03 = 0.0f;
//		//	result.m13 = 0.0f;
//		//	result.m23 = 0.0f;
//		//	result.m33 = dot;

//		//	return result;
//		//}

//		///// <summary>
//		///// Creates a Matrix that reflects the coordinate system about a specified Plane.
//		///// </summary>
//		///// <param name="value">The Plane about which to create a reflection.</param>
//		///// <returns>A new matrix expressing the reflection.</returns>
//		//public static Matrix4x4 CreateReflection(Plane value)
//		//{
//		//	value = Plane.Normalize(value);

//		//	float a = value.Normal.X;
//		//	float b = value.Normal.Y;
//		//	float c = value.Normal.Z;

//		//	float fa = -2.0f * a;
//		//	float fb = -2.0f * b;
//		//	float fc = -2.0f * c;

//		//	Matrix4x4 result;

//		//	result.m00 = fa * a + 1.0f;
//		//	result.m01 = fb * a;
//		//	result.m02 = fc * a;
//		//	result.m03 = 0.0f;

//		//	result.m10 = fa * b;
//		//	result.m11 = fb * b + 1.0f;
//		//	result.m12 = fc * b;
//		//	result.m13 = 0.0f;

//		//	result.m20 = fa * c;
//		//	result.m21 = fb * c;
//		//	result.m22 = fc * c + 1.0f;
//		//	result.m23 = 0.0f;

//		//	result.m30 = fa * value.D;
//		//	result.m31 = fb * value.D;
//		//	result.m32 = fc * value.D;
//		//	result.m33 = 1.0f;

//		//	return result;
//		//}

//		/// <summary>
//		/// Calculates the determinant of the matrix.
//		/// </summary>
//		/// <returns>The determinant of the matrix.</returns>
//		public float GetDeterminant()
//		{
//			// | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
//			// | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
//			// | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
//			// | m n o p |
//			//
//			//   | f g h |
//			// a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
//			//   | n o p |
//			//
//			//   | e g h |     
//			// b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
//			//   | m o p |     
//			//
//			//   | e f h |
//			// c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
//			//   | m n p |
//			//
//			//   | e f g |
//			// d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
//			//   | m n o |
//			//
//			// Cost of operation
//			// 17 adds and 28 muls.
//			//
//			// add: 6 + 8 + 3 = 17
//			// mul: 12 + 16 = 28

//			float a = m00, b = m01, c = m02, d = m03;
//			float e = m10, f = m11, g = m12, h = m13;
//			float i = m20, j = m21, k = m22, l = m23;
//			float m = m30, n = m31, o = m32, p = m33;

//			float kp_lo = k * p - l * o;
//			float jp_ln = j * p - l * n;
//			float jo_kn = j * o - k * n;
//			float ip_lm = i * p - l * m;
//			float io_km = i * o - k * m;
//			float in_jm = i * n - j * m;

//			return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
//				   b * (e * kp_lo - g * ip_lm + h * io_km) +
//				   c * (e * jp_ln - f * ip_lm + h * in_jm) -
//				   d * (e * jo_kn - f * io_km + g * in_jm);
//		}

//		/// <summary>
//		/// Attempts to calculate the inverse of the given matrix. If successful, result will contain the inverted matrix.
//		/// </summary>
//		/// <param name="matrix">The source matrix to invert.</param>
//		/// <param name="result">If successful, contains the inverted matrix.</param>
//		/// <returns>True if the source matrix could be inverted; False otherwise.</returns>
//		public static bool Invert(Matrix4x4 matrix, out Matrix4x4 result)
//		{
//			//                                       -1
//			// If you have matrix M, inverse Matrix M   can compute
//			//
//			//     -1       1      
//			//    M   = --------- A
//			//            det(M)
//			//
//			// A is adjugate (adjoint) of M, where,
//			//
//			//      T
//			// A = C
//			//
//			// C is Cofactor matrix of M, where,
//			//           i + j
//			// C   = (-1)      * det(M  )
//			//  ij                    ij
//			//
//			//     [ a b c d ]
//			// M = [ e f g h ]
//			//     [ i j k l ]
//			//     [ m n o p ]
//			//
//			// First Row
//			//           2 | f g h |
//			// C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
//			//  11         | n o p |
//			//
//			//           3 | e g h |
//			// C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
//			//  12         | m o p |
//			//
//			//           4 | e f h |
//			// C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
//			//  13         | m n p |
//			//
//			//           5 | e f g |
//			// C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
//			//  14         | m n o |
//			//
//			// Second Row
//			//           3 | b c d |
//			// C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
//			//  21         | n o p |
//			//
//			//           4 | a c d |
//			// C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
//			//  22         | m o p |
//			//
//			//           5 | a b d |
//			// C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
//			//  23         | m n p |
//			//
//			//           6 | a b c |
//			// C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
//			//  24         | m n o |
//			//
//			// Third Row
//			//           4 | b c d |
//			// C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
//			//  31         | n o p |
//			//
//			//           5 | a c d |
//			// C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
//			//  32         | m o p |
//			//
//			//           6 | a b d |
//			// C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
//			//  33         | m n p |
//			//
//			//           7 | a b c |
//			// C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
//			//  34         | m n o |
//			//
//			// Fourth Row
//			//           5 | b c d |
//			// C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
//			//  41         | j k l |
//			//
//			//           6 | a c d |
//			// C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
//			//  42         | i k l |
//			//
//			//           7 | a b d |
//			// C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
//			//  43         | i j l |
//			//
//			//           8 | a b c |
//			// C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
//			//  44         | i j k |
//			//
//			// Cost of operation
//			// 53 adds, 104 muls, and 1 div.
//			float a = matrix.m00, b = matrix.m01, c = matrix.m02, d = matrix.m03;
//			float e = matrix.m10, f = matrix.m11, g = matrix.m12, h = matrix.m13;
//			float i = matrix.m20, j = matrix.m21, k = matrix.m22, l = matrix.m23;
//			float m = matrix.m30, n = matrix.m31, o = matrix.m32, p = matrix.m33;

//			float kp_lo = k * p - l * o;
//			float jp_ln = j * p - l * n;
//			float jo_kn = j * o - k * n;
//			float ip_lm = i * p - l * m;
//			float io_km = i * o - k * m;
//			float in_jm = i * n - j * m;

//			float a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
//			float a12 = -(e * kp_lo - g * ip_lm + h * io_km);
//			float a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
//			float a14 = -(e * jo_kn - f * io_km + g * in_jm);

//			float det = a * a11 + b * a12 + c * a13 + d * a14;

//			if (Math.Abs(det) < float.Epsilon)
//			{
//				result = new Matrix4x4(float.NaN, float.NaN, float.NaN, float.NaN,
//									   float.NaN, float.NaN, float.NaN, float.NaN,
//									   float.NaN, float.NaN, float.NaN, float.NaN,
//									   float.NaN, float.NaN, float.NaN, float.NaN);
//				return false;
//			}

//			float invDet = 1.0f / det;

//			result.m00 = a11 * invDet;
//			result.m10 = a12 * invDet;
//			result.m20 = a13 * invDet;
//			result.m30 = a14 * invDet;

//			result.m01 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
//			result.m11 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
//			result.m21 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
//			result.m31 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

//			float gp_ho = g * p - h * o;
//			float fp_hn = f * p - h * n;
//			float fo_gn = f * o - g * n;
//			float ep_hm = e * p - h * m;
//			float eo_gm = e * o - g * m;
//			float en_fm = e * n - f * m;

//			result.m02 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
//			result.m12 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
//			result.m22 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
//			result.m32 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

//			float gl_hk = g * l - h * k;
//			float fl_hj = f * l - h * j;
//			float fk_gj = f * k - g * j;
//			float el_hi = e * l - h * i;
//			float ek_gi = e * k - g * i;
//			float ej_fi = e * j - f * i;

//			result.m03 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
//			result.m13 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
//			result.m23 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
//			result.m33 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

//			return true;
//		}

//		struct CanonicalBasis
//		{
//			public XYZ Row0;
//			public XYZ Row1;
//			public XYZ Row2;
//		};

//		[System.Security.SecuritySafeCritical]
//		struct VectorBasis
//		{
//			public unsafe XYZ* Element0;
//			public unsafe XYZ* Element1;
//			public unsafe XYZ* Element2;
//		}

//		/// <summary>
//		/// Attempts to extract the scale, translation, and rotation components from the given scale/rotation/translation matrix.
//		/// If successful, the out parameters will contained the extracted values.
//		/// </summary>
//		/// <param name="matrix">The source matrix.</param>
//		/// <param name="scale">The scaling component of the transformation matrix.</param>
//		/// <param name="rotation">The rotation component of the transformation matrix.</param>
//		/// <param name="translation">The translation component of the transformation matrix</param>
//		/// <returns>True if the source matrix was successfully decomposed; False otherwise.</returns>
//		[System.Security.SecuritySafeCritical]
//		public static bool Decompose(Matrix4x4 matrix, out XYZ scale, out Quaternion rotation, out XYZ translation)
//		{
//			bool result = true;

//			unsafe
//			{
//				fixed (XYZ* scaleBase = &scale)
//				{
//					float* pfScales = (float*)scaleBase;
//					const float EPSILON = 0.0001f;
//					float det;

//					VectorBasis vectorBasis;
//					XYZ** pVectorBasis = (XYZ**)&vectorBasis;

//					Matrix4x4 matTemp = Matrix4x4.Identity;
//					CanonicalBasis canonicalBasis = new CanonicalBasis();
//					XYZ* pCanonicalBasis = &canonicalBasis.Row0;

//					canonicalBasis.Row0 = new XYZ(1.0f, 0.0f, 0.0f);
//					canonicalBasis.Row1 = new XYZ(0.0f, 1.0f, 0.0f);
//					canonicalBasis.Row2 = new XYZ(0.0f, 0.0f, 1.0f);

//					translation = new XYZ(
//						matrix.m30,
//						matrix.m31,
//						matrix.m32);

//					pVectorBasis[0] = (XYZ*)&matTemp.m00;
//					pVectorBasis[1] = (XYZ*)&matTemp.m10;
//					pVectorBasis[2] = (XYZ*)&matTemp.m20;

//					*(pVectorBasis[0]) = new XYZ(matrix.m00, matrix.m01, matrix.m02);
//					*(pVectorBasis[1]) = new XYZ(matrix.m10, matrix.m11, matrix.m12);
//					*(pVectorBasis[2]) = new XYZ(matrix.m20, matrix.m21, matrix.m22);

//					scale.X = pVectorBasis[0]->GetLength();
//					scale.Y = pVectorBasis[1]->GetLength();
//					scale.Z = pVectorBasis[2]->GetLength();

//					uint a, b, c;
//					#region Ranking
//					float x = pfScales[0], y = pfScales[1], z = pfScales[2];
//					if (x < y)
//					{
//						if (y < z)
//						{
//							a = 2;
//							b = 1;
//							c = 0;
//						}
//						else
//						{
//							a = 1;

//							if (x < z)
//							{
//								b = 2;
//								c = 0;
//							}
//							else
//							{
//								b = 0;
//								c = 2;
//							}
//						}
//					}
//					else
//					{
//						if (x < z)
//						{
//							a = 2;
//							b = 0;
//							c = 1;
//						}
//						else
//						{
//							a = 0;

//							if (y < z)
//							{
//								b = 2;
//								c = 1;
//							}
//							else
//							{
//								b = 1;
//								c = 2;
//							}
//						}
//					}
//					#endregion

//					if (pfScales[a] < EPSILON)
//					{
//						*(pVectorBasis[a]) = pCanonicalBasis[a];
//					}

//					*pVectorBasis[a] = pVectorBasis[a]->Normalize(); //XYZ.Normalize(*pVectorBasis[a]);

//					if (pfScales[b] < EPSILON)
//					{
//						uint cc;
//						float fAbsX, fAbsY, fAbsZ;

//						fAbsX = (float)Math.Abs(pVectorBasis[a]->X);
//						fAbsY = (float)Math.Abs(pVectorBasis[a]->Y);
//						fAbsZ = (float)Math.Abs(pVectorBasis[a]->Z);

//						#region Ranking
//						if (fAbsX < fAbsY)
//						{
//							if (fAbsY < fAbsZ)
//							{
//								cc = 0;
//							}
//							else
//							{
//								if (fAbsX < fAbsZ)
//								{
//									cc = 0;
//								}
//								else
//								{
//									cc = 2;
//								}
//							}
//						}
//						else
//						{
//							if (fAbsX < fAbsZ)
//							{
//								cc = 1;
//							}
//							else
//							{
//								if (fAbsY < fAbsZ)
//								{
//									cc = 1;
//								}
//								else
//								{
//									cc = 2;
//								}
//							}
//						}
//						#endregion

//						*pVectorBasis[b] = XYZ.Cross(*pVectorBasis[a], *(pCanonicalBasis + cc));
//					}

//					*pVectorBasis[b] = pVectorBasis[b]->Normalize(); // XYZ.Normalize(*pVectorBasis[b]);

//					if (pfScales[c] < EPSILON)
//					{
//						*pVectorBasis[c] = XYZ.Cross(*pVectorBasis[a], *pVectorBasis[b]);
//					}

//					*pVectorBasis[c] = pVectorBasis[c]->Normalize(); //XYZ.Normalize(*pVectorBasis[c]);

//					det = matTemp.GetDeterminant();

//					// use Kramer's rule to check for handedness of coordinate system
//					if (det < 0.0f)
//					{
//						// switch coordinate system by negating the scale and inverting the basis vector on the x-axis
//						pfScales[a] = -pfScales[a];
//						*pVectorBasis[a] = -(*pVectorBasis[a]);

//						det = -det;
//					}

//					det -= 1.0f;
//					det *= det;

//					if ((EPSILON < det))
//					{
//						// Non-SRT matrix encountered
//						rotation = Quaternion.Identity;
//						result = false;
//					}
//					else
//					{
//						// generate the quaternion from the matrix
//						rotation = Quaternion.CreateFromRotationMatrix(matTemp);
//					}
//				}
//			}

//			return result;
//		}

//		/// <summary>
//		/// Transforms the given matrix by applying the given Quaternion rotation.
//		/// </summary>
//		/// <param name="myMat">The source matrix to transform.</param>
//		/// <param name="value">The rotation to apply.</param>
//		/// <returns>The transformed matrix.</returns>
//		public static Matrix4x4 Transform(Matrix4x4 myMat, Quaternion value)
//		{
//			// Compute rotation matrix.
//			float x2 = (float)(value.X + value.X);
//			float y2 = (float)(value.Y + value.Y);
//			float z2 = (float)(value.Z + value.Z);

//			float wx2 = (float)(value.W * x2);
//			float wy2 = (float)(value.W * y2);
//			float wz2 = (float)(value.W * z2);
//			float xx2 = (float)(value.X * x2);
//			float xy2 = (float)(value.X * y2);
//			float xz2 = (float)(value.X * z2);
//			float yy2 = (float)(value.Y * y2);
//			float yz2 = (float)(value.Y * z2);
//			float zz2 = (float)(value.Z * z2);

//			float q11 = 1.0f - yy2 - zz2;
//			float q21 = xy2 - wz2;
//			float q31 = xz2 + wy2;

//			float q12 = xy2 + wz2;
//			float q22 = 1.0f - xx2 - zz2;
//			float q32 = yz2 - wx2;

//			float q13 = xz2 - wy2;
//			float q23 = yz2 + wx2;
//			float q33 = 1.0f - xx2 - yy2;

//			Matrix4x4 result;

//			// First row
//			result.m00 = myMat.m00 * q11 + myMat.m01 * q21 + myMat.m02 * q31;
//			result.m01 = myMat.m00 * q12 + myMat.m01 * q22 + myMat.m02 * q32;
//			result.m02 = myMat.m00 * q13 + myMat.m01 * q23 + myMat.m02 * q33;
//			result.m03 = myMat.m03;

//			// Second row
//			result.m10 = myMat.m10 * q11 + myMat.m11 * q21 + myMat.m12 * q31;
//			result.m11 = myMat.m10 * q12 + myMat.m11 * q22 + myMat.m12 * q32;
//			result.m12 = myMat.m10 * q13 + myMat.m11 * q23 + myMat.m12 * q33;
//			result.m13 = myMat.m13;

//			// Third row
//			result.m20 = myMat.m20 * q11 + myMat.m21 * q21 + myMat.m22 * q31;
//			result.m21 = myMat.m20 * q12 + myMat.m21 * q22 + myMat.m22 * q32;
//			result.m22 = myMat.m20 * q13 + myMat.m21 * q23 + myMat.m22 * q33;
//			result.m23 = myMat.m23;

//			// Fourth row
//			result.m30 = myMat.m30 * q11 + myMat.m31 * q21 + myMat.m32 * q31;
//			result.m31 = myMat.m30 * q12 + myMat.m31 * q22 + myMat.m32 * q32;
//			result.m32 = myMat.m30 * q13 + myMat.m31 * q23 + myMat.m32 * q33;
//			result.m33 = myMat.m33;

//			return result;
//		}

//		/// <summary>
//		/// Transposes the rows and columns of a matrix.
//		/// </summary>
//		/// <param name="matrix">The source matrix.</param>
//		/// <returns>The transposed matrix.</returns>
//		public static Matrix4x4 Transpose(Matrix4x4 matrix)
//		{
//			Matrix4x4 result;

//			result.m00 = matrix.m00;
//			result.m01 = matrix.m10;
//			result.m02 = matrix.m20;
//			result.m03 = matrix.m30;
//			result.m10 = matrix.m01;
//			result.m11 = matrix.m11;
//			result.m12 = matrix.m21;
//			result.m13 = matrix.m31;
//			result.m20 = matrix.m02;
//			result.m21 = matrix.m12;
//			result.m22 = matrix.m22;
//			result.m23 = matrix.m32;
//			result.m30 = matrix.m03;
//			result.m31 = matrix.m13;
//			result.m32 = matrix.m23;
//			result.m33 = matrix.m33;

//			return result;
//		}

//		/// <summary>
//		/// Linearly interpolates between the corresponding values of two matrices.
//		/// </summary>
//		/// <param name="matrix1">The first source matrix.</param>
//		/// <param name="matrix2">The second source matrix.</param>
//		/// <param name="amount">The relative weight of the second source matrix.</param>
//		/// <returns>The interpolated matrix.</returns>
//		public static Matrix4x4 Lerp(Matrix4x4 matrix1, Matrix4x4 matrix2, float amount)
//		{
//			Matrix4x4 result;

//			// First row
//			result.m00 = matrix1.m00 + (matrix2.m00 - matrix1.m00) * amount;
//			result.m01 = matrix1.m01 + (matrix2.m01 - matrix1.m01) * amount;
//			result.m02 = matrix1.m02 + (matrix2.m02 - matrix1.m02) * amount;
//			result.m03 = matrix1.m03 + (matrix2.m03 - matrix1.m03) * amount;

//			// Second row
//			result.m10 = matrix1.m10 + (matrix2.m10 - matrix1.m10) * amount;
//			result.m11 = matrix1.m11 + (matrix2.m11 - matrix1.m11) * amount;
//			result.m12 = matrix1.m12 + (matrix2.m12 - matrix1.m12) * amount;
//			result.m13 = matrix1.m13 + (matrix2.m13 - matrix1.m13) * amount;

//			// Third row
//			result.m20 = matrix1.m20 + (matrix2.m20 - matrix1.m20) * amount;
//			result.m21 = matrix1.m21 + (matrix2.m21 - matrix1.m21) * amount;
//			result.m22 = matrix1.m22 + (matrix2.m22 - matrix1.m22) * amount;
//			result.m23 = matrix1.m23 + (matrix2.m23 - matrix1.m23) * amount;

//			// Fourth row
//			result.m30 = matrix1.m30 + (matrix2.m30 - matrix1.m30) * amount;
//			result.m31 = matrix1.m31 + (matrix2.m31 - matrix1.m31) * amount;
//			result.m32 = matrix1.m32 + (matrix2.m32 - matrix1.m32) * amount;
//			result.m33 = matrix1.m33 + (matrix2.m33 - matrix1.m33) * amount;

//			return result;
//		}

//		/// <summary>
//		/// Returns a new matrix with the negated elements of the given matrix.
//		/// </summary>
//		/// <param name="value">The source matrix.</param>
//		/// <returns>The negated matrix.</returns>
//		public static Matrix4x4 Negate(Matrix4x4 value)
//		{
//			Matrix4x4 result;

//			result.m00 = -value.m00;
//			result.m01 = -value.m01;
//			result.m02 = -value.m02;
//			result.m03 = -value.m03;
//			result.m10 = -value.m10;
//			result.m11 = -value.m11;
//			result.m12 = -value.m12;
//			result.m13 = -value.m13;
//			result.m20 = -value.m20;
//			result.m21 = -value.m21;
//			result.m22 = -value.m22;
//			result.m23 = -value.m23;
//			result.m30 = -value.m30;
//			result.m31 = -value.m31;
//			result.m32 = -value.m32;
//			result.m33 = -value.m33;

//			return result;
//		}

//		/// <summary>
//		/// Adds two matrices together.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The resulting matrix.</returns>
//		public static Matrix4x4 Add(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 result;

//			result.m00 = value1.m00 + value2.m00;
//			result.m01 = value1.m01 + value2.m01;
//			result.m02 = value1.m02 + value2.m02;
//			result.m03 = value1.m03 + value2.m03;
//			result.m10 = value1.m10 + value2.m10;
//			result.m11 = value1.m11 + value2.m11;
//			result.m12 = value1.m12 + value2.m12;
//			result.m13 = value1.m13 + value2.m13;
//			result.m20 = value1.m20 + value2.m20;
//			result.m21 = value1.m21 + value2.m21;
//			result.m22 = value1.m22 + value2.m22;
//			result.m23 = value1.m23 + value2.m23;
//			result.m30 = value1.m30 + value2.m30;
//			result.m31 = value1.m31 + value2.m31;
//			result.m32 = value1.m32 + value2.m32;
//			result.m33 = value1.m33 + value2.m33;

//			return result;
//		}

//		/// <summary>
//		/// Subtracts the second matrix from the first.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The result of the subtraction.</returns>
//		public static Matrix4x4 Subtract(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 result;

//			result.m00 = value1.m00 - value2.m00;
//			result.m01 = value1.m01 - value2.m01;
//			result.m02 = value1.m02 - value2.m02;
//			result.m03 = value1.m03 - value2.m03;
//			result.m10 = value1.m10 - value2.m10;
//			result.m11 = value1.m11 - value2.m11;
//			result.m12 = value1.m12 - value2.m12;
//			result.m13 = value1.m13 - value2.m13;
//			result.m20 = value1.m20 - value2.m20;
//			result.m21 = value1.m21 - value2.m21;
//			result.m22 = value1.m22 - value2.m22;
//			result.m23 = value1.m23 - value2.m23;
//			result.m30 = value1.m30 - value2.m30;
//			result.m31 = value1.m31 - value2.m31;
//			result.m32 = value1.m32 - value2.m32;
//			result.m33 = value1.m33 - value2.m33;

//			return result;
//		}

//		/// <summary>
//		/// Multiplies a matrix by another matrix.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The result of the multiplication.</returns>
//		public static Matrix4x4 Multiply(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 result;

//			// First row
//			result.m00 = value1.m00 * value2.m00 + value1.m01 * value2.m10 + value1.m02 * value2.m20 + value1.m03 * value2.m30;
//			result.m01 = value1.m00 * value2.m01 + value1.m01 * value2.m11 + value1.m02 * value2.m21 + value1.m03 * value2.m31;
//			result.m02 = value1.m00 * value2.m02 + value1.m01 * value2.m12 + value1.m02 * value2.m22 + value1.m03 * value2.m32;
//			result.m03 = value1.m00 * value2.m03 + value1.m01 * value2.m13 + value1.m02 * value2.m23 + value1.m03 * value2.m33;

//			// Second row
//			result.m10 = value1.m10 * value2.m00 + value1.m11 * value2.m10 + value1.m12 * value2.m20 + value1.m13 * value2.m30;
//			result.m11 = value1.m10 * value2.m01 + value1.m11 * value2.m11 + value1.m12 * value2.m21 + value1.m13 * value2.m31;
//			result.m12 = value1.m10 * value2.m02 + value1.m11 * value2.m12 + value1.m12 * value2.m22 + value1.m13 * value2.m32;
//			result.m13 = value1.m10 * value2.m03 + value1.m11 * value2.m13 + value1.m12 * value2.m23 + value1.m13 * value2.m33;

//			// Third row
//			result.m20 = value1.m20 * value2.m00 + value1.m21 * value2.m10 + value1.m22 * value2.m20 + value1.m23 * value2.m30;
//			result.m21 = value1.m20 * value2.m01 + value1.m21 * value2.m11 + value1.m22 * value2.m21 + value1.m23 * value2.m31;
//			result.m22 = value1.m20 * value2.m02 + value1.m21 * value2.m12 + value1.m22 * value2.m22 + value1.m23 * value2.m32;
//			result.m23 = value1.m20 * value2.m03 + value1.m21 * value2.m13 + value1.m22 * value2.m23 + value1.m23 * value2.m33;

//			// Fourth row
//			result.m30 = value1.m30 * value2.m00 + value1.m31 * value2.m10 + value1.m32 * value2.m20 + value1.m33 * value2.m30;
//			result.m31 = value1.m30 * value2.m01 + value1.m31 * value2.m11 + value1.m32 * value2.m21 + value1.m33 * value2.m31;
//			result.m32 = value1.m30 * value2.m02 + value1.m31 * value2.m12 + value1.m32 * value2.m22 + value1.m33 * value2.m32;
//			result.m33 = value1.m30 * value2.m03 + value1.m31 * value2.m13 + value1.m32 * value2.m23 + value1.m33 * value2.m33;

//			return result;
//		}

//		/// <summary>
//		/// Multiplies a matrix by a scalar value.
//		/// </summary>
//		/// <param name="value1">The source matrix.</param>
//		/// <param name="value2">The scaling factor.</param>
//		/// <returns>The scaled matrix.</returns>
//		public static Matrix4x4 Multiply(Matrix4x4 value1, float value2)
//		{
//			Matrix4x4 result;

//			result.m00 = value1.m00 * value2;
//			result.m01 = value1.m01 * value2;
//			result.m02 = value1.m02 * value2;
//			result.m03 = value1.m03 * value2;
//			result.m10 = value1.m10 * value2;
//			result.m11 = value1.m11 * value2;
//			result.m12 = value1.m12 * value2;
//			result.m13 = value1.m13 * value2;
//			result.m20 = value1.m20 * value2;
//			result.m21 = value1.m21 * value2;
//			result.m22 = value1.m22 * value2;
//			result.m23 = value1.m23 * value2;
//			result.m30 = value1.m30 * value2;
//			result.m31 = value1.m31 * value2;
//			result.m32 = value1.m32 * value2;
//			result.m33 = value1.m33 * value2;

//			return result;
//		}

//		/// <summary>
//		/// Returns a new matrix with the negated elements of the given matrix.
//		/// </summary>
//		/// <param name="value">The source matrix.</param>
//		/// <returns>The negated matrix.</returns>
//		public static Matrix4x4 operator -(Matrix4x4 value)
//		{
//			Matrix4x4 m;

//			m.m00 = -value.m00;
//			m.m01 = -value.m01;
//			m.m02 = -value.m02;
//			m.m03 = -value.m03;
//			m.m10 = -value.m10;
//			m.m11 = -value.m11;
//			m.m12 = -value.m12;
//			m.m13 = -value.m13;
//			m.m20 = -value.m20;
//			m.m21 = -value.m21;
//			m.m22 = -value.m22;
//			m.m23 = -value.m23;
//			m.m30 = -value.m30;
//			m.m31 = -value.m31;
//			m.m32 = -value.m32;
//			m.m33 = -value.m33;

//			return m;
//		}

//		/// <summary>
//		/// Adds two matrices together.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The resulting matrix.</returns>
//		public static Matrix4x4 operator +(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 m;

//			m.m00 = value1.m00 + value2.m00;
//			m.m01 = value1.m01 + value2.m01;
//			m.m02 = value1.m02 + value2.m02;
//			m.m03 = value1.m03 + value2.m03;
//			m.m10 = value1.m10 + value2.m10;
//			m.m11 = value1.m11 + value2.m11;
//			m.m12 = value1.m12 + value2.m12;
//			m.m13 = value1.m13 + value2.m13;
//			m.m20 = value1.m20 + value2.m20;
//			m.m21 = value1.m21 + value2.m21;
//			m.m22 = value1.m22 + value2.m22;
//			m.m23 = value1.m23 + value2.m23;
//			m.m30 = value1.m30 + value2.m30;
//			m.m31 = value1.m31 + value2.m31;
//			m.m32 = value1.m32 + value2.m32;
//			m.m33 = value1.m33 + value2.m33;

//			return m;
//		}

//		/// <summary>
//		/// Subtracts the second matrix from the first.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The result of the subtraction.</returns>
//		public static Matrix4x4 operator -(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 m;

//			m.m00 = value1.m00 - value2.m00;
//			m.m01 = value1.m01 - value2.m01;
//			m.m02 = value1.m02 - value2.m02;
//			m.m03 = value1.m03 - value2.m03;
//			m.m10 = value1.m10 - value2.m10;
//			m.m11 = value1.m11 - value2.m11;
//			m.m12 = value1.m12 - value2.m12;
//			m.m13 = value1.m13 - value2.m13;
//			m.m20 = value1.m20 - value2.m20;
//			m.m21 = value1.m21 - value2.m21;
//			m.m22 = value1.m22 - value2.m22;
//			m.m23 = value1.m23 - value2.m23;
//			m.m30 = value1.m30 - value2.m30;
//			m.m31 = value1.m31 - value2.m31;
//			m.m32 = value1.m32 - value2.m32;
//			m.m33 = value1.m33 - value2.m33;

//			return m;
//		}

//		/// <summary>
//		/// Multiplies a matrix by another matrix.
//		/// </summary>
//		/// <param name="value1">The first source matrix.</param>
//		/// <param name="value2">The second source matrix.</param>
//		/// <returns>The result of the multiplication.</returns>
//		public static Matrix4x4 operator *(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			Matrix4x4 m;

//			// First row
//			m.m00 = value1.m00 * value2.m00 + value1.m01 * value2.m10 + value1.m02 * value2.m20 + value1.m03 * value2.m30;
//			m.m01 = value1.m00 * value2.m01 + value1.m01 * value2.m11 + value1.m02 * value2.m21 + value1.m03 * value2.m31;
//			m.m02 = value1.m00 * value2.m02 + value1.m01 * value2.m12 + value1.m02 * value2.m22 + value1.m03 * value2.m32;
//			m.m03 = value1.m00 * value2.m03 + value1.m01 * value2.m13 + value1.m02 * value2.m23 + value1.m03 * value2.m33;

//			// Second row
//			m.m10 = value1.m10 * value2.m00 + value1.m11 * value2.m10 + value1.m12 * value2.m20 + value1.m13 * value2.m30;
//			m.m11 = value1.m10 * value2.m01 + value1.m11 * value2.m11 + value1.m12 * value2.m21 + value1.m13 * value2.m31;
//			m.m12 = value1.m10 * value2.m02 + value1.m11 * value2.m12 + value1.m12 * value2.m22 + value1.m13 * value2.m32;
//			m.m13 = value1.m10 * value2.m03 + value1.m11 * value2.m13 + value1.m12 * value2.m23 + value1.m13 * value2.m33;

//			// Third row
//			m.m20 = value1.m20 * value2.m00 + value1.m21 * value2.m10 + value1.m22 * value2.m20 + value1.m23 * value2.m30;
//			m.m21 = value1.m20 * value2.m01 + value1.m21 * value2.m11 + value1.m22 * value2.m21 + value1.m23 * value2.m31;
//			m.m22 = value1.m20 * value2.m02 + value1.m21 * value2.m12 + value1.m22 * value2.m22 + value1.m23 * value2.m32;
//			m.m23 = value1.m20 * value2.m03 + value1.m21 * value2.m13 + value1.m22 * value2.m23 + value1.m23 * value2.m33;

//			// Fourth row
//			m.m30 = value1.m30 * value2.m00 + value1.m31 * value2.m10 + value1.m32 * value2.m20 + value1.m33 * value2.m30;
//			m.m31 = value1.m30 * value2.m01 + value1.m31 * value2.m11 + value1.m32 * value2.m21 + value1.m33 * value2.m31;
//			m.m32 = value1.m30 * value2.m02 + value1.m31 * value2.m12 + value1.m32 * value2.m22 + value1.m33 * value2.m32;
//			m.m33 = value1.m30 * value2.m03 + value1.m31 * value2.m13 + value1.m32 * value2.m23 + value1.m33 * value2.m33;

//			return m;
//		}

//		/// <summary>
//		/// Multiplies a matrix by a scalar value.
//		/// </summary>
//		/// <param name="value1">The source matrix.</param>
//		/// <param name="value2">The scaling factor.</param>
//		/// <returns>The scaled matrix.</returns>
//		public static Matrix4x4 operator *(Matrix4x4 value1, float value2)
//		{
//			Matrix4x4 m;

//			m.m00 = value1.m00 * value2;
//			m.m01 = value1.m01 * value2;
//			m.m02 = value1.m02 * value2;
//			m.m03 = value1.m03 * value2;
//			m.m10 = value1.m10 * value2;
//			m.m11 = value1.m11 * value2;
//			m.m12 = value1.m12 * value2;
//			m.m13 = value1.m13 * value2;
//			m.m20 = value1.m20 * value2;
//			m.m21 = value1.m21 * value2;
//			m.m22 = value1.m22 * value2;
//			m.m23 = value1.m23 * value2;
//			m.m30 = value1.m30 * value2;
//			m.m31 = value1.m31 * value2;
//			m.m32 = value1.m32 * value2;
//			m.m33 = value1.m33 * value2;
//			return m;
//		}

//		/// <summary>
//		/// Returns a boolean indicating whether the given two matrices are equal.
//		/// </summary>
//		/// <param name="value1">The first matrix to compare.</param>
//		/// <param name="value2">The second matrix to compare.</param>
//		/// <returns>True if the given matrices are equal; False otherwise.</returns>
//		public static bool operator ==(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			return (value1.m00 == value2.m00 && value1.m11 == value2.m11 && value1.m22 == value2.m22 && value1.m33 == value2.m33 && // Check diagonal element first for early out.
//												value1.m01 == value2.m01 && value1.m02 == value2.m02 && value1.m03 == value2.m03 &&
//					value1.m10 == value2.m10 && value1.m12 == value2.m12 && value1.m13 == value2.m13 &&
//					value1.m20 == value2.m20 && value1.m21 == value2.m21 && value1.m23 == value2.m23 &&
//					value1.m30 == value2.m30 && value1.m31 == value2.m31 && value1.m32 == value2.m32);
//		}

//		/// <summary>
//		/// Returns a boolean indicating whether the given two matrices are not equal.
//		/// </summary>
//		/// <param name="value1">The first matrix to compare.</param>
//		/// <param name="value2">The second matrix to compare.</param>
//		/// <returns>True if the given matrices are not equal; False if they are equal.</returns>
//		public static bool operator !=(Matrix4x4 value1, Matrix4x4 value2)
//		{
//			return (value1.m00 != value2.m00 || value1.m01 != value2.m01 || value1.m02 != value2.m02 || value1.m03 != value2.m03 ||
//					value1.m10 != value2.m10 || value1.m11 != value2.m11 || value1.m12 != value2.m12 || value1.m13 != value2.m13 ||
//					value1.m20 != value2.m20 || value1.m21 != value2.m21 || value1.m22 != value2.m22 || value1.m23 != value2.m23 ||
//					value1.m30 != value2.m30 || value1.m31 != value2.m31 || value1.m32 != value2.m32 || value1.m33 != value2.m33);
//		}

//		/// <summary>
//		/// Returns a boolean indicating whether this matrix instance is equal to the other given matrix.
//		/// </summary>
//		/// <param name="other">The matrix to compare this instance to.</param>
//		/// <returns>True if the matrices are equal; False otherwise.</returns>
//		public bool Equals(Matrix4x4 other)
//		{
//			return (m00 == other.m00 && m11 == other.m11 && m22 == other.m22 && m33 == other.m33 && // Check diagonal element first for early out.
//										m01 == other.m01 && m02 == other.m02 && m03 == other.m03 &&
//					m10 == other.m10 && m12 == other.m12 && m13 == other.m13 &&
//					m20 == other.m20 && m21 == other.m21 && m23 == other.m23 &&
//					m30 == other.m30 && m31 == other.m31 && m32 == other.m32);
//		}

//		/// <summary>
//		/// Returns a boolean indicating whether the given Object is equal to this matrix instance.
//		/// </summary>
//		/// <param name="obj">The Object to compare against.</param>
//		/// <returns>True if the Object is equal to this matrix; False otherwise.</returns>
//		public override bool Equals(object obj)
//		{
//			if (obj is Matrix4x4)
//			{
//				return Equals((Matrix4x4)obj);
//			}

//			return false;
//		}

//		/// <summary>
//		/// Returns a String representing this matrix instance.
//		/// </summary>
//		/// <returns>The string representation.</returns>
//		public override string ToString()
//		{
//			CultureInfo ci = CultureInfo.CurrentCulture;

//			return String.Format(ci, "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
//								 m00.ToString(ci), m01.ToString(ci), m02.ToString(ci), m03.ToString(ci),
//								 m10.ToString(ci), m11.ToString(ci), m12.ToString(ci), m13.ToString(ci),
//								 m20.ToString(ci), m21.ToString(ci), m22.ToString(ci), m23.ToString(ci),
//								 m30.ToString(ci), m31.ToString(ci), m32.ToString(ci), m33.ToString(ci));
//		}

//		/// <summary>
//		/// Returns the hash code for this instance.
//		/// </summary>
//		/// <returns>The hash code.</returns>
//		public override int GetHashCode()
//		{
//			return m00.GetHashCode() + m01.GetHashCode() + m02.GetHashCode() + m03.GetHashCode() +
//				   m10.GetHashCode() + m11.GetHashCode() + m12.GetHashCode() + m13.GetHashCode() +
//				   m20.GetHashCode() + m21.GetHashCode() + m22.GetHashCode() + m23.GetHashCode() +
//				   m30.GetHashCode() + m31.GetHashCode() + m32.GetHashCode() + m33.GetHashCode();
//		}
//	}
//}
