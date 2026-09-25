namespace SunamoRoslyn.Data;

/// <summary>
/// Represents a parsed source file containing its syntax tree and compilation root.
/// </summary>
public class SourceFileTree
{
    /// <summary>
    /// Gets or sets the syntax tree of the source file.
    /// </summary>
    public SyntaxTree? Tree { get; set; }

    /// <summary>
    /// Gets or sets the compilation unit syntax root of the source file.
    /// </summary>
    public CompilationUnitSyntax? Root { get; set; }
}
