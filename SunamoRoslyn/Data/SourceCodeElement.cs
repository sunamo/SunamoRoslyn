namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the source code element type.
/// </summary>
public class SourceCodeElement
{
    /// <summary>
    /// Gets or sets the file.
    /// </summary>
    public int File { get; set; } = 0;

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    public NamespaceCodeElementsType Type { get; set; }
}
