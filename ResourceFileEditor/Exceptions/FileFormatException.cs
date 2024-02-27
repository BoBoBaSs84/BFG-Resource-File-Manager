using System;

namespace ResourceFileEditor.Exceptions;

/// <summary>
/// The <see cref="FileFormatException"/> class.
/// </summary>
/// <inheritdoc/>
public sealed class FileFormatException(string message, Exception innerException) : NotSupportedException(message, innerException)
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FileFormatException"/> class.
	/// </summary>
	/// <inheritdoc/>
	public FileFormatException(string message) : this(message, null)
	{ }
}
