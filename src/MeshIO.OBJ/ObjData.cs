using CSMath;
using System.Collections.Generic;

namespace MeshIO.OBJ
{
	internal class ObjData
	{
		public ObjTemplate Current { get; private set; }

		public List<XYZM> Vertices { get; } = [];

		public List<XYZ> Normals { get; } = [];

		public List<XYZ> UVs { get; } = [];

		public List<ObjTemplate> Templates { get; private set; } = [];

		public ObjData() { }

		public void CreateIndexer(string line)
		{
			this.MoveNext();

			this.Current = new ObjTemplate(line);
		}

		public void MoveNext()
		{
			if (this.Current != null)
			{
				this.Current.Vertices.AddRange(this.Vertices);
				this.Current.Normals.AddRange(this.Normals);
				this.Current.UVs.AddRange(this.UVs);

				this.Templates.Add(this.Current);

				this.Vertices.Clear();
				this.Normals.Clear();
				this.UVs.Clear();
			}
		}
	}
}
