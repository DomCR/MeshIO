using Xunit;
using MeshIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Tests
{
	public class TransformTests
	{
		[Fact()]
		public void TransformTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void TryDecomposeTest()
		{
			XYZ translation = new XYZ(100, 200, 300);
			XYZ scale = new XYZ(5, 7, 8);
			//Quaternion rotation = new Quaternion(100, 200, 300,1);

			Transform transform = new Transform();
			transform.Translation = translation;
			transform.Scale = scale;

			Assert.True(Transform.TryDecompose(transform, out XYZ t, out XYZ s, out Quaternion r));
			Assert.Equal(translation, t / s);
			Assert.Equal(scale, s);
		}
	}
}