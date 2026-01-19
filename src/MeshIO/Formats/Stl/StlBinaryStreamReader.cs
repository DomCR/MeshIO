using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.IO;

namespace MeshIO.Formats.Stl
{
	internal class StlBinaryStreamReader : IStlStreamReader
	{
		private readonly BinaryReader reader;
		public StlBinaryStreamReader(Stream stream)
		{
			reader = new BinaryReader(stream);
		}

		public event NotificationEventHandler OnNotification;

		public IEnumerable<Mesh> Read()
		{
			List<Mesh> meshes = new List<Mesh>();

			Mesh mesh = new Mesh();
			LayerElementNormal normals = new(MappingMode.ByPolygon, ReferenceMode.Direct);
			mesh.Layers.Add(normals);

			//Header
			reader.ReadBytes(80);
			uint triangleCount = reader.ReadUInt32();

			int[] indices = new int[3];
			XYZ[] vertices = new XYZ[3];
			for (int i = 0; i < triangleCount; i++)
			{
				var normalx = (double)reader.ReadSingle();
				var normaly = (double)reader.ReadSingle();
				var normalz = (double)reader.ReadSingle();

				for (int j = 0; j < 3; j++)
				{
					vertices[j].X = reader.ReadSingle();
					vertices[j].Y = reader.ReadSingle();
					vertices[j].Z = reader.ReadSingle();
				}

				mesh.AddPolygons(vertices);
				normals.Add(new XYZ(normalx, normaly, normalz));
			}

			meshes.Add(mesh);
			return meshes;
		}
	}
}
