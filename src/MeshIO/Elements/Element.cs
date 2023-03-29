using System;

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
		public ulong? Id { get; internal set; } = null;

		/// <summary>
		/// Name of the element.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Properties of this element
		/// </summary>
		public PropertyCollection Properties { get; }

		[Obsolete("use the public ulong to administrate ids")]
		internal ulong? _id = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public Element() : this(string.Empty) { }

		public Element(string name)
		{
			this.Name = name;

			this.Properties = new PropertyCollection(this);

			this._id = Utils.CreateId();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.GetType().Name}:{this.Name}";
		}
	}
}
