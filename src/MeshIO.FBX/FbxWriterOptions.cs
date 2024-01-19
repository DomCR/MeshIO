namespace MeshIO.FBX
{
	public class FbxWriterOptions
	{
		public bool IsBinaryFormat { get; set; } = false;

		public FbxVersion Version { get; set; } = FbxVersion.v7400;

		public FbxGlobalSettings GlobalSettings { get; set; }
	}
}
