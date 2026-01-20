using MeshIO.Tests.TestModels;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Common;

public abstract class IOTestsBase
{
	protected readonly ITestOutputHelper _output;

	public IOTestsBase(ITestOutputHelper output)
	{
		_output = output;
	}

	protected static void loadSamples(string folder, string ext, TheoryData<FileModel> files)
	{
		loadSamples(folder, string.Empty, ext, files);
	}

	protected static void loadSamples(string subFolder, string prefix, string ext, TheoryData<FileModel> files)
	{
		string path = TestVariables.InputSamplesFolder;

		if (!string.IsNullOrEmpty(subFolder))
		{
			path = Path.Combine(TestVariables.InputSamplesFolder, subFolder);
		}

		if (!Directory.Exists(path))
		{
			files.Add(new FileModel());
			return;
		}

		foreach (string file in Directory.GetFiles(path, $"*{prefix}.{ext}"))
		{
			files.Add(new FileModel(file));
		}

		if (files.Count == 0)
		{
			files.Add(new FileModel());
		}
	}

	protected void onNotification(object sender, NotificationEventArgs e)
	{
		_output.WriteLine(e.Message);
	}
}