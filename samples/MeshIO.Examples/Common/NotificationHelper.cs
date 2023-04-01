using MeshIO.Core;
using System;

namespace MeshIO.Examples.Common
{
	public class NotificationHelper
	{
		public static void LogConsoleNotification(object sender, NotificationEventArgs e)
		{
			switch (e.NotificationType)
			{
				case NotificationType.NotImplemented:
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case NotificationType.Information:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case NotificationType.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case NotificationType.Error:
				case NotificationType.NotSupported:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				default:
					break;
			}

			//Write in the console all the messages
			Console.WriteLine(e.Message);
		}
	}
}
