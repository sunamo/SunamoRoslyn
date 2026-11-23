namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the source file tree type.
/// </summary>
public class SourceFileTree
{
    /// <summary>
    /// Gets or sets the tree.
    /// </summary>
    public SyntaxTree? Tree { get; set; }

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public CompilationUnitSyntax? Root { get; set; }
}
