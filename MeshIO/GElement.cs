using MeshIO.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO
{
	/// <summary>
	/// Geometric element.
	/// </summary>
	public class GElement
	{
		/// <summary>
		/// Name of the current element.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// All the different geometries that create this element.
		/// </summary>
		public List<Mesh> Geometries { get; set; }
		/// <summary>
		/// Independent elements in the hirearchy of this element.
		/// </summary>
		public List<GElement> Subelements { get; set; }
		/// <summary>
		/// Materials in this element.
		/// </summary>
		public List<Material> Materials { get; set; }
		public GElement Parent { get; set; }
		public bool HasParent { get { return Parent != null; } }
		public List<Property> Properties { get; set; } = new List<Property>();
		/// <summary>
		/// Initialize an empty element.
		/// </summary>
		public GElement()
		{
			Name = Guid.NewGuid().ToString();
			Geometries = new List<Mesh>();
			Subelements = new List<GElement>();
			Materials = new List<Material>();
		}
		//**************************************************************************************
		/// <summary>
		/// Add the geometry and link it to the parent.
		/// </summary>
		/// <param name="geometry"></param>
		public void AddGeometry(Mesh geometry)
		{
			//Check materials
			//if (!this.Materials.Select(o => o.Name).Contains(geometry.Material.Name))
			//{
			//    //Material does not exist, added to the current element
			//    this.Materials.Add(geometry.Material);
			//}

			//Add geometry to the list linking the parent
			Geometries.Add(geometry);
		}
		public void AddGeometries(IEnumerable<Mesh> geometries)
		{
			foreach (Mesh g in geometries)
			{
				AddGeometry(g);
			}
		}
		/// <summary>
		/// Add the geometry and link it to the parent.
		/// </summary>
		/// <param name="element"></param>
		public void AddSubelement(GElement element)
		{
			//Check materials
			//if (!this.Materials.Select(o => o.Name).Contains(geometry.Material.Name))
			//{
			//    //Material does not exist, added to the current element
			//    this.Materials.Add(geometry.Material);
			//}

			//Add geometry to the list linking the parent
			element.Parent = this;
			Subelements.Add(element);
		}
		public void AddSubelements(IEnumerable<GElement> elements)
		{
			foreach (GElement e in elements)
			{
				AddSubelement(e);
			}
		}
		/// <summary>
		/// Return all the different materials in the <see cref="GElement"/>.
		/// </summary>
		/// <remarks>
		/// The materials will be distinct by name.
		/// </remarks>
		/// <returns></returns>
		public List<Material> GetMaterials()
		{
			//Setup a list with all the materials in the element
			List<Material> materials = new List<Material>();
			materials.AddRange(Geometries.Select(o => o.Material));
			foreach (GElement item in Subelements)
			{
				materials.AddRange(item.GetMaterials());
			}

			return materials;
		}
	}
}
