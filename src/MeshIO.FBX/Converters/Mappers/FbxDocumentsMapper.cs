using System;
using System.Linq;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxDocumentsMapper : FbxMapperBase
	{
		public override string SectionName { get { return "Documents"; } }

		public Scene RootScene { get; private set; }

		public override void Map(FbxNode node)
		{
			base.Map(node);

			foreach (FbxNode n in node.Where(n => n.Name == "Document"))
			{
				if (n.TryGetNode("RootNode", out FbxNode root))
				{
					this.RootScene = new Scene(n.Properties[1].ToString());
					this.RootScene.Id = Convert.ToUInt64(n.Properties[0]);

					this.RootScene.RootNode.Id = Convert.ToUInt64(root.Value);
				}
			}
		}
	}
}
