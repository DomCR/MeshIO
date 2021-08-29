using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MeshIO.Tests
{
	public class Matrix4Tests
	{
		/*
		 Matrix organization:
		 |m00|m10|m20|m30|
		 |m01|m11|m21|m31|
		 |m02|m12|m22|m32|
		 |m03|m13|m23|m33|
		 */

		[Fact()]
		public void GetDeterminantTest()
		{
			Matrix4 m = new Matrix4(
				 1, 2, 1, 0,
				 0, 3, 1, 1,
				-1, 0, 3, 1,
				 3, 1, 2, 0);

			Assert.Equal(16, m.GetDeterminant());
		}

		[Fact()]
		public void MultiplyTest()
		{
			Matrix4 a = new Matrix4(
				 7, 8, 6, 2,
				 7, 4, 6, 9,
				 5, 8, 5, 3,
				 0, 2, 7, 8);

			Matrix4 b = new Matrix4(
				 3, 4, 0, 0,
				 7, 9, 2, 3,
				 3, 8, 4, 6,
				 6, 4, 2, 4);

			Matrix4 r = new Matrix4(
				107, 156, 44,  68,
				121, 148, 50,  84,
				104, 144, 42,  66,
				83 , 106, 48,  80);

			Assert.Equal(r, Matrix4.Multiply(a, b));
			Assert.Equal(r, a * b);
		}
	}
}
