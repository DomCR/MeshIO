using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxDefinitionsMapper : FbxMapperBase
	{
		public override string SectionName { get { return "Definitions"; } }

		private readonly Dictionary<string, FbxNode> _definitions = new Dictionary<string, FbxNode>();

		public FbxDefinitionsMapper()
		{
		}

		public override void Map(FbxNode node)
		{
			foreach (FbxNode definition in node.Where(n => n.Name == "ObjectType"))
			{

			}
		}

		public IEnumerable<Property> GetDefinitions(string nodeType)
		{
			throw new NotImplementedException();
		}

		private void processTemplate(FbxNode definition)
		{
			if (!definition.TryGetNode("PropertyTemplate", out FbxNode template))
				return;

			if (!template.TryGetNode("Properties70", out FbxNode properties))
				return;

			_definitions.Add(definition.Value.ToString(), template);
		}
	}
}
