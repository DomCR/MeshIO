using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxWriterTest : IOTestsBase
	{
		public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

		public FbxWriterTest(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteEmptyAsciiStream(FbxVersion version)
		{
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), version))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(FbxFileFormat.ASCII);
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteEmptyBinaryStream(FbxVersion version)
		{
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), version))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(FbxFileFormat.Binary);
			}
		}
	}
}
