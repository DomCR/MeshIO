using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	internal static class NodeUtils
	{
		///<remarks>
		///Avoid duplicated ids in the tight loops.
		///</remarks>
		private static readonly Random m_random = new Random();
		private static readonly object m_syncLock = new object();
		/// <summary>
		/// Creates an id as a long.
		/// </summary>
		/// <returns></returns>
		public static ulong CreateId()
		{
			lock (m_syncLock)
			{
				byte[] buffer = new byte[8];
				m_random.NextBytes(buffer);
				return (ulong)Math.Abs(BitConverter.ToInt64(buffer, 0));
			}
		}
	}
}
