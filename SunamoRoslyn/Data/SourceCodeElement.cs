namespace SunamoRoslyn.Data;

/// <summary>
/// Represents a source code element with a file reference and element type.
/// </summary>
public class SourceCodeElement
{
    /// <summary>
    /// Gets or sets the file index associated with this source code element.
    /// </summary>
    public int File { get; set; } = 0;

    /// <summary>
    /// Gets or sets the namespace code element type.
    /// </summary>
    public NamespaceCodeElementsType Type { get; set; }
}
