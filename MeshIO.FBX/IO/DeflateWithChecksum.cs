using System.IO;
using System.IO.Compression;

namespace MeshIO
{
	/// <summary>
	/// A wrapper for DeflateStream that calculates the Adler32 checksum of the payload.
	/// </summary>
	public class DeflateWithChecksum : DeflateStream
	{
		private const int k_modAdler = 65521;
		private uint m_checksumA;
		private uint m_checksumB;

		/// <summary>
		/// Gets the Adler32 checksum at the current point in the stream.
		/// </summary>
		public int Checksum
		{
			get
			{
				m_checksumA %= k_modAdler;
				m_checksumB %= k_modAdler;
				return (int)((m_checksumB << 16) | m_checksumA);
			}
		}

		/// <inheritdoc />
		public DeflateWithChecksum(Stream stream, CompressionMode mode) : base(stream, mode)
		{
			ResetChecksum();
		}

		/// <inheritdoc />
		public DeflateWithChecksum(Stream stream, CompressionMode mode, bool leaveOpen) : base(stream, mode, leaveOpen)
		{
			ResetChecksum();
		}

		/// <summary>
		/// Initializes the checksum values.
		/// </summary>
		public void ResetChecksum()
		{
			m_checksumA = 1;
			m_checksumB = 0;
		}
		/// <inheritdoc />
		public override void Write(byte[] array, int offset, int count)
		{
			base.Write(array, offset, count);
			calcChecksum(array, offset, count);
		}
		/// <inheritdoc />
		public override int Read(byte[] array, int offset, int count)
		{
			var ret = base.Read(array, offset, count);
			calcChecksum(array, offset, count);
			return ret;
		}
		/// <summary> 
		/// Efficiently extends the checksum with the given buffer.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		void calcChecksum(byte[] array, int offset, int count)
		{
			m_checksumA %= k_modAdler;
			m_checksumB %= k_modAdler;
			for (int i = offset, c = 0; i < (offset + count); i++, c++)
			{
				m_checksumA += array[i];
				m_checksumB += m_checksumA;
				if (c > 4000) // This is about how many iterations it takes for B to reach IntMax
				{
					m_checksumA %= k_modAdler;
					m_checksumB %= k_modAdler;
					c = 0;
				}
			}
		}
	}
}
