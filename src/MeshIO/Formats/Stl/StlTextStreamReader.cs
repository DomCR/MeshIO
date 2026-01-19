using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.IO;

namespace MeshIO.Formats.Stl;

internal class StlTextStreamReader : IStlStreamReader
{
	public event NotificationEventHandler OnNotification;

	private readonly StreamReader _reader;

	public StlTextStreamReader(Stream stream)
	{
		this._reader = new StreamReader(stream);
	}

	public IEnumerable<Mesh> Read()
	{
		List<Mesh> meshes = new List<Mesh>();

		Mesh mesh = null;
		XYZ normal = XYZ.NaN;
		LayerElementNormal normals = new(MappingMode.ByPolygon, ReferenceMode.Direct);
		List<XYZ> loop = new();
		while (!_reader.EndOfStream)
		{
			if (!processArgs(_reader.ReadLine(), out var keyword, out var value))
			{
				continue;
			}

			switch (keyword.ToLower())
			{
				case StlFileToken.Solid:
					mesh = new Mesh(value);
					break;
				case StlFileToken.Facet:
					processArgs(value, out _, out value);
					normals.Add(this.parseXYZ(value));
					break;
				case StlFileToken.EndFacet:
					//Should do something?
					break;
				case StlFileToken.Vertex:
					XYZ v = this.parseXYZ(value);
					loop.Add(v);
					break;
				case StlFileToken.Outer when value.Equals(StlFileToken.Loop):
					loop.Clear();
					break;
				case StlFileToken.EndLoop:
					if (loop.Count < 3)
					{
						continue;
					}
					mesh.AddPolygons(loop);
					break;
				case StlFileToken.EndSolid:
					mesh.Layers.Add(normals);
					meshes.Add(mesh);
					break;
				default:
					this.OnNotification.Invoke(this, new NotificationEventArgs($"Unknown keyword {keyword}.", NotificationType.Warning));
					break;
			}
		}

		return meshes;
	}

	private XYZ parseXYZ(string value)
	{
		var data = value.Split(' ');

		XYZ xyz = new XYZ();
		for (int i = 0; i < data.Length; i++)
		{
			xyz[i] = double.Parse(data[i]);
		}

		return xyz;
	}

	private bool processArgs(string line, out string token, out string value)
	{
		token = string.Empty;
		value = string.Empty;
		if (line == null)
		{
			return false;
		}
		line = line.Trim();
		int commentIndex = line.IndexOf(StlFileToken.Comment);
		if (commentIndex != -1)
		{
			line = line.Substring(0, commentIndex).TrimEnd();
		}
		if (string.IsNullOrEmpty(line))
		{
			return false;
		}
		int spaceIndex = line.IndexOf(' ');
		int tabIndex = line.IndexOf('\t');
		int splitIndex = (spaceIndex == -1) ? tabIndex
			: (tabIndex != -1 && spaceIndex > tabIndex ? tabIndex : spaceIndex);

		if (splitIndex == -1)
		{
			token = line;
		}
		else
		{
			token = line.Substring(0, splitIndex).Trim();
			value = line.Substring(splitIndex + 1).Trim();
		}
		return true;
	}
}