using System;

namespace MeshIO
{
	[Obsolete]
	internal static class Utils
	{
		///<remarks>
		///Avoid duplicated ids in the tight loops.
		///</remarks>
		private static readonly Random _random = new Random();

		private static readonly object _syncLock = new object();

		/// <summary>
		/// Creates an id as a long.
		/// </summary>
		/// <returns></returns>
		public static long CreateId()
		{
			lock (_syncLock)
			{
				byte[] buffer = new byte[8];
				_random.NextBytes(buffer);
				return Math.Abs(BitConverter.ToInt64(buffer, 0));
			}
		}
	}
}
