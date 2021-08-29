using System;
using System.Collections.Generic;

namespace MeshIO
{
	public static class VectorExtensions
	{
		public static double GetLength<T>(this T vector)
			where T : IVector<T>
		{
			double length = 0;

			foreach (var item in vector.GetComponents())
			{
				length += item * item;
			}

			return Math.Sqrt(length);
		}

		public static T Normalize<T>(this T vector)
			where T : IVector<T>, new()
		{
			double length = vector.GetLength();
			double[] components = vector.GetComponents();

			for (int i = 0; i < components.Length; i++)
			{
				components[i] /= length;
			}

			return new T().SetComponents(components);
		}

		public static double Dot<T>(this T vector1, T vector2)
			where T : IVector<T>
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

		public static T Add<T>(this T vector1, T vector2)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(vector1, vector2, (o, x) => o + x);
		}

		public static T Multiply<T>(this T vector1, T vector2)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(vector1, vector2, (o, x) => o * x);
		}

		public static T Divide<T>(this T vector1, T vector2)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(vector1, vector2, (o, x) => o / x);
		}

		private static T applyFunctionByComponentIndex<T>(this T vector1, T vector2, Func<double, double, double> op)
			where T : IVector<T>, new()
		{
			double[] components1 = vector1.GetComponents();
			double[] components2 = vector2.GetComponents();
			double[] result = new double[components1.Length];

			for (int i = 0; i < components1.Length; i++)
			{
				result[i] = op(components1[i], components2[i]);
			}

			return new T().SetComponents(result);
		}
	}
}
