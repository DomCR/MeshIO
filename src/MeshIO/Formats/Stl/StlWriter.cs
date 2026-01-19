using MeshIO.Entities.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Stl
{
	internal interface IStlStreamWriter
	{
		public event NotificationEventHandler OnNotification;

		public void Write();
	}

	public class StlWriter : SceneWriter<StlWriterOptions>
	{
		private static System.Globalization.NumberFormatInfo _nfi;

		static StlWriter()
		{
			_nfi = new();
			_nfi.NumberDecimalSeparator = ".";
		}

		public StlWriter(string path, Scene scene, StlWriterOptions options = null, NotificationEventHandler notification = null)
			: base(path, scene, options, notification) { }

		public StlWriter(Stream stream, Scene scene, StlWriterOptions options = null, NotificationEventHandler notification = null)
			: base(stream, scene, options, notification)
		{
		}

		public override void Write()
		{
			IStlStreamWriter writer = null;
			switch (this.Options.ContentType)
			{
				case ContentType.Binary:
					writer = new StlBinaryStreamWriter(_stream, _scene);
					break;
				case ContentType.ASCII:
					writer = new StlTextStreamWriter(_stream, _scene, Options);
					break;
			}

			writer.OnNotification += this.onNotificationEvent;
			writer.Write();
		}
	}
}