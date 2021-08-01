using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Element
	{
		/// <summary>
		/// Name of the element.
		/// </summary>
		public string Name { get; set; }

		public PropertyCollection Properties { get; }

		internal ulong? _id = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Element()
		{
			Name = string.Empty;
			Properties = new PropertyCollection(this);
		}

		public Element(string name) : this()
		{
			Name = name;
		}

		public Property GetProperty(string property)
		{
			throw new NotImplementedException();
		}

		public void RemoveProperty(string property)
		{
			throw new NotImplementedException();
		}

		public void RemoveProperty(Property property)
		{
			throw new NotImplementedException();
		}

		public void SetProperty(string property, object value)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return $"{this.GetType().Name}:{Name}";
		}
	}
}
