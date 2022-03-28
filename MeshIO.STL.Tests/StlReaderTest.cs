using MeshIO.Core;
using MeshIO.Elements;
using MeshIO.Elements.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.STL.Tests
{
	public class StlReaderTest
	{
		private const string _samplesFolder = "../../../../samples/stl";

		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		private readonly ITestOutputHelper _output;

		static StlReaderTest()
		{
			AsciiFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(_samplesFolder, "*_ascii.stl"))
			{
				AsciiFiles.Add(file);
			}

			BinaryFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(_samplesFolder, "*_binary.stl"))
			{
				BinaryFiles.Add(file);
			}
		}

		public StlReaderTest(ITestOutputHelper output)
		{
			this._output = output;
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void IsBinaryTest(string test)
		{
			using (StlReader reader = new StlReader(test, onNotification))
			{
				Assert.True(reader.IsBinary());
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void IsAsciiTest(string test)
		{
			using (StlReader reader = new StlReader(test, onNotification))
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

		private Mesh readFile(string path)
		{
			using (StlReader reader = new StlReader(path, onNotification))
			{
				return reader.Read();
			}
		}

		private void onNotification(NotificationArgs e)
		{
			this._output.WriteLine(e.Message);
		}
	}
}
