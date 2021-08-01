using System;

namespace MeshIO.FBX
{
	internal static class Utils
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
