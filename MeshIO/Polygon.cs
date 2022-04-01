using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	//TODO: Polygon classes need to be in the geometry namespace
	public abstract class Polygon : IEnumerable<int>
	{
		public abstract int[] ToArray();

		public IEnumerator GetEnumerator()
		{
			return this.ToArray().GetEnumerator();
		}

		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			return (IEnumerator<int>)this.ToArray().GetEnumerator();
		}
	}

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
			this.Index0 = i0;
			this.Index1 = i1;
			this.Index2 = i2;
		}

		public override int[] ToArray()
		{
			return new int[] { this.Index0, this.Index1, this.Index2 };
		}
	}

	public class Quad : Polygon
	{
		public int Index0 { get; set; }
		public int Index1 { get; set; }
		public int Index2 { get; set; }
		public int Index3 { get; set; }

		public Quad(int i0, int i1, int i2, int i3)
		{
			this.Index0 = i0;
			this.Index1 = i1;
			this.Index2 = i2;
			this.Index3 = i3;
		}

		public override int[] ToArray()
		{
			return new int[] { this.Index0, this.Index1, this.Index2, this.Index3 };
		}
	}
}
