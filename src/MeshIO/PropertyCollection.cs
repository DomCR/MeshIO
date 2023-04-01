using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO
{
    public class PropertyCollection : IEnumerable<Property>
	{
		public Property this[int index] { get { return _properties.Values.ElementAt(index); } }

		public Property this[string name] { get { return _properties[name]; } }

		public int Count { get { return _properties.Count; } }

		private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();

		public Element3D Owner { get; }

        public PropertyCollection(Element3D owner)
        {
            Owner = owner;
        }

        public void Add(Property property)
		{
			if (property.Owner != null)
				throw new ArgumentException("Property already has an owner", nameof(property));

			_properties.Add(property.Name, property);

			property.Owner = this.Owner;
		}

		public bool Contains(string name)
		{
			return _properties.ContainsKey(name);
		}

		public void Remove(Property property)
		{
			this.Remove(property.Name);
		}

		public void Remove(string name)
		{
			_properties.Remove(name);
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
