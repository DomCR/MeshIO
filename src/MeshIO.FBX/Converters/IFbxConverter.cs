namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a fbx scene to a <see cref="FbxRootNode"/>
	/// </summary>
	public interface IFbxConverter
	{
		FbxVersion Version { get; }

		FbxRootNode ToRootNode();
	}
}
