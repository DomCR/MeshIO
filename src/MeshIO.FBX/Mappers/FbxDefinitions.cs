using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Mappers
{
	public class FbxDefinitionMapper
	{
		private readonly Dictionary<string, FbxNode> _typeDefinitions = new Dictionary<string, FbxNode>();

		public FbxDefinitionMapper(FbxNode node)
		{
			foreach (var item in node.Where(o => o.Name == "ObjectType"))
			{
				if (item.TryGetNode("PropertyTemplate", out FbxNode template))
				{
					this._typeDefinitions[item.Value.ToString()] = template["Properties70"];
				}
			}
		}
	}
}
