using CSMath;
using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Core;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.STL
{
	/// <summary>
	/// Reader for STL files in ascii or binary
	/// </summary>
	public class StlReader : ReaderBase
	{
		private StreamIO _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="StlReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		public StlReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			this._stream = new StreamIO(path, FileMode.Open, FileAccess.Read);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StlReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public StlReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this._stream = new StreamIO(stream);
		}

		/// <summary>
		/// Check the format of the file
		/// </summary>
		/// <returns>true if is binary</returns>
		public bool IsBinary()
		{
			this._stream.Position = 0;
			this._stream.ReadString(80);
			int nTriangles = this._stream.ReadInt<LittleEndianConverter>();

			return this.checkStreamLenth(nTriangles);
		}

		public override Scene Read()
		{
			Scene scene = new Scene();

			Mesh mesh = this.ReadAsMesh();

			Node node = new Node(mesh.Name);
			node.Add(mesh);

			scene.RootNode.Add(node);

			return scene;
		}

		/// <summary>
		/// Read the STL file
		/// </summary>
		/// <returns><see cref="Mesh"/> defined in the file</returns>
		public Mesh ReadAsMesh()
		{
			this._stream.Position = 0;

			string header = this._stream.ReadString(80);
			this.triggerNotification(header.Replace("\0", ""), NotificationType.Information);

			Mesh mesh = new Mesh();
			LayerElementNormal normals = new LayerElementNormal();
			mesh.Layers.Add(normals);

			int nTriangles = this._stream.ReadInt<LittleEndianConverter>();

			if (this.checkStreamLenth(nTriangles))
			{
				for (int i = 0; i < nTriangles; i++)
				{
					XYZ normal = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());

					normals.Add(normal);

					XYZ v1 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());
					XYZ v2 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());
					XYZ v3 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());

					mesh.AddPolygons(v1, v2, v3);

					ushort attByteCount = this._stream.ReadUShort();
				}
			}
			else
			{
				this._stream.Position = 0;

				string line = this._stream.ReadUntil('\n');
				string name = Regex.Match(line, @"solid \s\n", options: RegexOptions.IgnoreCase).Value;
				mesh.Name = name;

				line = this._stream.ReadUntil('\n');

				while (!line.Contains($"endsolid {name}"))
				{
					XYZ normal = this.readPoint(line, "facet normal");
					normals.Add(normal);

					this.checkLine(this._stream.ReadUntil('\n'), "outer loop");

					XYZ v1 = this.readPoint(this._stream.ReadUntil('\n'), "vertex");
					XYZ v2 = this.readPoint(this._stream.ReadUntil('\n'), "vertex");
					XYZ v3 = this.readPoint(this._stream.ReadUntil('\n'), "vertex");

					mesh.AddPolygons(v1, v2, v3);

					this.checkLine(this._stream.ReadUntil('\n'), "endloop");
					this.checkLine(this._stream.ReadUntil('\n'), "endfacet");

					line = this._stream.ReadUntil('\n');
				}
			}

			return mesh;
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			this._stream.Dispose();
		}

		private bool checkStreamLenth(int nTriangles)
		{
			//Compare the length of the stream to check if is ascii file
			return this._stream.Length == 84 + nTriangles * 50;
		}

		private void checkLine(string line, string match)
		{
			if (string.IsNullOrEmpty(match) &&
				Regex.Match(line, match + @" \s\n", options: RegexOptions.IgnoreCase).Success)
			{
				throw new StlException($"Expected match: {match} | line: {line}");
			}
		}

		private XYZ readPoint(string line, string match)
		{
			this.checkLine(line, match);

			var x = Regex.Match(line, @"\d+(\.\d+)?");
			var y = x.NextMatch();
			var z = y.NextMatch();

			return new XYZ(double.Parse(x.Value), double.Parse(y.Value), double.Parse(z.Value));
		}
	}
}
