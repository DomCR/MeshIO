using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX
{
	/// <summary>
	/// Represents a node in an FBX file
	/// </summary>
	public class FbxNode : FbxNodeCollection
	{
		/// <summary>
		/// The node name, which is often a class type
		/// </summary>
		/// <remarks>
		/// The name must be smaller than 256 characters to be written to a binary stream
		/// </remarks>
		public string Name { get; set; }

		/// <summary>
		/// Whether the node is empty of data
		/// </summary>
		public bool IsEmpty => string.IsNullOrEmpty(Name) && Properties.Count == 0 && Nodes.Count == 0;

		/// <summary>
		/// The list of properties associated with the node
		/// </summary>
		/// <remarks>
		/// Supported types are primitives (apart from byte and char),arrays of primitives, and strings
		/// </remarks>
		public List<object> Properties { get; } = new List<object>();

		/// <summary>
		/// The first property element
		/// </summary>
		public object Value
		{
			get { return Properties.Count < 1 ? null : Properties[0]; }
			set
			{
				if (Properties.Count < 1)
					Properties.Add(value);
				else
					Properties[0] = value;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public FbxNode() : base() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public FbxNode(string name) : base()
		{
			Name = name;
		}

		public FbxNode(string name, object value) : this(name)
		{
			Value = value;
		}

		public FbxNode(string name, params object[] properties) : this(name)
		{
			Properties = new List<object>(properties);
		}

		public bool TryGetProperty<T>(int index, out T value)
		{
			var v = this.Properties.ElementAtOrDefault(index);
			if (v is T t)
			{
				value = t;
				return true;
			}
			else
			{
				value = default;
				return false;
			}
		}

		public T GetProperty<T>(int index)
		{
			object value = this.Properties.ElementAtOrDefault(index);
			return (T)Convert.ChangeType(value, typeof(T));
		}

		public override string ToString()
		{
			return $"{Name}:{Value}";
		}
	}
}
