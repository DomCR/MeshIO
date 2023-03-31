namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a fbx scene to a <see cref="FbxRootNode"/> following the standards of the version <see cref="FbxVersion.v7400"/>
	/// </summary>
	public class FbxConverter7400 : FbxConverterBase
	{
		public FbxConverter7400(Scene scene) : base(scene, FbxVersion.v7400) { }
	}
}
