using System;

namespace MeshIO
{
	public class Property
	{
		public string Name { get; }

		public object Value { get; set; }

		public Element3D Owner { get; internal set; }

		public Property(string name, Element3D owner)
		{
			this.Name = name;
			this.Owner = owner;
		}

		public Property(string name) : this(name, null) { }

		public Property(string name, object value) : this(name, null, value) { }

		public Property(string name, Element3D owner, object value) : this(name, owner)
		{
			this.Value = value;
		}

		public static Property<T> ConvertProperty<T>(Property property)
		{
			Property<T> typed = new Property<T>(property.Name, property.Owner);

			if (!property.Value.GetType().IsEquivalentTo(typeof(T)))
				throw new ArgumentException($"Value is not equivalent to {typeof(T).FullName}", nameof(property));

			typed.Value = (T)Convert.ChangeType(property.Value, typeof(T));

			return typed;
		}
	}

	public class Property<T> : Property
	{
		public new T Value { get; set; }

		public Property(string name, Element3D owner) : base(name, owner) { }

		public Property(string name) : base(name, null) { }

		public Property(string name, T value) : base(name, null, value) { }

		public Property(string name, Element3D owner, T value) : base(name, owner, value) { }
	}
}