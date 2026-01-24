using MeshIO.Formats.Gltf;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Gltf;

public class GltfReaderLocalTest : IOTestsBase
{
	public static readonly TheoryData<FileModel> V1Files = new();

	public static readonly TheoryData<FileModel> V2Files = new();

	private const string _samplesFolder = "..\\..\\..\\..\\..\\..\\glTF-Sample-Models";

	static GltfReaderLocalTest()
	{
		if (!Directory.Exists(_samplesFolder))
		{
			V1Files.Add(new FileModel());
			V2Files.Add(new FileModel());
			return;
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "1.0"), "*.glb", SearchOption.AllDirectories))
		{
			FileModel model = new FileModel(file);
			V1Files.Add(model);
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "2.0"), "*.glb", SearchOption.AllDirectories))
		{
			V2Files.Add(new FileModel(file));
		}
	}

	public GltfReaderLocalTest(ITestOutputHelper output) : base(output) { }

	[Theory]
	[MemberData(nameof(V1Files))]
	public void ReadGlbV1(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
			return;

		using (GlbReader reader = new GlbReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			Assert.Throws<NotSupportedException>(reader.Read);
		}
	}

	[Theory]
	[MemberData(nameof(V2Files))]
	public void ReadGlbV2(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
			return;

		using (GlbReader reader = new GlbReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			reader.Read();
		}
	}
}
