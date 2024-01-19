using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Primitives
{
	public abstract class Primitive : Entity
	{
		/// <summary>
		/// The geometry is visible or not
		/// </summary>
		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// This geometry can cast shadow or not
		/// </summary>
		public bool CastShadows { get; set; } = true;

		/// <summary>
		/// This geometry can receive shadow or not
		/// </summary>
		public bool ReceiveShadows { get; set; } = true;

		/// <summary>
		/// Process this primitive into a mesh
		/// </summary>
		/// <returns></returns>
		public abstract Mesh CreateMesh();

		public Primitive() : this(string.Empty) { }

		public Primitive(string name) : base(name) { }

		protected Mesh createMesh(List<XYZ> vertices, List<XYZ> normals, List<XY> uvs, List<Polygon> polygons)
		{
			Mesh mesh = new Mesh(this.Name);

			mesh.IsVisible = this.IsVisible;
			mesh.CastShadows = this.CastShadows;
			mesh.ReceiveShadows = this.ReceiveShadows;

			mesh.Vertices.AddRange(vertices);
			mesh.Polygons.AddRange(polygons);

			LayerElementNormal layerNormals = new LayerElementNormal(MappingMode.ByVertex, ReferenceMode.Direct);
			layerNormals.AddRange(normals);
			mesh.Layers.Add(layerNormals);

			LayerElementUV layerUvs = new LayerElementUV(MappingMode.ByVertex, ReferenceMode.Direct);
			layerUvs.AddRange(uvs);
			mesh.Layers.Add(layerUvs);

			return mesh;
		}
	}
}
