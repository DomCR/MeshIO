using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements.Geometries.Layers
{
	public abstract class LayerElement
	{
		public string Name { get; set; } = string.Empty;
		public MappingMode MappingInformationType { get; set; }
		public ReferenceMode ReferenceInformationType { get; set; }

		protected Geometry _owner;

		public LayerElement(Geometry owner)
		{
			_owner = owner;
		}
	}

	public class LayerCollection	//TODO: Organize and create different layers for a geometry element
	{

	}

	public class LayerElementMaterial : LayerElement
	{
		public List<int> Materials { get; set; } = new List<int>();
		public LayerElementMaterial(Geometry owner) : base(owner)
		{
			MappingInformationType = MappingMode.AllSame;
			ReferenceInformationType = ReferenceMode.IndexToDirect;
			Materials.Add(0);
		}
	}

	public class LayerElementBinormal : LayerElement
	{
		public List<XYZ> BiNormals { get; set; } = new List<XYZ>();
		public LayerElementBinormal(Geometry owner) : base(owner) { }
	}

	public class LayerElementUV : LayerElement
	{
		public List<XY> UV { get; set; } = new List<XY>();
		public List<int> UVIndex { get; set; } = new List<int>();
		public LayerElementUV(Geometry owner) : base(owner) { }
	}

	public class LayerElementSmoothing : LayerElement
	{
		public LayerElementSmoothing(Geometry owner) : base(owner)
		{
		}
	}

	public class LayerElementTangent : LayerElement
	{
		public List<XYZ> Tangents { get; set; } = new List<XYZ>();
		public LayerElementTangent(Geometry owner) : base(owner) { }
	}

	public class LayerElementNormal : LayerElement
	{
		public List<XYZ> Normals { get; set; } = new List<XYZ>();
		public LayerElementNormal(Geometry owner) : base(owner) { }

		public void CalculateFlatNormals()
		{
			if (!(_owner is Mesh mesh))
				return;

			Normals.Clear();

			MappingInformationType = MappingMode.ByPolygon;
			ReferenceInformationType = ReferenceMode.Direct;

			foreach (Polygon item in mesh.Polygons)
			{
				XYZ normal = XYZ.FindNormal(
					mesh.Vertices[item.ToArray()[0]],
					mesh.Vertices[item.ToArray()[1]],
					mesh.Vertices[item.ToArray()[2]]);

				Normals.Add(normal.Normalize());
			}
		}
	}

	public class LayerElementVertexColor : LayerElement
	{
		public LayerElementVertexColor(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementVertexCrease : LayerElement
	{
		public LayerElementVertexCrease(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementEdgeCrease : LayerElement
	{
		public LayerElementEdgeCrease(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementUserData : LayerElement
	{
		public LayerElementUserData(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementVisibility : LayerElement
	{
		public LayerElementVisibility(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementSpecular : LayerElement
	{
		public LayerElementSpecular(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementWeight : LayerElement
	{
		public LayerElementWeight(Geometry owner) : base(owner)
		{
		}
	}
	public class LayerElementHole : LayerElement
	{
		public LayerElementHole(Geometry owner) : base(owner)
		{
		}
	}
}
