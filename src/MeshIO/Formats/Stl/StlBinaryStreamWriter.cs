using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeshIO.Formats.Stl;

internal class StlBinaryStreamWriter : IStlStreamWriter
{
	public event NotificationEventHandler OnNotification;

	private readonly Scene _scene;

	private readonly BinaryWriter _writer;
	private readonly IEnumerable<Mesh> _meshes;

	public StlBinaryStreamWriter(Stream stream, Scene scene)
	{
		this._writer = new BinaryWriter(stream, Encoding.UTF8);
		this._scene = scene;
	}

	public StlBinaryStreamWriter(Stream stream, IEnumerable<Mesh> meshes)
	{
		this._writer = new BinaryWriter(stream, Encoding.UTF8);
		this._meshes = meshes;
	}

	public void Write()
	{
		IEnumerable<Mesh> meshes = null;
		if (this._scene != null)
		{
			meshes = StlWriterUtils.ExtractMeshes(this._scene.RootNode);
		}
		else
		{
			meshes = this._meshes.Select(m => StlWriterUtils.Prepare(m));
		}

		for (int i = 0; i < 80; i++)
		{
			//Header
			this._writer.Write((byte)0);
		}

		if (!meshes.Any())
		{
			this._writer.Write(0u);
			this._writer.Flush();
			return;
		}

		int triangleCount = 0;
		var ms = new MemoryStream();
		using (var meshWriter = new BinaryWriter(ms, Encoding.UTF8))
		{
			foreach (Mesh mesh in meshes)
			{
				LayerElementNormal layer = mesh.Layers.GetLayer<LayerElementNormal>();
				triangleCount += mesh.Polygons.Count;

				var triangles = mesh.Polygons.OfType<Triangle>().ToArray();
				for (int i = 0; i < triangles.Length; i++)
				{
					var normal = layer.Normals[i];
					meshWriter.Write((float)normal.X);
					meshWriter.Write((float)normal.Y);
					meshWriter.Write((float)normal.Z);

					var triangle = triangles[i];
					foreach (int index in triangle.ToArray())
					{
						var v = mesh.Vertices[index];
						meshWriter.Write((float)(v.X));
						meshWriter.Write((float)(v.Y));
						meshWriter.Write((float)(v.Z));
					}

					meshWriter.Write((ushort)0);
				}
			}

			this._writer.Write(triangleCount);
			this._writer.Write(ms.ToArray());
		}

		this._writer.Flush();
	}

	private Mesh mergeMeshes(IEnumerable<Mesh> meshes)
	{
		throw new NotImplementedException();
	}
}