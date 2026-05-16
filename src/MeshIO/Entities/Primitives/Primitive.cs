using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Primitives;

/// <summary>
/// The base class of all primitives, which can be converted to mesh. It is also an entity, so it has a name and can be added to a scene.
/// </summary>
public abstract class Primitive : Entity
{
	/// <summary>
	/// This geometry can cast shadow or not
	/// </summary>
	public bool CastShadows { get; set; } = true;

	/// <summary>
	/// The geometry is visible or not
	/// </summary>
	public bool IsVisible { get; set; } = true;

	/// <summary>
	/// This geometry can receive shadow or not
	/// </summary>
	public bool ReceiveShadows { get; set; } = true;

	protected Primitive(string name) : base(name)
	{
	}

	/// <summary>
	/// Convert this primitive to mesh. The mesh will be added to the scene, and the primitive will be removed from the scene. So after calling this method, the primitive will not exist in the scene, but the mesh will exist in the scene. If you want to keep the primitive in the scene, you can call this method and then add the primitive back to the scene.
	/// </summary>
	/// <remarks>
	/// The current implementation returns a mesh with no shared vertices and the following layers:<br/>
	/// <see cref="LayerElementNormal"/><br/>
	/// <see cref="LayerElementUV"/><br/>
	/// configured with <see cref="MappingMode.ByVertex"/> and <see cref=" ReferenceMode.Direct"/>
	/// </remarks>
	/// <returns></returns>
	public abstract Mesh ToMesh();

	protected Mesh createMesh(IEnumerable<XYZ> vertices, IEnumerable<XYZ> normals, IEnumerable<XY> uvs, IEnumerable<Polygon> polygons)
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