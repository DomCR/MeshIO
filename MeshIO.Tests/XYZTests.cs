using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace MeshIO.Tests
{
	public class XYZTests : VectorTests<XYZ>
	{
		public XYZTests(ITestOutputHelper output) : base(output) { }
	}
}
