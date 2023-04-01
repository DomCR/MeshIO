using MeshIO.Tests.Shared;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxReaderTest : ReaderTestsBase
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

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		[MemberData(nameof(BinaryFiles))]
		public void GetVersion(string path)
		{
			using (FbxReader reader = new FbxReader(path, ErrorLevel.Checked))
			{
				var version = reader.GetVersion();
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadAsciiTest(string test)
		{
			readFile(test);
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void ReadBinaryTest(string test)
		{
			readFile(test);
		}

		private Scene readFile(string path)
		{
			using (FbxReader reader = new FbxReader(path, ErrorLevel.Checked))
			{
				reader.OnNotification += onNotification;

				if (reader.GetVersion() <= FbxVersion.v5800)
				{
					Assert.Throws<NotSupportedException>(reader.Read);
					return null;
				}

				if (reader.GetVersion() <= FbxVersion.v6100)
				{
					Assert.Throws<NotImplementedException>(reader.Read);
					return null;
				}

				return reader.Read();
			}
		}
	}
}
