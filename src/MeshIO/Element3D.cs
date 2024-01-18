using System;

namespace MeshIO
{
	/// <summary>
	/// Base class for all the elements contained in the 3D environment
	/// </summary>
	public abstract class Element3D
	{
		/// <summary>
		/// Unique id to identify this element
		/// </summary>
		public ulong? Id { get; internal set; } = null;

		/// <summary>
		/// Name of the element
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Properties of this element
		/// </summary>
		public PropertyCollection Properties { get; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public Element3D() : this(string.Empty) { }

		public Element3D(string name)
		{
			this.Name = name;
			this.Id = IdUtils.CreateId();
			this.Properties = new PropertyCollection(this);
		}

		/// <summary>
		/// Gets the Id of the object, if is null it sets a value
		/// </summary>
		/// <returns></returns>
		public long GetIdOrDefault()
		{
			if (!Id.HasValue)
			{
				this.Id = IdUtils.CreateId();
			}

			return Math.Abs((long)Id.Value);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.GetType().Name}:{this.Name}";
		}
	}
}
