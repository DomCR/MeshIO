using MeshIO.Elements;
using System;

namespace MeshIO
{
	public class Property
	{
		public string Name { get; }

		public virtual object Value { get; set; }

		protected Element _owner;

		public Property(string name, Element owner)
		{
			Name = name;
			_owner = owner;
		}

		public Property(string name, Element owner, object value) : this(name, owner)
		{
			Value = value;
		}

		public static Property<T> CreateTypedProperty<T>(Property property)
		{
			Property<T> typed = new Property<T>(property.Name, property._owner);

			if (!property.Value.GetType().IsEquivalentTo(typeof(T)))
				throw new ArgumentException($"Value is not equivalent to {typeof(T).FullName}", nameof(property));

			typed.Value = (T)Convert.ChangeType(property.Value, typeof(T));

			return typed;
		}
	}

	public class Property<T> : Property
	{
		public new T Value { get; set; }

		public Property(string name, Element owner) : base(name, owner) { }

		public Property(string name, Element owner, T value) : base(name, owner, value) { }
	}
}