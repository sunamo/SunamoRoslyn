namespace SunamoRoslyn.Roslyns;

/// <summary>
/// Represents the process file bool result type.
/// </summary>
public class ProcessFileBoolResult
{
    /// <summary>
    /// Initializes a new instance of the process file bool result class.
    /// </summary>
    public ProcessFileBoolResult()
    {

    }

    /// <summary>
    /// Gets or sets the indexed.
    /// </summary>
    public bool Indexed { get; set; } = false;

    /// <summary>
    /// Gets or sets the tree.
    /// </summary>
    public SyntaxTree? Tree { get; set; }

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public CompilationUnitSyntax? Root { get; set; }
}
