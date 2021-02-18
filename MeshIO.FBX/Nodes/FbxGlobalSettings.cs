using MeshIO.FBX.Nodes.Properties;

namespace MeshIO.FBX.Nodes
{
	public class FbxGlobalSettings : FbxEmitter
	{
		public override string ClassName { get { return "GlobalSettings"; } }
		public FbxPropertyCollection Properties { get; set; } = new FbxPropertyCollection();
		public FbxGlobalSettings() : base() { }
		public FbxGlobalSettings(FbxNode node)
		{

		}
	}
}
