using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.CAD.IO.DWG
{
	internal class DwgObjectReader
	{
		private void getEntityType(IDwgStreamHandler shandler, ACadVersion version)
		{
			//RS : CRC for the data section, starting after the sentinel. Use 0xC0C1 for the initial value.
			CRC8StreamHandler crc = new CRC8StreamHandler(shandler.StreamToRead, 0xC0C1);

			//MS : Size of object, not including the CRC
			ushort size = (ushort)shandler.ReadModularChar();

			if (size <= 0U)
				throw new Exception();

			if(version >= ACadVersion.AC1024)
			{

			}
		}
	}
}
