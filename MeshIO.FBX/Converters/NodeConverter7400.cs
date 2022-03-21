using MeshIO.Core;

namespace MeshIO.FBX.Converters
{
	/// <inheritdoc/>
	/// <remarks>
	/// Class to convert a node structure in the <see cref="FbxVersion.v7400"/> version
	/// </remarks>
	public class NodeConverter7400 : NodeConverterBase
	{
		public NodeConverter7400(FbxRootNode root, NotificationHandler onNotification) : base(root, onNotification) { }
	}
}
