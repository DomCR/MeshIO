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
	public static readonly TheoryData<FileModel> GlbV1Files = new();

	public static readonly TheoryData<FileModel> GlbV2Files = new();

	public static readonly TheoryData<FileModel> GltfV1Files = new();

	public static readonly TheoryData<FileModel> GltfV2Files = new();

	private const string _samplesFolder = "..\\..\\..\\..\\..\\..\\glTF-Sample-Models";

	static GltfReaderLocalTest()
	{
		if (!Directory.Exists(_samplesFolder))
		{
			GlbV1Files.Add(new FileModel());
			GlbV2Files.Add(new FileModel());
			return;
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "1.0"), "*.glb", SearchOption.AllDirectories))
		{
			FileModel model = new FileModel(file);
			GlbV1Files.Add(model);
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "1.0"), "*.gltf", SearchOption.AllDirectories))
		{
			if (file.Contains("glTF-Embedded"))
				continue;

			FileModel model = new FileModel(file);
			GltfV1Files.Add(model);
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "2.0"), "*.glb", SearchOption.AllDirectories))
		{
			GlbV2Files.Add(new FileModel(file));
		}

		foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "2.0"), "*.gltf", SearchOption.AllDirectories))
		{
			if (file.Contains("Draco") || file.Contains("glTF-Embedded"))
				continue;

			GltfV2Files.Add(new FileModel(file));
		}
	}

	public GltfReaderLocalTest(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(GlbV1Files))]
	public void ReadGlbV1(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
			return;

		Scene scene = null;
		using (GlbReader reader = new GlbReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
		}

		Assert.NotNull(scene);
	}

	[Theory]
	[MemberData(nameof(GlbV2Files))]
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

	[Theory]
	[MemberData(nameof(GltfV1Files))]
	public void ReadGltfV1(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
			return;

		Scene scene = null;
		using (GltfReader reader = new GltfReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
		}

		Assert.NotNull(scene);
	}

	[Theory]
	[MemberData(nameof(GltfV2Files))]
	public void ReadGltfV2(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
			return;

		using (GltfReader reader = new GltfReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			reader.Read();
		}
	}
}