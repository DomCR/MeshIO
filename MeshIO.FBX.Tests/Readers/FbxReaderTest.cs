using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests.Readers
{
	public class FbxReaderTest
	{
		private const string _samplesFolder = "../../../../samples/fbx";

		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		private readonly ITestOutputHelper output;

		static FbxReaderTest()
		{
			AsciiFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(_samplesFolder, "*_ascii.fbx"))
			{
				AsciiFiles.Add(file);
			}

			BinaryFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(_samplesFolder, "*_binary.fbx"))
			{
				BinaryFiles.Add(file);
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadAsciiTest(string test)
		{
			Scene scene = FbxReader.Read(test, ErrorLevel.Checked);
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void ReadBinaryTest(string test)
		{
			Scene scene = FbxReader.Read(test, ErrorLevel.Checked);
		}
	}
}
