namespace MeshIO.Entities.Geometries
{
	public class Triangle : Polygon
	{
		public int Index0 { get; set; }
		public int Index1 { get; set; }
		public int Index2 { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Triangle() { }

		/// <summary>
		/// Setup a triangle with the 3 indexes.
		/// </summary>
		/// <param name="i0"></param>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		public Triangle(int i0, int i1, int i2)
		{
			Index0 = i0;
			Index1 = i1;
			Index2 = i2;
		}

		public Triangle(uint i0, uint i1, uint i2)
		{
			Index0 = (int)i0;
			Index1 = (int)i1;
			Index2 = (int)i2;
		}

		public override int[] ToArray()
		{
			return new int[] { Index0, Index1, Index2 };
		}
	}
}
