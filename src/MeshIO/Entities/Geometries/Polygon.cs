using System.Collections;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries
{
    public abstract class Polygon : IEnumerable<int>
    {
        public abstract int[] ToArray();

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
