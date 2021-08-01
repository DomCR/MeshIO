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
		public Property this[int index] { get { return _properties.Values.ElementAt(index); } }
		public Property this[string name] { get { return _properties[name]; } }

		private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();
		private Element _owner;

		public PropertyCollection(Element owner)
		{
			_owner = owner;
		}

		public void Add(Property property)
		{
			_properties.Add(property.Name, property);
		}

		/// <inheritdoc/>
		public IEnumerator<Property> GetEnumerator()
		{
			return _properties.Values.GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _properties.Values.GetEnumerator();
		}
	}
}
