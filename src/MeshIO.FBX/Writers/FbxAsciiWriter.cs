﻿using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using MeshIO.FBX.Exceptions;

namespace MeshIO.FBX
{
	/// <summary>
	/// Writes an FBX document in a text format
	/// </summary>
	internal class FbxAsciiWriter : IDisposable
	{
		/// <summary>
		/// The maximum line length in characters when outputting arrays
		/// </summary>
		/// <remarks>
		/// Lines might end up being a few characters longer than this, visibly and otherwise,
		/// so don't rely on it as a hard limit in code!
		/// </remarks>
		public int MaxLineLength { get; set; } = 260;

		/// <summary>
		/// If this is active the max line length will not be applied
		/// </summary>
		public bool ApplyLineMaxLength { get; set; } 

		private readonly Stack<string> _nodePath = new Stack<string>();
		private readonly Stream _stream;

		/// <summary>
		/// Creates a new reader
		/// </summary>
		/// <param name="stream"></param>
		public FbxAsciiWriter(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			this._stream = stream;
		}

		/// <summary>
		/// Writes an FBX document to the stream
		/// </summary>
		/// <param name="document"></param>
		/// <remarks>
		/// ASCII FBX files have no header or footer, so you can call this multiple times
		/// </remarks>
		public void Write(FbxRootNode document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));
			var sb = new StringBuilder();

			// Write version header (a comment, but required for many importers)
			var vMajor = (int)document.Version / 1000;
			var vMinor = ((int)document.Version % 1000) / 100;
			var vRev = ((int)document.Version % 100) / 10;
			sb.Append($"; FBX {vMajor}.{vMinor}.{vRev} project file\n\n");

			_nodePath.Clear();
			foreach (var n in document.Nodes)
			{
				if (n == null)
					continue;
				buildString(n, sb, document.Version >= FbxVersion.v7100);
				sb.Append('\n');
			}
			var b = Encoding.ASCII.GetBytes(sb.ToString());
			_stream.Write(b, 0, b.Length);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_stream.Dispose();
		}

		// Adds the given node text to the string
		private void buildString(FbxNode node, StringBuilder sb, bool writeArrayLength, int indentLevel = 0)
		{
			_nodePath.Push(node.Name ?? "");
			int lineStart = sb.Length;
			// Write identifier
			for (int i = 0; i < indentLevel; i++)
				sb.Append('\t');
			sb.Append(node.Name).Append(':');

			// Write properties
			var first = true;
			for (int j = 0; j < node.Properties.Count; j++)
			{
				var p = node.Properties[j];
				if (p == null)
					continue;
				if (!first)
					sb.Append(',');
				sb.Append(' ');
				if (p is string)
				{
					sb.Append('"').Append(p).Append('"');
				}
				else if (p is Array)
				{
					var array = (Array)p;
					var elementType = p.GetType().GetElementType();
					// ReSharper disable once PossibleNullReferenceException
					// We know it's an array, so we don't need to check for null
					if (array.Rank != 1 || !elementType.IsPrimitive)
						throw new FbxException(_nodePath, j,
							"Invalid array type " + p.GetType());
					if (writeArrayLength)
					{
						sb.Append('*').Append(array.Length).Append(" {\n");
						lineStart = sb.Length;
						for (int i = -1; i < indentLevel; i++)
							sb.Append('\t');
						sb.Append("a: ");
					}
					bool pFirst = true;
					foreach (var v in (Array)p)
					{
						if (!pFirst)
							sb.Append(',');
						var vstr = v.ToString();

						if (this.ApplyLineMaxLength)
							if ((sb.Length - lineStart) + vstr.Length >= MaxLineLength)
							{
								sb.Append('\n');
								lineStart = sb.Length;
							}

						sb.Append(vstr);
						pFirst = false;
					}
					if (writeArrayLength)
					{
						sb.Append('\n');
						for (int i = 0; i < indentLevel; i++)
							sb.Append('\t');
						sb.Append('}');
					}
				}
				else if (p is char)
					sb.Append((char)p);
				else if (p.GetType().IsPrimitive && p is IFormattable)
					sb.Append(p);
				else
					throw new FbxException(_nodePath, j,
						"Invalid property type " + p.GetType());
				first = false;
			}

			// Write child nodes
			if (node.Nodes.Count > 0)
			{
				sb.Append(" {\n");
				foreach (var n in node.Nodes)
				{
					if (n == null)
						continue;
					buildString(n, sb, writeArrayLength, indentLevel + 1);
				}
				for (int i = 0; i < indentLevel; i++)
					sb.Append('\t');
				sb.Append('}');
			}
			sb.Append('\n');

			_nodePath.Pop();
		}
	}
}
