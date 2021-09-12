using System;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests
{
	public abstract class VectorTests<T>
		where T : IVector<T>, new()
	{
		public VectorTestCaseFactory Factory { get; set; }

		protected readonly ITestOutputHelper output;

		public VectorTests(ITestOutputHelper output)
		{
			Random random = new Random();
			Factory = new VectorTestCaseFactory(random.Next());

			this.output = output;
		}

		[Fact]
		public void AdditionTest()
		{
			var test = Factory.CreateOperationCase<T>((o, x) => o + x);
			writeTest(test);

			Assert.Equal(test.Item3, test.Item1.Add(test.Item2));
		}

		[Fact]
		public void SubsctractTest()
		{
			var test = Factory.CreateOperationCase<T>((o, x) => o - x);
			writeTest(test);

			Assert.Equal(test.Item3, test.Item1.Substract(test.Item2));
		}

		[Fact]
		public void MultiplyTest()
		{
			var test = Factory.CreateOperationCase<T>((o, x) => o * x);
			writeTest(test);

			Assert.Equal(test.Item3, test.Item1.Multiply(test.Item2));
		}

		[Fact]
		public void DivideTest()
		{
			(T, T, T) test = Factory.CreateOperationCase<T>((o, x) => o / x);
			writeTest(test);

			Assert.Equal(test.Item3, test.Item1.Divide(test.Item2));
		}

		protected void writeTest((T, T, T) test)
		{
			output.WriteLine($"Item 1 : {test.Item1}");
			output.WriteLine($"Item 2 : {test.Item2}");
			output.WriteLine($"Result : {test.Item3}");
		}
	}
}
