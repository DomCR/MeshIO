using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO
{
	public class Property
	{
		public string Name { get; set; }
		public Type StorageType { get; set; }
		public object Value { get; set; }
	}
}
