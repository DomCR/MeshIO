using MeshIO.Tests.Shared;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxReaderTest : IOTestsBase
	{
		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		static FbxReaderTest()
		{
			AsciiFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesFbx, "*_ascii.fbx"))
			{
				AsciiFiles.Add(file);
			}

			BinaryFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesFbx, "*_binary.fbx"))
			{
				BinaryFiles.Add(file);
			}
		}

		public FbxReaderTest(ITestOutputHelper output) : base(output) { }

		[Theory(Skip = "skipy skip")]
		[MemberData(nameof(AsciiFiles))]
		[MemberData(nameof(BinaryFiles))]
		public void GetVersion(string path)
		{
			using (FbxReader reader = new FbxReader(path))
			{
				var version = reader.GetVersion();
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadAsciiTest(string test)
		{
			Scene scene = readFile(test);
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void ReadBinaryTest(string test)
		{
			readFile(test);
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadWriteAsciiTest(string test)
		{
			Scene scene = readFile(test);

			if (scene == null)
				return;

			FbxWriterOptions options = new FbxWriterOptions
			{
				IsBinaryFormat = false,
			};
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene, options))
			{
				writer.Write();
			}
		}

		private Scene readFile(string path)
		{
			using (FbxReader reader = new FbxReader(path))
			{
				reader.OnNotification += onNotification;
				return reader.Read();
			}
		}
	}
}
