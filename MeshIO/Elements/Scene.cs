using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Scene : Element
	{
		public List<Node> Nodes { get; set; } = new List<Node>();

		public Scene() : base() { }
		public Scene(string name) : base(name) { }
	}
}
