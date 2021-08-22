using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Node : Element
	{
		public bool? MultiLayer { get; set; }
		public bool? MultiTake { get; set; }
		public bool Shading { get; set; } = true;
		public string Culling { get; set; } = "CullingOff";

		public Transform Transform { get; set; } = new Transform();

		public List<Element> Children { get; set; } = new List<Element>();

		public Node() : base() { }

		public Node(string name) : base(name) { }

		public IEnumerable<T> GetElements<T>()
		{
			return Children.Where(e => e is T).Cast<T>();
		}
	}
}
