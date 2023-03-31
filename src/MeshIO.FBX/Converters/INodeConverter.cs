namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a <see cref="FbxRootNode"/> into a <see cref="Scene"/>
	/// </summary>
	public interface INodeConverter
	{
		FbxVersion Version { get; }

		Scene ConvertScene();
	}
}
