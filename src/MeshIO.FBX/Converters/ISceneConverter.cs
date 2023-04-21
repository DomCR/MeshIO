namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a fbx scene to a <see cref="FbxRootNode"/>
	/// </summary>
	public interface ISceneConverter
	{
		FbxVersion Version { get; }

		FbxRootNode ToRootNode();
	}
}
