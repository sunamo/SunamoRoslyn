namespace SunamoRoslyn.Roslyns;

/// <summary>
/// Represents the result of processing a source file, including whether it was indexed and its syntax data.
/// </summary>
public class ProcessFileBoolResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessFileBoolResult"/> class.
    /// </summary>
    public ProcessFileBoolResult()
    {

    }

    /// <summary>
    /// Gets or sets a value indicating whether the file was successfully indexed.
    /// </summary>
    public bool Indexed { get; set; } = false;

    /// <summary>
    /// Gets or sets the syntax tree of the processed file.
    /// </summary>
    public SyntaxTree? Tree { get; set; }

    /// <summary>
    /// Gets or sets the compilation unit syntax root of the processed file.
    /// </summary>
    public CompilationUnitSyntax? Root { get; set; }
}
