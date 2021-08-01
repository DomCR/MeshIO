using MeshIO.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	public class PropertyCollection : IEnumerable<Property>
	{
		private readonly List<Property> _properties = new List<Property>();
		protected Element _owner;

		public PropertyCollection(Element owner)
		{
			_owner = owner;
		}

		public void Add(Property property)
		{
			_properties.Add(property);
		}

		public IEnumerator<Property> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _properties.GetEnumerator();
		}
	}
}
