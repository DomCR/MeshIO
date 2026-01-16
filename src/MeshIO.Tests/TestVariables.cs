using CSUtilities;
using System;
using System.IO;

namespace MeshIO.Tests;

public static class TestVariables
{
	public static int DecimalPrecision { get { return EnvironmentVars.Get<int>("DECIMAL_PRECISION"); } }

	public static double Delta { get { return EnvironmentVars.Get<double>("DELTA"); } }

	public static string DesktopFolder { get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); } }

	public static string InputSamplesFolder { get { return EnvironmentVars.Get<string>("INPUT_SAMPLES_FOLDER"); } }

	public static bool LocalEnv { get { return EnvironmentVars.Get<bool>("LOCAL_ENV"); } }

	public static string OutputSamplesFolder { get { return EnvironmentVars.Get<string>("OUTPUT_SAMPLES_FOLDER"); } }

	public static string SamplesFolder { get { return EnvironmentVars.Get<string>("SAMPLES_FOLDER"); } }

	public static bool SaveOutputInStream { get { return EnvironmentVars.Get<bool>("SAVE_OUTPUT_IN_STREAM"); } }

	public static bool SelfCheckOutput { get { return EnvironmentVars.Get<bool>("SELF_CHECK_OUTPUT"); } }

	static TestVariables()
	{
		EnvironmentVars.SetIfNull("SAMPLES_FOLDER", "../../../../../samples/");
		EnvironmentVars.SetIfNull("OUTPUT_SAMPLES_FOLDER", "../../../../../samples/out");
		EnvironmentVars.SetIfNull("INPUT_SAMPLES_FOLDER", "../../../../../samples/in");
		EnvironmentVars.SetIfNull("LOCAL_ENV", "true");
		EnvironmentVars.SetIfNull("DELTA", "0.00001");
		EnvironmentVars.SetIfNull("DECIMAL_PRECISION", "5");
		EnvironmentVars.SetIfNull("SAVE_OUTPUT_IN_STREAM", "false");
		EnvironmentVars.SetIfNull("SELF_CHECK_OUTPUT", "true");
	}

	public static void CreateOutputFolders()
	{
		string outputSamplesFolder = OutputSamplesFolder;
		string inputSamplesFolder = InputSamplesFolder;

#if NETFRAMEWORK
		string curr = AppDomain.CurrentDomain.BaseDirectory;
		outputSamplesFolder = Path.GetFullPath(Path.Combine(curr, OutputSamplesFolder));
		inputSamplesFolder = Path.GetFullPath(Path.Combine(curr, InputSamplesFolder));
#endif

		craateFolderIfDoesNotExist(outputSamplesFolder);
		craateFolderIfDoesNotExist(inputSamplesFolder);
	}

	private static void craateFolderIfDoesNotExist(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
}