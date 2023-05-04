﻿using CSMath;
using Xunit;

namespace MeshIO.Tests.Shared
{
	public static class TestVariables
	{
		public const double Delta = 0.00001d;

		public const int DecimalPrecision = 5;
	}

	public static class AssertUtils
	{
		public static void AreEqual<T>(T expected, T actual, string varname = "undefined")
		{
			switch (expected, actual)
			{
				case (double d1, double d2):
					Assert.Equal(d1, d2, 10);
					break;
				case (XY xy1, XY xy2):
					Assert.True(xy1.IsEqual(xy2, TestVariables.DecimalPrecision), $"Different {varname}");
					break;
				case (XYZ xyz1, XYZ xyz2):
					Assert.True(xyz1.IsEqual(xyz2, TestVariables.DecimalPrecision), $"Different {varname}");
					break;
				case (Quaternion q1, Quaternion q2):
					Assert.True(q1.Equals(q2, TestVariables.DecimalPrecision), $"Different {varname}");
					break;
				default:
					Assert.Equal(expected, actual);
					break;
			}
		}
	}
}
