using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	/// <summary>
	/// A node that contains the different entities and materials that are related to each other
	/// </summary>
	public class Node : Element
	{
		[Obsolete]
		public bool? MultiLayer { get; set; }
		[Obsolete]
		public bool? MultiTake { get; set; }
		[Obsolete]
		public bool Shading { get; set; } = true;
		[Obsolete]
		public string Culling { get; set; } = "CullingOff";

		public Transform Transform { get; internal set; } = new Transform();

		public Node Parent { get; }

		public List<Element> Children { get; set; } = new List<Element>();

		public Node() : base() { }

		public Node(string name) : base(name) { }

		public IEnumerable<T> GetElements<T>()
			where T : Element
		{
			return this.Children.Where(e => e is T).Cast<T>();
		}
	}
}
