using CSUtilities.Extensions;
using CSUtilities.Tests.Mock;
using Xunit;

namespace CSUtilities.Tests
{
	public class EnumUtilsTests
	{
		[Fact]
		public void GetStringValueTest()
		{
			MockEnumWithStringValues value0 = MockEnumWithStringValues.EnumValue_0;

			Assert.Equal("Enum String Value 0", value0.GetStringValue());
		}
	}
}
