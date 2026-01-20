namespace MeshIO.Formats;

/// <summary>
/// Specifies the format of content for data processing or transmission.
/// </summary>
/// <remarks>Use this enumeration to indicate whether content should be treated as binary data or as ASCII-encoded
/// text. The selected value may affect how data is read, written, or interpreted by consuming components.</remarks>
public enum ContentType
{
	/// <summary>
	/// Specifies that the data is encoded in binary format.
	/// </summary>
	Binary,
	/// <summary>
	/// Gets an encoding for the ASCII character set.
	/// </summary>
	ASCII,
}
