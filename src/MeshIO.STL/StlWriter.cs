using CSMath;
using CSUtilities.IO;
using MeshIO.Entities.Geometries.Layers;
using System;
using System.IO;
using System.Linq;
using System.Text;
using MeshIO.Entities.Geometries;

namespace MeshIO.STL
{
	public class StlWriter : IDisposable
	{
		private System.Globalization.NumberFormatInfo _nfi;

		private StreamIO _stream;

		private StlWriter()
		{
			this._nfi = new();
			this._nfi.NumberDecimalSeparator = ".";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StlWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		public StlWriter(string path) : this()
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			this._stream = new StreamIO(File.Create(path));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StlWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public StlWriter(Stream stream) : this()
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this._stream = new StreamIO(stream);
		}

		/// <summary>
		/// Write a mesh into an STL ascii file
		/// </summary>
		/// <param name="mesh"></param>
		/// <remarks>
		/// The mesh must be formed by triangles, quads are not accpeted for this format. <br/>
		/// The mesh must also contain a <see cref="LayerElementNormal"/> set to <see cref="MappingMode.ByPolygon"/>
		/// </remarks>
		public void WriteAscii(Mesh mesh)
		{
			this.validate(mesh);

			using (TextWriter tw = new StreamWriter(this._stream.Stream, new UTF8Encoding(false)))
			{
				tw.WriteLine($"solid {mesh.Name}");

				LayerElementNormal normals = mesh.Layers.GetLayer<LayerElementNormal>();
				for (int i = 0; i < mesh.Polygons.Count; i++)
				{
					XYZ normal = normals.Normals[i];

					tw.WriteLine($"  facet normal {normal.X.ToString(this._nfi)} {normal.Y.ToString(this._nfi)} {normal.Z.ToString(this._nfi)}");

					tw.WriteLine($"    outer loop");

					foreach (int item in (Triangle)mesh.Polygons[i])
					{
						var v = mesh.Vertices[item];

						tw.WriteLine($"      vertex {v.X.ToString(this._nfi)} {v.Y.ToString(this._nfi)} {v.Z.ToString(this._nfi)}");
					}

					tw.WriteLine($"    endloop");
					tw.WriteLine($"  endfacet");
				}

				tw.WriteLine($"endsolid {mesh.Name}");
			}
		}

		/// <summary>
		/// Write a mesh into an STL binary file
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="header">Optional header parameter</param>
		/// <remarks>
		/// The mesh must be formed by triangles, quads are not accpeted for this format. <br/>
		/// The mesh must also contain a <see cref="LayerElementNormal"/> set to <see cref="MappingMode.ByPolygon"/>
		/// </remarks>
		public void WriteBinary(Mesh mesh, string header = null)
		{
			this.validate(mesh);

			if (string.IsNullOrEmpty(header))
			{
				header = "File created by MeshIO.STL";
			}
			else if (header.Length > 80)
			{
				throw new StlException("Header length must be 80 or less");
			}

			header = header + new string('\0', 80 - header.Length);

			this._stream.Write(header, Encoding.UTF8);
			this._stream.Write(mesh.Polygons.Count);

			LayerElementNormal normals = mesh.Layers.GetLayer<LayerElementNormal>();
			for (int i = 0; i < mesh.Polygons.Count; i++)
			{
				XYZ normal = normals.Normals[i];

				this._stream.Write((float)normal.X);
				this._stream.Write((float)normal.Y);
				this._stream.Write((float)normal.Z);

				foreach (int item in (Triangle)mesh.Polygons[i])
				{
					var v = mesh.Vertices[item];

					this._stream.Write((float)v.X);
					this._stream.Write((float)v.Y);
					this._stream.Write((float)v.Z);
				}

				ushort attByteCount = 0;
				this._stream.Write(attByteCount);
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			this._stream.Dispose();
		}

		private void validate(Mesh mesh)
		{
			if (mesh.Polygons.OfType<Quad>().Any())
				throw new StlException("Quads are not accepted in stl format");

			if (!mesh.Layers.TryGetLayer<LayerElementNormal>(out LayerElementNormal layer) ||
				layer?.MappingMode != MappingMode.ByPolygon)
				throw new StlException("LayerElementNormal must be set for the mesh with the mapping mode set to ByPolygon");
		}
	}
}
