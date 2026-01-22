using MeshIO.Formats.Fbx.Connections;

namespace MeshIO.Formats.Fbx.Writers;

internal class FbxFileWriter7000 : FbxFileWriterBase
{
	public FbxFileWriter7000(Scene scene, FbxWriterOptions options) : base(scene, options)
	{
	}

	protected override void addFileSections()
	{
		this.fbxRoot.Nodes.Add(this.nodeGlobalSettings());
		this.fbxRoot.Nodes.Add(this.nodeDocuments());
		this.fbxRoot.Nodes.Add(this.nodeReferences());
		this.fbxRoot.Nodes.Add(this.nodeDefinitions());
		this.fbxRoot.Nodes.Add(this.nodeObjects());
		this.fbxRoot.Nodes.Add(this.nodeConnections());
	}

	protected override void setConnectionIds(FbxNode con, FbxConnection c)
	{
		con.Properties.Add(long.Parse(c.Child.Id));
		con.Properties.Add(long.Parse(c.Parent.Id));
	}
}
