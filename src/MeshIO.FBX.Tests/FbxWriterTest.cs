using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxWriterTest : IOTestsBase
	{
		public static readonly TheoryData<FbxVersion> Versions = new TheoryData<FbxVersion>();

		static FbxWriterTest()
		{
			Versions.Add(FbxVersion.v7000);
			Versions.Add(FbxVersion.v7100);
			Versions.Add(FbxVersion.v7200);
			Versions.Add(FbxVersion.v7300);
			Versions.Add(FbxVersion.v7400);
			Versions.Add(FbxVersion.v7500);
			Versions.Add(FbxVersion.v7600);
			Versions.Add(FbxVersion.v7700);
		}

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
