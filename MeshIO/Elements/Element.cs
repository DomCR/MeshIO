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

			_id = Utils.CreateId();
		}

		public Element(string name) : this()
		{
			Name = name;
		}

		public override string ToString()
		{
			return $"{this.GetType().Name}:{Name}";
		}
	}
}
