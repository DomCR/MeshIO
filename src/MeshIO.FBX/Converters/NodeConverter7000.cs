namespace MeshIO.FBX.Converters
{
	/// <inheritdoc/>
	/// <remarks>
	/// Class to convert a node structure to <see cref="FbxVersion.v7000"/> version
	/// </remarks>
	public class NodeConverter7000 : NodeConverterBase
	{
		public NodeConverter7000(FbxRootNode root) : base(root) { }
	}
}
