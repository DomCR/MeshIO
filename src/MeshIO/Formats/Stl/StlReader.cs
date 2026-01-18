using CSMath;
using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.Formats.Stl
{
	/// <summary>
	/// Provides functionality to read 3D geometry data from STL (Stereolithography) files and streams, supporting both
	/// binary and ASCII STL formats.
	/// </summary>
	/// <remarks>The StlReader class enables importing mesh data from STL files into a scene graph. It supports
	/// reading from file paths or streams and can notify callers of progress or events during the reading process via an
	/// optional notification handler. STL files are commonly used for 3D printing and CAD applications.</remarks>
	public class StlReader : SceneReader
	{
		/// <summary>
		/// Initializes a new instance of the StlReader class for reading STL files from the specified path.
		/// </summary>
		/// <param name="path">The file system path to the STL file to be read. Cannot be null or empty.</param>
		/// <param name="notification">An optional delegate to receive notifications or progress updates during the reading process. Can be null if
		/// notifications are not required.</param>
		public StlReader(string path, NotificationEventHandler notification = null)
			: base(path, notification)
		{
		}

		/// <summary>
		/// Initializes a new instance of the StlReader class using the specified input stream.
		/// </summary>
		/// <param name="stream">The input stream containing the STL data to be read. The stream must be readable and positioned at the beginning of
		/// the STL content.</param>
		/// <param name="notification">An optional delegate to receive notifications or progress updates during the reading process. Can be null if
		/// notifications are not required.</param>
		public StlReader(Stream stream, NotificationEventHandler notification = null)
			: base(stream, notification)
		{
		}

		/// <summary>
		/// Determines whether the underlying stream contains data in binary format.
		/// </summary>
		/// <remarks>This method inspects the stream from its beginning to assess whether the data is in binary format.
		/// The stream position is reset to the start before analysis. Calling this method will modify the stream's current
		/// position.</remarks>
		/// <returns>true if the stream is detected to be in binary format; otherwise, false.</returns>
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
			node.Entities.Add(mesh);

			scene.RootNode.Nodes.Add(node);

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
