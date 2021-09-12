using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Tests
{
	public class VectorTestCaseFactory
	{
		private Random _random;

		public VectorTestCaseFactory()
		{
			_random = new Random();
		}

		public VectorTestCaseFactory(int seed)
		{
			_random = new Random(seed);
		}

		public (T, T, T) CreateOperationCase<T>(Func<double, double, double> op)
			where T : IVector<T>, new()
		{
			T v1 = new T();
			T v2 = new T();
			T result = new T();

			int n = v1.GetComponents().Length;

			List<double> components1 = new List<double>();
			List<double> components2 = new List<double>();
			List<double> components3 = new List<double>();

			for (int i = 0; i < n; i++)
			{
				var a = _random.Next(-100, 100);
				var b = _random.Next(-100, 100);

				components1.Add(a);
				components2.Add(b);
				components3.Add(op(a, b));
			}

			v1 = v1.SetComponents(components1.ToArray());
			v2 = v2.SetComponents(components2.ToArray());
			result = result.SetComponents(components3.ToArray());

			return (v1, v2, result);
		}
	}
}
