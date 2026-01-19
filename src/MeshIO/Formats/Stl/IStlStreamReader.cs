using MeshIO.Entities.Geometries;
using System.Collections.Generic;

namespace MeshIO.Formats.Stl
{
	internal interface IStlStreamReader
	{
		public event NotificationEventHandler OnNotification;

		public IEnumerable<Mesh> Read();
	}
}
