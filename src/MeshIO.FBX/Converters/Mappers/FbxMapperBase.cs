using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.FBX.Converters.Mappers
{
	public abstract class FbxMapperBase : ConverterBase, IFbxMapper
	{
		public abstract string SectionName { get; }

		public virtual void Map(FbxNode node)
		{
			if (node.Name != SectionName)
				throw new ArgumentException();
		}

		public virtual IEnumerable<Property> MapProperties(FbxNode node)
		{
			throw new NotImplementedException();
		}
	}
}
