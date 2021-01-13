using MeshIO.CAD;
using MeshIO.CAD.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.Examples
{
	public static class CadExamples
	{
		private static string m_folder = "\\CadSamples\\dwg";
		private static Dictionary<ACadVersion, string> m_filePaths = new Dictionary<ACadVersion, string>
		{
			{ ACadVersion.AC1014, $"{m_folder}\\drawing_R14.dwg"},
			{ ACadVersion.AC1015, $"{m_folder}\\drawing_2000.dwg"},
			{ ACadVersion.AC1018, $"{m_folder}\\drawing_2004.dwg"},
			{ ACadVersion.AC1021, $"{m_folder}\\drawing_2007.dwg"},
			{ ACadVersion.AC1024, $"{m_folder}\\drawing_2010.dwg"},
			{ ACadVersion.AC1027, $"{m_folder}\\drawing_2013.dwg"},
			{ ACadVersion.AC1032, $"{m_folder}\\drawing_2018.dwg"},
		};
		public static void ReadHeader()
		{
			DwgReader reader = new DwgReader(Utils.GetFileDirectoryPath()
				+ m_filePaths[ACadVersion.AC1021]);
			DwgFileHeader fheader = reader.ReadFileHeader();
		}
		public static void ReadSummaryInfo()
		{
			DwgReader reader = new DwgReader(Utils.GetFileDirectoryPath()
				+ m_filePaths[ACadVersion.AC1032]);
			reader.ReadSummaryInfo();
		}
	}
}
