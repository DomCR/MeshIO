using MeshIO.Core;
using System;

namespace MeshIO.FBX.Converters.Mappers
{
	[Obsolete]
	public interface IFbxMapper
	{
		public event NotificationEventHandler OnNotification;

		string SectionName { get; }

		/// <summary>
		/// Map a <see cref="FbxNode"/> into a mapper
		/// </summary>
		/// <param name="node"></param>
		void Map(FbxNode node);
	}
}
