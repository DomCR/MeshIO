using System;
using System.Collections.Generic;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxConnectionMapper : FbxMapperBase
	{
		public override string SectionName { get { return "Connections"; } }

		public Dictionary<ulong, HashSet<ulong>> Connections { get; } = new();

		public FbxConnectionMapper() { }

		public override void Map(FbxNode node)
		{
			base.Map(node);

			foreach (FbxNode c in node)
			{
				string connectionType = c.Properties[0].ToString();

				switch (connectionType)
				{
					case "OO":  // Object(source) to Object(destination).
						break;
					case "OP":  // Object(source) to Property(destination).
					case "PO":  // Property(source) to Object(destination).
					case "PP":  // Property(source) to Property(destination).
					default:
						throw new NotImplementedException();
				}

				ulong childId = Convert.ToUInt64(c.Properties[1]);
				ulong parentId = Convert.ToUInt64(c.Properties[2]);

				if (Connections.TryGetValue(parentId, out HashSet<ulong> lst))
				{
					lst.Add(childId);
				}
				else
				{
					Connections.Add(parentId, new HashSet<ulong> { childId });
				}
			}
		}

		public IEnumerable<ulong> GetChildren(ulong parentId)
		{
			if (this.Connections.TryGetValue(parentId, out HashSet<ulong> lst))
			{
				return lst;
			}
			else
			{
				return new List<ulong>();
			}
		}
	}
}
