using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxDefinitionsMapper : FbxMapperBase
	{
		public override string SectionName { get { return "Definitions"; } }

		private readonly Dictionary<string, IEnumerable<Property>> _definitions = new();

		public FbxDefinitionsMapper()
		{
		}

		public override void Map(FbxNode node)
		{
			base.Map(node);

			foreach (FbxNode definition in node.Where(n => n.Name == "ObjectType"))
			{
				this.processTemplate(definition);
			}
		}

		public IEnumerable<Property> GetDefinitions(string nodeType)
		{
			if (this._definitions.TryGetValue(nodeType, out var definition))
			{
				return definition;
			}
			else
			{
				return new List<Property>();
			}
		}

		private void processTemplate(FbxNode definition)
		{
			if (!definition.TryGetNode("PropertyTemplate", out FbxNode template))
				return;

			if (!template.TryGetNode("Properties70", out FbxNode properties))
				return;

			_definitions.Add(definition.Value.ToString(), this.MapProperties(properties));
		}
	}
}
