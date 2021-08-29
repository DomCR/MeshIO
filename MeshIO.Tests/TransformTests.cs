using MeshIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MeshIO.Tests
{
	public class TransformTests
	{
		[Fact()]
		public void DecomposeTest()
		{
			System.Numerics.Matrix4x4 myMatrix = new System.Numerics.Matrix4x4((float)-1.0, (float)8.979318677493353e-11, (float)0.0, (float)0.0, (float)-8.979318677493353e-11, (float)-1.0, (float)0.0, (float)0.0, (float)0.0, (float)0.0, (float)1.0, (float)0.0, (float)-136.860107421875, (float)64.45372009277344, (float)3.8203670978546144, (float)1.0);
			System.Numerics.Matrix4x4.Decompose(myMatrix, out System.Numerics.Vector3 s1, out System.Numerics.Quaternion r1, out System.Numerics.Vector3 t1);

			Matrix4 m2 = new Matrix4(
				(float)-1.0, (float)8.979318677493353e-11, (float)0.0, (float)0.0,
				(float)-8.979318677493353e-11, (float)-1.0, (float)0.0, (float)0.0,
				(float)0.0, (float)0.0, (float)1.0, (float)0.0,
				(float)-136.860107421875, (float)64.45372009277344, (float)3.8203670978546144, (float)1.0);

			Transform transform = new Transform(m2.Transpose());
			Transform.TryDecompose(transform, out XYZ t, out XYZ s, out Quaternion r);

			Assert.True(t.X == t1.X && t.Y == t1.Y && t.Z == t1.Z);
			Assert.True(s.X == s1.X && s.Y == s1.Y && s.Z == s1.Z);
			Assert.True(r.X == r1.X && r.Y == r1.Y && r.Z == r1.Z);
		}
	}
}