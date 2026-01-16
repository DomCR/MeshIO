using MeshIO.Formats.Fbx;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Fbx;

public class FbxReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> FbxFiles { get; } = new();

	static FbxReaderTests()
	{
		loadSamples("fbx", "fbx", FbxFiles);
	}

	public FbxReaderTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public void IOAsciiTest()
	{
		string inPath = Path.Combine(TestVariables.InputSamplesFolder, "fbx", "sample_basic_box_ascii.fbx");

		Scene scene = null;
		using (FbxReader reader = new FbxReader(File.OpenRead(inPath)))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
		}

		using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene))
		{
			writer.Write();
		}
	}

	[Fact]
	public void IOBinaryTest()
	{
		string inPath = Path.Combine(TestVariables.InputSamplesFolder, "fbx", "sample_basic_box_binary.fbx");

		Scene scene = null;
		using (FbxReader reader = new FbxReader(File.OpenRead(inPath)))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
		}

		using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene))
		{
			writer.Write();
		}
	}

	[Theory]
	[MemberData(nameof(FbxFiles))]
	public void ParseTest(FileModel test)
	{
		using (FbxReader reader = new FbxReader(test.Path))
		{
			reader.OnNotification += onNotification;
			reader.Parse();
		}
	}
}