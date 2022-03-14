using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	/// <summary>
	/// Base class for all the elements contained in the 3D environment
	/// </summary>
	public abstract class Element
	{
		/// <summary>
		/// Unique id to identify this element
		/// </summary>
		public Guid Id { get; internal set; }

		/// <summary>
		/// Name of the element.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Properties of this element
		/// </summary>
		public PropertyCollection Properties { get; }

		[Obsolete]
		internal ulong? _id = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public Element() : this(string.Empty)
		{
			this.Properties = new PropertyCollection(this);

			this.Id = Guid.NewGuid();

			this._id = Utils.CreateId();
		}

		public Element(string name)
		{
			this.Name = name;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.GetType().Name}:{this.Name}";
		}
	}
}
