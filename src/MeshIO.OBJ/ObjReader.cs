using MeshIO.Core;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MeshIO.OBJ
{
	public class ObjReader : ReaderBase
	{
		private readonly Regex _matchNoneWhiteSpaces;
		private readonly StreamReader _reader;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read from</param>
		public ObjReader(string path) : this(File.OpenRead(path)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		public ObjReader(Stream stream) : base(stream)
		{
			_reader = new StreamReader(stream);
			_matchNoneWhiteSpaces = new Regex(@"\s+", RegexOptions.Compiled);
		}

		/// <summary>
		/// Read the Obj file
		/// </summary>
		public override Scene Read()
		{
			Scene scene = new Scene();

			while (!_reader.EndOfStream)
			{
				string line = _reader.ReadLine();
				if (!this.processLine(line, out string token, out string values))
				{
					continue;
				}

				switch (token)
				{
					default:
						this.triggerNotification($"[{nameof(ObjReader)}] Unknown token: {token}", NotificationType.Warning);
						break;
				}
			}

			return scene;
		}

		private bool processLine(string line, out string token, out string values)
		{
			token = string.Empty;
			values = string.Empty;
			if (line == null)
			{
				return false;
			}

			line = _matchNoneWhiteSpaces.Replace(line, " ").Trim();

			if (this.isComment(line))
			{
				return false;
			}

			int indexOfSpace = line.IndexOf(' ');
			if (indexOfSpace == -1)
			{
				token = line;
			}
			else
			{
				token = line.Substring(0, indexOfSpace);
				values = line.Substring(indexOfSpace + 1);
			}
			return true;
		}

		private bool isComment(string line)
		{
			return line.StartsWith("#");
		}
	}
}
