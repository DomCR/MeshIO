using CSMath;
using MeshIO.Core;
using MeshIO.Entities.Geometries;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.OBJ
{
	public class ObjReader : ReaderBase
	{
		private readonly Regex _matchNoneWhiteSpaces;
		private readonly StreamReader _reader;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read from</param>
		public ObjReader(string path) : this(File.OpenRead(path)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		public ObjReader(Stream stream) : base(stream)
		{
			_reader = new StreamReader(stream);
			_matchNoneWhiteSpaces = new Regex(@"\s+", RegexOptions.Compiled);
		}

		/// <summary>
		/// Read the Obj file
		/// </summary>
		public override Scene Read()
		{
			ObjData data = new ObjData();
			Scene scene = new Scene();

			while (!_reader.EndOfStream)
			{
				string line = _reader.ReadLine();
				if (!this.processLine(line, out ObjFileToken token, out string values))
				{
					continue;
				}

				switch (token)
				{
					case ObjFileToken.Object:
						data.CreateIndexer(values);
						break;
					case ObjFileToken.Vertice:
						data.Vertices.Add(this.parseVertex(values));
						break;
					case ObjFileToken.Normal:
						data.Normals.Add(this.parseNormal(values));
						break;
					case ObjFileToken.TextureVertice:
						data.UVs.Add(this.parse<XYZ>(values));
						break;
					case ObjFileToken.Face:
						break;
				}
			}

			data.MoveNext();
			this.processData(data, scene);

			return scene;
		}

		private void processData(ObjData data, Scene scene)
		{
			foreach (ObjTemplate item in data.Templates)
			{
				Mesh mesh = item.CreateMesh();
				Node node = new Node(item.Name);
				node.Entities.Add(mesh);

				scene.RootNode.Nodes.Add(node);
			}
		}

		private T parse<T>(string line)
			where T : IVector, new()
		{
			T v = new T();
			string[] arr = (string[])line.Split(' ');

			int i;
			for (i = 0; i < arr.Length; i++)
			{
				v[i] = double.Parse(arr[i]);
			}

			if (arr.Length < v.Dimension)
			{
				v[i] = 1.0d;
			}

			return v;
		}

		private XYZM parseVertex(string line)
		{
			XYZM v = new XYZM();
			string[] arr = (string[])line.Split(' ');

			v.X = double.Parse(arr[0]);
			v.Y = double.Parse(arr[1]);
			v.Z = double.Parse(arr[2]);

			if (arr.Length == 4)
			{
				v.M = double.Parse(arr[3]);
			}
			else
			{
				v.M = 1.0d;
			}

			return v;
		}

		private XYZ parseNormal(string line)
		{
			XYZ v = new XYZ();
			string[] arr = (string[])line.Split(' ');

			v.X = double.Parse(arr[0]);
			v.Y = double.Parse(arr[1]);
			v.Z = double.Parse(arr[2]);

			return v;
		}

		private bool processLine(string line, out ObjFileToken token, out string values)
		{
			token = ObjFileToken.Undefined;
			values = string.Empty;
			if (line == null)
			{
				return false;
			}

			line = _matchNoneWhiteSpaces.Replace(line, " ").Trim();

			if (this.isComment(line))
			{
				return false;
			}

			string strToken = string.Empty;
			int indexOfSpace = line.IndexOf(' ');
			if (indexOfSpace == -1)
			{
				strToken = line;
			}
			else
			{
				strToken = line.Substring(0, indexOfSpace);
				values = line.Substring(indexOfSpace + 1);
			}

			if (!ObjFileParser.ParseToken(strToken, out token))
			{
				this.triggerNotification($"[{nameof(ObjReader)}] Unknown token: {token}", NotificationType.Warning);
				return false;
			}

			return true;
		}

		private bool isComment(string line)
		{
			return line.StartsWith("#");
		}
	}
}
