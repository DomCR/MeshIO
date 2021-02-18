using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using MeshIO.Exceptions;

namespace MeshIO.FBX.IO
{
	/// <summary>
	/// Writes an FBX document in a text format
	/// </summary>
	public class FbxASCIIWriter
	{
		/// <summary>
		/// The maximum line length in characters when outputting arrays.
		/// </summary>
		/// <remarks>
		/// Lines might end up being a few characters longer than this, visibly and otherwise,
		/// so don't rely on it as a hard limit in code!
		/// </remarks>
		public int MaxLineLength { get; set; }

		private readonly Stack<string> nodePath = new Stack<string>();
		private readonly Stream m_stream;

		private FbxASCIIWriter()
		{
			MaxLineLength = 260;
		}
		/// <summary>
		/// Creates a new writer
		/// </summary>
		/// <param name="stream"></param>
		public FbxASCIIWriter(Stream stream) : this()
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			this.m_stream = stream;
		}
		//*****************************************************************************************
		/// <summary>
		/// Writes an FBX document to the stream
		/// </summary>
		/// <param name="document"></param>
		/// <remarks>
		/// ASCII FBX files have no header or footer, so you can call this multiple times
		/// </remarks>
		public void Write(FbxRoot document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			var sb = new StringBuilder();

			// Write version header (a comment, but required for many importers)
			var vMajor = (int)document.Version / 1000;
			var vMinor = ((int)document.Version % 1000) / 100;
			var vRev = ((int)document.Version % 100) / 10;

			sb.Append($"; FBX {vMajor}.{vMinor}.{vRev} project file\n\n");

			nodePath.Clear();

			foreach (FbxNode n in document.Nodes)
			{
				if (n == null)
					continue;

				//Clear the node path for each main node
				nodePath.Clear();

				buildString(n, sb, document.Version >= FbxVersion.v7100);

				sb.Append('\n');
			}

			var b = Encoding.ASCII.GetBytes(sb.ToString());
			m_stream.Write(b, 0, b.Length);
		}
		//*****************************************************************************************
		/// <summary>
		/// Adds the given node text to the string
		/// </summary>
		/// <param name="node"></param>
		/// <param name="sb"></param>
		/// <param name="writeArrayLength"></param>
		/// <param name="applyLineLimit"></param>
		/// <param name="indentLevel"></param>
		private void buildString(FbxNode node, StringBuilder sb, bool writeArrayLength, bool applyLineLimit = true, int indentLevel = 0)
		{
			nodePath.Push(node.Name ?? "");
			int lineStart = sb.Length;

			// Write class name
			for (int i = 0; i < indentLevel; i++)
			{
				sb.Append('\t');
			}

			sb.Append(node.Name).Append(':');

			// Write properties
			var first = true;
			for (int j = 0; j < node.Properties.Count; j++)
			{
				object prop = node.Properties[j];

				//Guard, ignore the null values
				if (prop == null)
					continue;

				//Write a comma if is not the first
				if (!first)
					sb.Append(',');

				sb.Append(' ');

				//Write a string between commas
				if (prop is string)
				{
					sb.Append('"').Append(prop).Append('"');
				}
				//Evaluate an array
				else if (prop is Array array)
				{
					Type elementType = prop.GetType().GetElementType();

					// We know it's an array, so we don't need to check for null
					if (array.Rank != 1 || !elementType.IsPrimitive)
						//Array cannot be 2d or a none primitive type
						throw new FbxException(nodePath, j, "Invalid array type " + prop.GetType());

					if (writeArrayLength)
					{
						sb.Append('*').Append(array.Length).Append(" {\n");

						lineStart = sb.Length;

						for (int i = -1; i < indentLevel; i++)
						{
							sb.Append('\t');
						}

						sb.Append("a: ");
					}

					bool pFirst = true;
					foreach (var v in (Array)prop)
					{
						if (!pFirst)
							sb.Append(',');
						var vstr = v.ToString();

						//Get rid of the line limit
						//if (applyLineLimit && (sb.Length - lineStart) + vstr.Length >= MaxLineLength)
						//{
						//	sb.Append('\n');
						//	lineStart = sb.Length;
						//}

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
				else if (prop is char)
					sb.Append((char)prop);
				else if (prop.GetType().IsPrimitive && prop is IFormattable)
					sb.Append(prop);
				else
					throw new FbxException(nodePath, j, "Invalid property type " + prop.GetType());

				first = false;
			}

			// Write child nodes
			if (node.Nodes.Count > 0)
			{
				sb.Append(" {\n");
				foreach (var n in node.Nodes)
				{
					//Guard: ignore null nodes
					if (n == null)
						continue;

					buildString(n, sb, writeArrayLength, applyLineLimit, indentLevel + 1);
				}

				//Tabulate the node
				//TODO: Check if it works
				//for (int k = 0; k < indentLevel; k++)
				sb.Append('\t', indentLevel);

				//Close the node
				sb.Append('}');
			}
			//Write the closing empty node if is a main node
			else if (nodePath.Count == 1)
			{
				sb.Append(" {\n");
				sb.Append('}');
			}

			sb.Append('\n');

			nodePath.Pop();
		}
	}
}
