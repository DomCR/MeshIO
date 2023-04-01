using MeshIO.Examples.Common;
using MeshIO.FBX;

namespace MeshIO.Examples.Fbx
{
	public static class FbxReaderExample
	{
		/// <summary>
		/// Get the fbx version of a file
		/// </summary>
		/// <param name="file">fbx file path</param>
		public static void GetVersion(string file)
		{
			using (FbxReader reader = new FbxReader(file, ErrorLevel.Checked))
			{
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				FbxVersion version = reader.GetVersion();
			}
		}

		/// <summary>
		/// Parse a fbx file into a node tree structure
		/// </summary>
		/// <param name="file">fbx file path</param>
		public static void ParseFbx(string file)
		{
			using (FbxReader reader = new FbxReader(file, ErrorLevel.Checked))
			{
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				FbxRootNode node = reader.Parse();
			}
		}

		/// <summary>
		/// Read a fbx file
		/// </summary>
		/// <param name="file">fbx file path</param>
		public static void ReadFbx(string file)
		{
			using (FbxReader reader = new FbxReader(file, ErrorLevel.Checked))
			{
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				Scene scene = reader.Read();
			}
		}
	}
}
