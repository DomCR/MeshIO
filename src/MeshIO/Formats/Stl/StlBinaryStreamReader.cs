using MeshIO.Entities.Geometries;
using System.Collections.Generic;
using System.IO;

namespace MeshIO.Formats.Stl
{
	internal class StlBinaryStreamReader: IStlStreamReader
	{
		public StlBinaryStreamReader(Stream stream)
		{
		}

		public event NotificationEventHandler OnNotification;

		public IEnumerable<Mesh> Read()
		{
			throw new System.NotImplementedException();
		}
	}
}
