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

		/// <summary>
		/// Gets the number of elements that are contained in the collection
		/// </summary>
		public int Count { get { return _properties.Count; } }

		public Element3D Owner { get; }

		private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();

		public PropertyCollection(Element3D owner)
		{
			Owner = owner;
		}

		/// <summary>
		/// Add a property to the collection
		/// </summary>
		/// <param name="property"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Add(Property property)
		{
			if (property.Owner != null)
				throw new ArgumentException("Property already has an owner", nameof(property));

			_properties.Add(property.Name, property);

			property.Owner = this.Owner;
		}

		/// <summary>
		/// Determines whether the collection contains the specified property
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Contains(string name)
		{
			return _properties.ContainsKey(name);
		}

		/// <summary>
		/// Remove property
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public bool Remove(Property property)
		{
			return this.Remove(property.Name);
		}

		/// <summary>
		/// Remove a property by it's name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Remove(string name)
		{
			return _properties.Remove(name);
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
