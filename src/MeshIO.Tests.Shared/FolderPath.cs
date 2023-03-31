using System.IO;

public static class FolderPath
{
	public const string InFiles = "../../../../Tests/inFiles";
	public const string OutFiles = "../../../../Tests/outFiles";

	public static readonly string InFilesFbx = Path.Combine(InFiles, "fbx");
	public static readonly string OutFilesFbx = Path.Combine(OutFiles, "fbx");
}