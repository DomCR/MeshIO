using CSUtilities.Attributes;

namespace MeshIO.Formats.Fbx.Connections
{
	internal enum FbxConnectionType
	{
		[StringValue("OO")]
		ObjectObject,
		[StringValue("OP")]
		ObjectProperty,
		[StringValue("PO")]
		PropertyObject,
		[StringValue("PP")]
		PropertyProperty,
	}
}
