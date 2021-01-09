using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using MeshIO.Exceptions;

namespace MeshIO.FBX.IO
{
	/// <summary>
	/// Reads FBX nodes from a text stream.
	/// </summary>
	internal class FbxASCIIReader : IDisposable
	{
		private enum ArrayType
		{
			Byte = 0,
			Int = 1,
			Long = 2,
			Float = 3,
			Double = 4,
		};

		/// <summary>
		/// The maximum array size that will be allocated.
		/// </summary>
		/// <remarks>
		/// If you trust the source, you can expand this value as necessary.
		/// Malformed files could cause large amounts of memory to be allocated
		/// and slow or crash the system as a result.
		/// </remarks>
		public int MaxArrayLength { get; set; } = (1 << 24);

		// We read bytes a lot, so we should make a more efficient method here
		// (The normal one makes a new byte array each time)
		private readonly byte[] m_singleChar = new byte[1];
		private char? m_prevChar;
		private bool m_endStream;
		private bool m_wasCr;
		private readonly Stream m_stream;
		private readonly ErrorLevel m_errorLevel;
		private int m_line = 1;
		private int m_column = 1;
		private object m_prevTokenSingle;
		private object m_prevToken;

		/// <summary>
		/// Creates a new reader
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="errorLevel"></param>
		public FbxASCIIReader(Stream stream, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			this.m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
			this.m_errorLevel = errorLevel;
		}
		/// <summary>
		/// Reads a full document from the stream
		/// </summary>
		/// <returns>The complete document object.</returns>
		public FbxRoot Read()
		{
			FbxRoot doc = new FbxRoot();

			// Read version string
			const string versionString = @"; FBX (\d)\.(\d)\.(\d) project file";

			// Skip whitespace
			char c;
			while (char.IsWhiteSpace(c = readChar()) && !m_endStream) { }
			bool hasVersionString = false;

			//Read the version string comment if exist
			if (c == ';')
			{
				var sb = new StringBuilder();

				do
				{
					sb.Append(c);
				} while (!isLineEnd(c = readChar()) && !m_endStream);

				Match match = Regex.Match(sb.ToString(), versionString);
				hasVersionString = match.Success;

				if (hasVersionString)
					doc.Version = (FbxVersion)(
						int.Parse(match.Groups[1].Value) * 1000 +
						int.Parse(match.Groups[2].Value) * 100 +
						int.Parse(match.Groups[3].Value) * 10
					);
			}

			//Check the version string 
			if (!hasVersionString && m_errorLevel >= ErrorLevel.Strict)
				throw new FbxException(m_line, m_column, "Invalid version string; first line must match \"" + versionString + "\"");

			//Add the child nodes
			FbxNode node;
			while ((node = ReadNode()) != null)
			{
				doc.Nodes.Add(node);
			}

			return doc;
		}
		/// <summary>
		/// Reads the next node from the stream
		/// </summary>
		/// <returns>The read node, or <c>null</c></returns>
		public FbxNode ReadNode()
		{
			var first = readToken();
			var id = first as Identifier;
			if (id == null)
			{
				if (first is EndOfStream)
					return null;

				throw new FbxException(m_line, m_column, "Unexpected '" + first + "', expected an identifier");
			}
			var node = new FbxNode { Name = id.String };

			// Read properties
			object token;
			bool expectComma = false;
			while (!'{'.Equals(token = readToken()) && !(token is Identifier) && !'}'.Equals(token))
			{
				if (expectComma)
				{
					if (!','.Equals(token))
						throw new FbxException(m_line, m_column,
							"Unexpected '" + token + "', expected a ','");
					expectComma = false;
					continue;
				}
				if (token is char)
				{
					var c = (char)token;
					switch (c)
					{
						case '*':
							token = readArray();
							break;
						case '}':
						case ':':
						case ',':
							throw new FbxException(m_line, m_column,
								"Unexpected '" + c + "' in property list");
					}
				}
				node.Properties.Add(token);
				expectComma = true; // The final comma before the open brace isn't required
			}
			// TODO: Merge property list into an array as necessary
			// Now we're either at an open brace, close brace or a new node
			if (token is Identifier || '}'.Equals(token))
			{
				m_prevToken = token;
				return node;
			}
			// The while loop can't end unless we're at an open brace, so we can continue right on
			object endBrace;
			while (!'}'.Equals(endBrace = readToken()))
			{
				m_prevToken = endBrace; // If it's not an end brace, the next node will need it
				node.Nodes.Add(ReadNode());
			}
			if (node.Nodes.Count < 1) // If there's an open brace, we want that to be preserved
				node.Nodes.Add(null);
			return node;
		}
		/// <inheritdoc/>
		public void Dispose()
		{
			m_stream.Dispose();
		}
		//**************************************************************************************
		/// <summary>
		/// Reads a char, allows peeking and checks for end of stream.
		/// </summary>
		/// <returns></returns>
		private char readChar()
		{
			if (m_prevChar != null)
			{
				char c = m_prevChar.Value;
				m_prevChar = null;
				return c;
			}
			if (m_stream.Read(m_singleChar, 0, 1) < 1)
			{
				m_endStream = true;
				return '\0';
			}
			var ch = (char)m_singleChar[0];
			// Handle line and column numbers here;
			// This isn't terribly accurate, but good enough for diagnostics
			if (ch == '\r')
			{
				m_wasCr = true;
				m_line++;
				m_column = 0;
			}
			else
			{
				if (ch == '\n' && !m_wasCr)
				{
					m_line++;
					m_column = 0;
				}
				m_wasCr = false;
			}
			m_column++;
			return ch;
		}
		/// <summary>
		/// Checks if a character is valid in a real number
		/// </summary>
		/// <param name="c"></param>
		/// <param name="first"></param>
		/// <returns></returns>
		private bool isDigit(char c, bool first)
		{
			if (char.IsDigit(c))
				return true;
			switch (c)
			{
				case '-':
				case '+':
					return true;
				case '.':
				case 'e':
				case 'E':
				case 'X':
				case 'x':
					return !first;
			}
			return false;
		}
		private bool isLineEnd(char c)
		{
			return c == '\r' || c == '\n';
		}

		// Token to mark the end of the stream
		[Obsolete]
		class EndOfStream
		{
			public override string ToString()
			{
				return "end of stream";
			}
		}

		// Wrapper around a string to mark it as an identifier
		// (as opposed to a string literal)
		[Obsolete]
		class Identifier
		{
			public readonly string String;

			public override bool Equals(object obj)
			{
				var id = obj as Identifier;
				if (id != null)
					return String == id.String;
				return false;
			}

			public override int GetHashCode()
			{
				return String?.GetHashCode() ?? 0;
			}

			public Identifier(string str)
			{
				String = str;
			}

			public override string ToString()
			{
				return String + ":";
			}
		}

		// Reads a single token, allows peeking
		// Can return 'null' for a comment or whitespace
		private object readTokenSingle()
		{
			if (m_prevTokenSingle != null)
			{
				var ret = m_prevTokenSingle;
				m_prevTokenSingle = null;
				return ret;
			}
			var c = readChar();
			if (m_endStream)
				return new EndOfStream();
			switch (c)
			{
				case ';': // Comments
					while (!isLineEnd(readChar()) && !m_endStream) { } // Skip a line
					return null;
				case '{': // Operators
				case '}':
				case '*':
				case ':':
				case ',':
					return c;
				case '"': // String literal
					var sb1 = new StringBuilder();
					while ((c = readChar()) != '"')
					{
						if (m_endStream)
							throw new FbxException(m_line, m_column,
								"Unexpected end of stream; expecting end quote");
						sb1.Append(c);
					}
					return sb1.ToString();
				default:
					if (char.IsWhiteSpace(c))
					{
						// Merge whitespace
						while (char.IsWhiteSpace(c = readChar()) && !m_endStream) { }
						if (!m_endStream)
							m_prevChar = c;
						return null;
					}
					if (isDigit(c, true)) // Number
					{
						var sb2 = new StringBuilder();
						do
						{
							sb2.Append(c);
							c = readChar();
						} while (isDigit(c, false) && !m_endStream);
						if (!m_endStream)
							m_prevChar = c;
						var str = sb2.ToString();
						if (str.Contains("."))
						{
							if (str.Split('.', 'e', 'E')[1].Length > 6)
							{
								double d;
								if (!double.TryParse(str, out d))
									throw new FbxException(m_line, m_column,
										"Invalid number");
								return d;
							}
							else
							{
								float f;
								if (!float.TryParse(str, out f))
									throw new FbxException(m_line, m_column,
										"Invalid number");
								return f;
							}
						}
						long l;
						if (!long.TryParse(str, out l))
							throw new FbxException(m_line, m_column,
								"Invalid integer");
						// Check size and return the smallest possible
						if (l >= byte.MinValue && l <= byte.MaxValue)
							return (byte)l;
						if (l >= int.MinValue && l <= int.MaxValue)
							return (int)l;
						return l;
					}
					if (char.IsLetter(c) || c == '_') // Identifier
					{
						var sb3 = new StringBuilder();
						do
						{
							sb3.Append(c);
							c = readChar();
						} while ((char.IsLetterOrDigit(c) || c == '_') && !m_endStream);
						if (!m_endStream)
							m_prevChar = c;
						return new Identifier(sb3.ToString());
					}
					break;
			}
			throw new FbxException(m_line, m_column,
				"Unknown character " + c);
		}

		// Use a loop rather than recursion to prevent stack overflow
		// Here we can also merge string+colon into an identifier,
		// returning single-character bare strings (for C-type properties)
		private object readToken()
		{
			object ret;
			if (m_prevToken != null)
			{
				ret = m_prevToken;
				m_prevToken = null;
				return ret;
			}
			do
			{
				ret = readTokenSingle();
			} while (ret == null);

			Identifier id = ret as Identifier;

			if (id != null)
			{
				object colon;
				do
				{
					colon = readTokenSingle();
				} while (colon == null);
				if (!':'.Equals(colon))
				{
					if (id.String.Length > 1)
						throw new FbxException(m_line, m_column,
							"Unexpected '" + colon + "', expected ':' or a single-char literal");
					ret = id.String[0];
					m_prevTokenSingle = colon;
				}
			}
			return ret;
		}

		private void expectToken(object token)
		{
			var t = readToken();
			if (!token.Equals(t))
				throw new FbxException(m_line, m_column, "Unexpected '" + t + "', expected " + token);
		}

		private Array readArray()
		{
			// Read array length and header
			var len = readToken();
			long l;

			if (len is long)
				l = (long)len;
			else if (len is int)
				l = (int)len;
			else if (len is byte)
				l = (byte)len;
			else
				throw new FbxException(m_line, m_column, "Unexpected '" + len + "', expected an integer");
			if (l < 0)
				throw new FbxException(m_line, m_column, "Invalid array length " + l);
			if (l > MaxArrayLength)
				throw new FbxException(m_line, m_column, "Array length " + l + " higher than permitted maximum " + MaxArrayLength);

			expectToken('{');
			expectToken(new Identifier("a"));
			var array = new double[l];

			// Read array elements
			bool expectComma = false;
			object token;
			var arrayType = ArrayType.Byte;
			long pos = 0;
			while (!'}'.Equals(token = readToken()))
			{
				if (expectComma)
				{
					if (!','.Equals(token))
						throw new FbxException(m_line, m_column, "Unexpected '" + token + "', expected ','");

					expectComma = false;
					continue;
				}
				if (pos >= array.Length)
				{
					if (m_errorLevel >= ErrorLevel.Checked)
						throw new FbxException(m_line, m_column, "Too many elements in array");
					continue;
				}

				// Add element to the array, checking for the maximum
				// size of any one element.
				// (I'm not sure if this is the 'correct' way to do it, but it's the only
				// logical one given the nature of the ASCII format)
				double d;
				if (token is byte)
				{
					d = (byte)token;
				}
				else if (token is int)
				{
					d = (int)token;
					if (arrayType < ArrayType.Int)
						arrayType = ArrayType.Int;
				}
				else if (token is long)
				{
					d = (long)token;
					if (arrayType < ArrayType.Long)
						arrayType = ArrayType.Long;
				}
				else if (token is float)
				{
					d = (float)token;
					// A long can't be accurately represented by a float
					arrayType = arrayType < ArrayType.Long ? ArrayType.Float : ArrayType.Double;
				}
				else if (token is double)
				{
					d = (double)token;
					if (arrayType < ArrayType.Double)
						arrayType = ArrayType.Double;
				}
				else
					throw new FbxException(m_line, m_column, "Unexpected '" + token + "', expected a number");

				array[pos++] = d;
				expectComma = true;
			}

			if (pos < array.Length && m_errorLevel >= ErrorLevel.Checked)
				throw new FbxException(m_line, m_column, "Too few elements in array - expected " + (array.Length - pos) + " more");

			// Convert the array to the smallest type we can see
			Array ret;
			switch (arrayType)
			{
				case ArrayType.Byte:
					var bArray = new byte[array.Length];
					for (int i = 0; i < bArray.Length; i++)
						bArray[i] = (byte)array[i];
					ret = bArray;
					break;
				case ArrayType.Int:
					var iArray = new int[array.Length];
					for (int i = 0; i < iArray.Length; i++)
						iArray[i] = (int)array[i];
					ret = iArray;
					break;
				case ArrayType.Long:
					var lArray = new long[array.Length];
					for (int i = 0; i < lArray.Length; i++)
						lArray[i] = (long)array[i];
					ret = lArray;
					break;
				case ArrayType.Float:
					var fArray = new float[array.Length];
					for (int i = 0; i < fArray.Length; i++)
						fArray[i] = (long)array[i];
					ret = fArray;
					break;
				default:
					ret = array;
					break;
			}
			return ret;
		}
	}
}
