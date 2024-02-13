using CSMath;
using System.Collections.Generic;

namespace MeshIO.OBJ
{
	internal class ObjData
	{
		public ObjTemplate Current { get; private set; }

		public ObjTemplate Placeholder { get; private set; }

		public List<ObjTemplate> Templates { get; private set; } = [];

		public ObjData()
		{
			Placeholder = new ObjTemplate(string.Empty);
		}

		public void CreateIndexer(string line)
		{
			this.MoveNext();

			this.Current = new ObjTemplate(line);
		}

		public void MoveNext()
		{
			if (this.Current == null)
			{
				return;
			}

			this.Current.Vertices.AddRange(this.Placeholder.Vertices);
			this.Current.Normals.AddRange(this.Placeholder.Normals);
			this.Current.UVs.AddRange(this.Placeholder.UVs);

			this.Current.MeshPolygons.AddRange(this.Placeholder.MeshPolygons);
			this.Current.TexturePolygons.AddRange(this.Placeholder.TexturePolygons);
			this.Current.NormalPolygons.AddRange(this.Placeholder.NormalPolygons);

			this.Templates.Add(this.Current);

			this.Placeholder.Vertices.Clear();
			this.Placeholder.Normals.Clear();
			this.Placeholder.UVs.Clear();

			this.Placeholder.MeshPolygons.Clear();
			this.Placeholder.TexturePolygons.Clear();
			this.Placeholder.NormalPolygons.Clear();
		}
	}
}
