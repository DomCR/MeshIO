using MeshIO.Examples.Common;
using MeshIO.FBX;

namespace MeshIO.Examples.Fbx
{
	public static class FbxReaderExample
	{
		/// <summary>
		/// Read a fbx file
		/// </summary>
		/// <param name="file">fbx file path</param>
		public static void ReadFbx(string file)
		{
			using (FbxReader reader = new FbxReader(file))
			{
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				Scene scene = reader.Read();
			}
		}
	}
}
