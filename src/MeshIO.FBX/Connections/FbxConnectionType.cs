using CSUtilities.Attributes;

namespace MeshIO.FBX.Connections
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
