using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Core
{
	public delegate void NotificationHandler(NotificationArgs e);

	public class NotificationArgs 
	{
		public string Message { get; }

		public NotificationArgs(string message)
		{
			this.Message = message;
		}
	}

	public class ReaderBase
	{
		public NotificationHandler OnNotification { get; set; }
	}
}
