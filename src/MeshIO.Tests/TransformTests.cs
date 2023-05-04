using CSMath;
using MeshIO.Tests.Shared;
using System;
using Xunit;

namespace MeshIO.Tests
{
	public class TransformTests
	{
		private CSMathRandom _random = new CSMathRandom();

		[Fact()]
		public void TranslationTest()
		{
			XYZ translation = _random.Next<XYZ>();
			XYZ scale = _random.Next<XYZ>();
			XYZ rotation = _random.Next<XYZ>();

			Transform transform = new Transform(translation, scale, rotation);

			Assert.Equal(translation, transform.Translation);
		}

		[Fact()]
		public void ScaleTest()
		{
			XYZ translation =  _random.Next<XYZ>();
			XYZ scale =  _random.Next<XYZ>();
			XYZ rotation =  _random.Next<XYZ>();

			Transform transform = new Transform(translation, scale, rotation);

			Assert.Equal(scale.X, transform.Scale.X, 15);
		}

		[Fact]
		public void RotationTest()
		{
			XYZ translation =  _random.Next<XYZ>();
			XYZ scale =  _random.Next<XYZ>();
			XYZ rotation =  _random.Next<XYZ>();

			Transform transform = new Transform(translation, scale, rotation);

			Assert.Equal(rotation, transform.EulerRotation);
		}

		[Fact()]
		public void DecomposeTest()
		{
			XYZ translation =  _random.Next<XYZ>();
			XYZ scale =  _random.Next<XYZ>();
			XYZ rotation = new XYZ(90, 0, 0);

			Transform transform = new Transform(translation, scale, rotation);

			transform.TryDecompose(out XYZ t, out XYZ s, out Quaternion r);

			AssertUtils.AreEqual(transform.Translation, t, "translation");
			AssertUtils.AreEqual(transform.Scale, s, "scale");
			AssertUtils.AreEqual(transform.Quaternion, r, "rotation");
		}
	}
}