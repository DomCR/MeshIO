using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.IO;

namespace MeshIO.Formats.Stl;

internal class StlBinaryStreamReader : IStlStreamReader
{
	public event NotificationEventHandler OnNotification;

	private readonly BinaryReader _reader;

	public StlBinaryStreamReader(Stream stream)
	{
		this._reader = new BinaryReader(stream);
	}

	public IEnumerable<Mesh> Read()
	{
		List<Mesh> meshes = new List<Mesh>();

		Mesh mesh = new Mesh();
		LayerElementNormal normals = new(MappingMode.ByPolygon, ReferenceMode.Direct);
		mesh.Layers.Add(normals);

		//Header
		this._reader.ReadBytes(80);
		uint triangleCount = this._reader.ReadUInt32();

		int[] indices = new int[3];
		XYZ[] vertices = new XYZ[3];
		for (int i = 0; i < triangleCount; i++)
		{
			var normalx = (double)this._reader.ReadSingle();
			var normaly = (double)this._reader.ReadSingle();
			var normalz = (double)this._reader.ReadSingle();

			for (int j = 0; j < 3; j++)
			{
				vertices[j].X = this._reader.ReadSingle();
				vertices[j].Y = this._reader.ReadSingle();
				vertices[j].Z = this._reader.ReadSingle();
			}

			mesh.AddPolygons(vertices);
			normals.Add(new XYZ(normalx, normaly, normalz));

			this._reader.ReadUInt16();
		}

		meshes.Add(mesh);
		return meshes;
	}
}