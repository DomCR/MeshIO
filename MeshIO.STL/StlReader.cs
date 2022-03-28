using CSMath;
using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Core;
using MeshIO.Elements.Geometries;
using MeshIO.Elements.Geometries.Layers;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.STL
{
	public class StlReader : ReaderBase, IDisposable
	{
		private StreamIO _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="StlReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		/// <param name="onNotification"></param>
		public StlReader(string path, NotificationHandler onNotification = null)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			this._stream = new StreamIO(path, FileMode.Open, FileAccess.Read);

			this.OnNotification = onNotification;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="errorLevel"></param>
		public StlReader(Stream stream, NotificationHandler onNotification = null)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this._stream = new StreamIO(stream);

			this.OnNotification = onNotification;
		}

		public bool IsBinary()
		{
			this._stream.Position = 0;
			this._stream.ReadString(80);
			int nTriangles = this._stream.ReadInt<LittleEndianConverter>();

			return checkStreamLenth(nTriangles);
		}

		public Mesh Read()
		{
			this._stream.Position = 0;

			string header = this._stream.ReadString(80);
			this.OnNotification?.Invoke(new NotificationArgs(header.Replace("\0", "")));

			Mesh mesh = new Mesh();
			LayerElementNormal normals = new LayerElementNormal();
			mesh.Layers.Add(normals);

			int nTriangles = this._stream.ReadInt<LittleEndianConverter>();

			if (checkStreamLenth(nTriangles))
			{
				for (int i = 0; i < nTriangles; i++)
				{
					XYZ normal = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());

					normals.Add(normal);

					XYZ v1 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());
					XYZ v2 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());
					XYZ v3 = new XYZ(this._stream.ReadSingle(), this._stream.ReadSingle(), this._stream.ReadSingle());

					mesh.AddTriangles(v1, v2, v3);

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
					XYZ normal = readPoint(line, "facet normal");
					normals.Add(normal);

					this.checkLine(this._stream.ReadUntil('\n'), "outer loop");

					XYZ v1 = readPoint(this._stream.ReadUntil('\n'), "vertex");
					XYZ v2 = readPoint(this._stream.ReadUntil('\n'), "vertex");
					XYZ v3 = readPoint(this._stream.ReadUntil('\n'), "vertex");

					mesh.AddTriangles(v1, v2, v3);

					this.checkLine(this._stream.ReadUntil('\n'), "endloop");
					this.checkLine(this._stream.ReadUntil('\n'), "endfacet");

					line = this._stream.ReadUntil('\n');
				}
			}

			return mesh;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			this._stream.Dispose();
		}

		private bool checkStreamLenth(int nTriangles)
		{
			//Compare the length of the stream to check if is ascii file
			return _stream.Length == 84 + nTriangles * 50;
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
