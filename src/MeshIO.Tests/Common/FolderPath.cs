using System.IO;

public static class FolderPath
{
	public const string InFiles = "../../../../Tests/inFiles";
	public const string OutFiles = "../../../../Tests/outFiles";

	public static readonly string InFilesFbx = Path.Combine(InFiles, "fbx");
	public static readonly string OutFilesFbx = Path.Combine(OutFiles, "fbx");

	public static readonly string InFilesObj = Path.Combine(InFiles, "obj");
	public static readonly string OutFilesObj = Path.Combine(OutFiles, "obj");

	public static readonly string InFilesStl = Path.Combine(InFiles, "stl");
	public static readonly string OutFilesStl = Path.Combine(OutFiles, "stl");

	public static readonly string InFilesGltf = Path.Combine(InFiles, "glb-gltf");
	public static readonly string OutFilesGltf = Path.Combine(OutFiles, "glb-gltf");

	static FolderPath()
	{
		string[] directories = new[]
		{
			OutFiles,
			OutFilesFbx,
			OutFilesObj,
			OutFilesStl,
			OutFilesGltf,
		};

		foreach (string d in directories)
		{
			if (!Directory.Exists(d))
			{
				Directory.CreateDirectory(d);
			}
		}
	}
}