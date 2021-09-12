using System;
using System.Collections.Generic;

namespace MeshIO
{
	public static class VectorExtensions
	{
		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		/// <returns>The vector's length.</returns>
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

		/// <summary>
		/// Returns a vector with the same direction as the given vector, but with a length of 1.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
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

		/// <summary>
		/// Returns the dot product of two vectors.
		/// </summary>
		/// <param name="left">The first vector.</param>
		/// <param name="right">The second vector.</param>
		/// <returns>The dot product.</returns>
		public static double Dot<T>(this T left, T right)
			where T : IVector<T>
		{
			var components1 = left.GetComponents();
			var components2 = right.GetComponents();
			double result = 0;

			for (int i = 0; i < components1.Length; i++)
			{
				result += components1[i] * components2[i];
			}

			return result;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
		public static bool IsEqual<T>(this T left, T right)
			where T : IVector<T>
		{
			var components1 = left.GetComponents();
			var components2 = right.GetComponents();

			for (int i = 0; i < components1.Length; i++)
			{
				if (components1[i] != components2[i])
					return false;
			}

			return true;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
		public static T Add<T>(this T left, T right)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(left, right, (o, x) => o + x);
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static T Substract<T>(this T left, T right)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(left, right, (o, x) => o - x);
		}

		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
		public static T Multiply<T>(this T left, T right)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(left, right, (o, x) => o * x);
		}

		/// <summary>
		/// Divides the first vector by the second.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
		public static T Divide<T>(this T left, T right)
			where T : IVector<T>, new()
		{
			return applyFunctionByComponentIndex(left, right, (o, x) => o / x);
		}

		/// <summary>
		/// Applies a function in all the components of a vector by order
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="op"></param>
		/// <returns></returns>
		private static T applyFunctionByComponentIndex<T>(this T left, T right, Func<double, double, double> op)
			where T : IVector<T>, new()
		{
			double[] components1 = left.GetComponents();
			double[] components2 = right.GetComponents();
			double[] result = new double[components1.Length];

			for (int i = 0; i < components1.Length; i++)
			{
				result[i] = op(components1[i], components2[i]);
			}

			return new T().SetComponents(result);
		}
	}
}
