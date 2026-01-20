using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Entities.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeshIO.Formats.Stl
{
	internal class StlTextStreamWriter : IStlStreamWriter
	{
		public event NotificationEventHandler OnNotification;

		private readonly StlWriterOptions _options;

		private readonly Scene _scene;

		private readonly StreamWriter _writer;
		private readonly IEnumerable<Mesh> _meshes;

		public StlTextStreamWriter(Stream stream, Scene scene, StlWriterOptions options)
		{
			this._writer = new StreamWriter(stream, new UTF8Encoding(false));
			this._scene = scene;
			this._options = options;
		}

		public StlTextStreamWriter(Stream stream, IEnumerable<Mesh> meshes, StlWriterOptions options)
		{
			this._writer = new StreamWriter(stream, new UTF8Encoding(false));
			this._meshes = meshes;
			this._options = options;
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
				meshes = this._meshes;
			}

			if (!meshes.Any())
			{
				_writer.WriteLine(StlFileToken.Solid);
				_writer.WriteLine(StlFileToken.EndSolid);
				_writer.Flush();
				return;
			}

			if (this._options.MergeMeshes)
			{
				_writer.WriteLine($"{StlFileToken.Solid} {meshes.First()}");
			}

			foreach (Mesh mesh in meshes)
			{
				if (!this._options.MergeMeshes)
				{
					_writer.WriteLine($"{StlFileToken.Solid} {mesh.Name}");
				}

				if (!mesh.Layers.TryGetLayer<LayerElementNormal>(out var normals)
					|| normals.MappingMode != MappingMode.ByPolygon)
				{
					normals = LayerElementNormal.CreateFlatNormals(mesh);
				}

				var triangles = mesh.Polygons.ToArray();
				for (int i = 0; i < triangles.Length; i++)
				{
					var normal = normals.Normals[i];

					_writer.WriteLine($"{StlFileToken.Facet} {StlFileToken.Normal} {xyzToString(normal)}");

					foreach (int index in triangles[i].ToArray())
					{
						_writer.WriteLine($"{StlFileToken.Vertex} {xyzToString(mesh.Vertices[index])}");
					}

					_writer.WriteLine($"{StlFileToken.EndLoop}");
					_writer.WriteLine($"{StlFileToken.EndFacet}");
				}

				if (!this._options.MergeMeshes)
				{
					_writer.WriteLine(StlFileToken.EndSolid);
				}
			}

			if (this._options.MergeMeshes)
			{
				_writer.WriteLine(StlFileToken.EndSolid);
			}

			_writer.Flush();
		}

		private string xyzToString(XYZ xyz)
		{
			return $"{doubleToString(xyz.X)} {doubleToString(xyz.Y)} {doubleToString(xyz.Z)}";
		}

		private string doubleToString(double value)
		{
			return value.ToString("0.0###############", System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}