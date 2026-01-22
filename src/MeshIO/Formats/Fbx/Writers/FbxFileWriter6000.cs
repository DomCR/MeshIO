using CSUtilities.Extensions;
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

			string name = this.Scene.Name.IsNullOrEmpty() ? "Scene" : this.Scene.Name;
			documents.Nodes.Add(new FbxNode("Name", name));

			return documents;
		}
	}
}