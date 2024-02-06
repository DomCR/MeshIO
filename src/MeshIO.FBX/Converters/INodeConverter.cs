using MeshIO.Core;
using System;

namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a <see cref="FbxRootNode"/> into a <see cref="Scene"/>
	/// </summary>
	[Obsolete]
	internal interface INodeConverter
	{
		public event NotificationEventHandler OnNotification;

		FbxVersion Version { get; }

		Scene ConvertScene();
	}
}
