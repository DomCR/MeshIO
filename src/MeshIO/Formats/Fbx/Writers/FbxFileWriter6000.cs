using CSUtilities.Extensions;
using MeshIO.Formats.Fbx.Connections;
using System;

namespace MeshIO.Formats.Fbx.Writers
{
	internal class FbxFileWriter6000 : FbxFileWriterBase
	{
		public FbxFileWriter6000(Scene scene, FbxWriterOptions options) : base(scene, options)
		{
		}

		protected override void addFileSections()
		{
			this.fbxRoot.Nodes.Add(this.nodeDocument());
			this.fbxRoot.Nodes.Add(this.nodeReferences());
			this.fbxRoot.Nodes.Add(this.nodeDefinitions());
			this.fbxRoot.Nodes.Add(this.nodeObjects());
			this.fbxRoot.Nodes.Add(this.nodeConnections());
		}

		protected FbxNode nodeDocument()
		{
			FbxNode documents = new FbxNode(FbxFileToken.Document);

			//Ensure name is not null or empty
			rootTemplate.GetIdByVersion(Version);
			documents.Nodes.Add(new FbxNode("Name", rootTemplate.Name));

			return documents;
		}

		protected override FbxNode nodeDefinitions()
		{
			FbxNode definitions = new FbxNode(FbxFileToken.Definitions);

			definitions.Nodes.Add(new FbxNode(FbxFileToken.Version, 100));

			return definitions;
		}

		protected override void setConnectionIds(FbxNode con, FbxConnection c)
		{
			con.Properties.Add(c.Child.GetIdByVersion(this.Version));
			con.Properties.Add(c.Parent.GetIdByVersion(this.Version));
		}
	}
}