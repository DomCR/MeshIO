using MeshIO.Entities.Geometries;
using System;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Stl
{
	internal class StlBinaryStreamWriter : IStlStreamWriter
	{
		public event NotificationEventHandler OnNotification;

		private readonly Scene _scene;

		private readonly StreamWriter _stream;

		public StlBinaryStreamWriter(Stream stream, Scene scene)
		{
			this._stream = new StreamWriter(stream, new UTF8Encoding(false));
			this._scene = scene;
		}

		public void Write()
		{
			Mesh mesh = this.mergeMeshes();



			throw new NotImplementedException();
		}

		private Mesh mergeMeshes()
		{
			throw new NotImplementedException();
		}
	}
}