using MeshIO.Entities.Geometries;
using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.STL.Tests
{
	public class StlReaderTest : IOTestsBase
	{
		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		static StlReaderTest()
		{
			AsciiFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesStl, "*_ascii.stl"))
			{
				AsciiFiles.Add(file);
			}

			BinaryFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesStl, "*_binary.stl"))
			{
				BinaryFiles.Add(file);
			}
		}

		public StlReaderTest(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void IsBinaryTest(string test)
		{
			using (StlReader reader = new StlReader(test))
			{
				Assert.True(reader.IsBinary());
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void IsAsciiTest(string test)
		{
			using (StlReader reader = new StlReader(test))
			{
				Assert.False(reader.IsBinary());
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadAsciiTest(string test)
		{
			this.readFile(test);
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void ReadBinaryTest(string test)
		{
			this.readFile(test);
		}

		private Scene readFile(string path)
		{
			using (StlReader reader = new StlReader(path))
			{
				reader.OnNotification += this.onNotification;
				return reader.Read();
			}
		}
	}
}
