using CSMath;
using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Core;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.STL
{
	/// <summary>
	/// Reader for STL files in ascii or binary
	/// </summary>
	public class StlReader : ReaderBase
	{
		private StreamIO _streamIO;

		/// <summary>
		/// Initializes a new instance of the <see cref="StlReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		public StlReader(string path) : this(new FileStream(path, FileMode.Open))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StlReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public StlReader(Stream stream) : base(stream)
		{
			this._streamIO = new StreamIO(this._stream);
		}

		/// <summary>
		/// Check the format of the file
		/// </summary>
		/// <returns>true if is binary</returns>
		public bool IsBinary()
		{
			this._streamIO.Position = 0;
			this._streamIO.ReadString(80);
			int nTriangles = this._streamIO.ReadInt<LittleEndianConverter>();

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
			this._streamIO.Position = 0;

			string header = this._streamIO.ReadString(80);
			this.triggerNotification(header.Replace("\0", ""), NotificationType.Information);

			Mesh mesh = new Mesh();
			LayerElementNormal normals = new LayerElementNormal();
			mesh.Layers.Add(normals);

			int nTriangles = this._streamIO.ReadInt<LittleEndianConverter>();

			if (this.checkStreamLenth(nTriangles))
			{
				for (int i = 0; i < nTriangles; i++)
				{
					XYZ normal = new XYZ(this._streamIO.ReadSingle(), this._streamIO.ReadSingle(), this._streamIO.ReadSingle());

					normals.Add(normal);

					XYZ v1 = new XYZ(this._streamIO.ReadSingle(), this._streamIO.ReadSingle(), this._streamIO.ReadSingle());
					XYZ v2 = new XYZ(this._streamIO.ReadSingle(), this._streamIO.ReadSingle(), this._streamIO.ReadSingle());
					XYZ v3 = new XYZ(this._streamIO.ReadSingle(), this._streamIO.ReadSingle(), this._streamIO.ReadSingle());

					mesh.AddPolygons(v1, v2, v3);

					ushort attByteCount = this._streamIO.ReadUShort();
				}
			}
			else
			{
				this._streamIO.Position = 0;

				string line = this._streamIO.ReadUntil('\n');
				string name = Regex.Match(line, @"solid \s\n", options: RegexOptions.IgnoreCase).Value;
				mesh.Name = name;

				line = this._streamIO.ReadUntil('\n');

				while (!line.Contains($"endsolid {name}"))
				{
					XYZ normal = this.readPoint(line, "facet normal");
					normals.Add(normal);

					this.checkLine(this._streamIO.ReadUntil('\n'), "outer loop");

					XYZ v1 = this.readPoint(this._streamIO.ReadUntil('\n'), "vertex");
					XYZ v2 = this.readPoint(this._streamIO.ReadUntil('\n'), "vertex");
					XYZ v3 = this.readPoint(this._streamIO.ReadUntil('\n'), "vertex");

					mesh.AddPolygons(v1, v2, v3);

					this.checkLine(this._streamIO.ReadUntil('\n'), "endloop");
					this.checkLine(this._streamIO.ReadUntil('\n'), "endfacet");

					line = this._streamIO.ReadUntil('\n');
				}
			}

			return mesh;
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			this._streamIO.Dispose();
		}

		private bool checkStreamLenth(int nTriangles)
		{
			//Compare the length of the stream to check if is ascii file
			return this._streamIO.Length == 84 + nTriangles * 50;
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
