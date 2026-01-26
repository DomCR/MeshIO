using MeshIO.Formats;
using MeshIO.Formats.Stl;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Stl;

public class StlReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> StlAsciiFiles { get; } = new();
	public static TheoryData<FileModel> StlBinaryFiles { get; } = new();

	static StlReaderTests()
	{
		loadSamples("stl", "_ascii", "stl", StlAsciiFiles);
		loadSamples("stl", "_binary", "stl", StlBinaryFiles);
	}

	public StlReaderTests(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(StlAsciiFiles))]
	public void IsAsciiTest(FileModel test)
	{
		Assert.Equal(ContentType.ASCII, StlReader.GetContentType(File.OpenRead(test.Path)));
	}

	[Theory]
	[MemberData(nameof(StlBinaryFiles))]
	public void IsBinaryTest(FileModel test)
	{
		Assert.Equal(ContentType.Binary, StlReader.GetContentType(File.OpenRead(test.Path)));
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