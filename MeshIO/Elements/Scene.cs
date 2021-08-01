using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Scene : Element
	{
		public List<Element> Elements { get; set; } = new List<Element>();
	}
}
