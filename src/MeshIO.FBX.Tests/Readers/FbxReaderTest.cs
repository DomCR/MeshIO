using MeshIO.Core;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests.Readers
{
    public class FbxReaderTest
	{
		private const string _samplesFolder = "../../../../Tests/FileSamples/fbx";

		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		private readonly ITestOutputHelper _output;

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

		public FbxReaderTest(ITestOutputHelper output)
		{
			this._output = output;
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
			using (FbxReader reader = new FbxReader(path, ErrorLevel.Checked))
			{
				reader.OnNotification = this.onNotification;

				if (reader.GetVersion() <= FbxVersion.v6100)
				{
					this._output.WriteLine($"Fbx version not implemented: {reader.GetVersion()}");
					return null;
				}

				return reader.Read();
			}
		}

		private void onNotification(NotificationArgs e)
		{
			this._output.WriteLine(e.Message);
		}
	}
}
