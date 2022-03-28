using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO
{
	//TODO: Polygon classes need to be in the geometry namespace
	public abstract class Polygon : IEnumerable
	{
		public abstract int[] ToArray();

		public IEnumerator GetEnumerator()
		{
			return this.ToArray().GetEnumerator();
		}
	}

	public class Triangle : Polygon
	{
		public uint Index0 { get; set; }
		public uint Index1 { get; set; }
		public uint Index2 { get; set; }

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
		public Triangle(uint i0, uint i1, uint i2)
		{
			this.Index0 = i0;
			this.Index1 = i1;
			this.Index2 = i2;
		}

		public override int[] ToArray()
		{
			return new int[] { (int)this.Index0, (int)this.Index1, (int)this.Index2 };
		}
	}

	public class Quad : Polygon
	{
		public uint Index0 { get; set; }
		public uint Index1 { get; set; }
		public uint Index2 { get; set; }
		public uint Index3 { get; set; }

		public Quad(uint i0, uint i1, uint i2, uint i3)
		{
			this.Index0 = i0;
			this.Index1 = i1;
			this.Index2 = i2;
			this.Index3 = i3;
		}

		public override int[] ToArray()
		{
			return new int[] { (int)this.Index0, (int)this.Index1, (int)this.Index2, (int)this.Index3 };
		}
	}
}
