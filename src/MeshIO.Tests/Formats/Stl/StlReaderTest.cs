using MeshIO.Formats.Stl;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Stl;

public class StlReaderTest : IOTestsBase
{
	public static TheoryData<FileModel> StlAsciiFiles { get; } = new();
	public static TheoryData<FileModel> StlBinaryFiles { get; } = new();

	static StlReaderTest()
	{
		loadSamples("stl", "_ascii", "stl", StlAsciiFiles);
		loadSamples("stl", "_binary", "stl", StlBinaryFiles);
	}

	public StlReaderTest(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(StlAsciiFiles))]
	public void IsAsciiTest(FileModel test)
	{
		using (StlReader reader = new StlReader(test.Path))
		{
			Assert.False(reader.IsBinary());
		}
	}

	[Theory]
	[MemberData(nameof(StlBinaryFiles))]
	public void IsBinaryTest(FileModel test)
	{
		using (StlReader reader = new StlReader(test.Path))
		{
			Assert.True(reader.IsBinary());
		}
	}

	[Theory]
	[MemberData(nameof(StlBinaryFiles))]
	[MemberData(nameof(StlAsciiFiles))]
	public void ReadTest(FileModel test)
	{
		this.readFile(test);
	}

	private Scene readFile(FileModel test)
	{
		using (StlReader reader = new StlReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			return reader.Read();
		}
	}
}