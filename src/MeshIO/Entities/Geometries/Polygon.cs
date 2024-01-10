using System.Collections;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries
{
	public abstract class Polygon : IEnumerable<int>
	{
		public abstract int[] ToArray();

		public static Polygon CreatePolygon(int[] arr)
		{
			throw new System.Exception();
		}

		public static IEnumerable<Polygon> CreatePolygons(int[] arr)
		{
			throw new System.Exception();
		}

		public IEnumerator GetEnumerator()
		{
			return ToArray().GetEnumerator();
		}

		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			return (IEnumerator<int>)ToArray().GetEnumerator();
		}
	}
}
