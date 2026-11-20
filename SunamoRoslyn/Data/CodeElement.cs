namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the code element t type.
/// </summary>
public class CodeElement<T>
{
    /// <summary>
    /// Gets or sets the name without generic.
    /// </summary>
    public string NameWithoutGeneric { get; set; } = string.Empty;

    string name = string.Empty;

    /// <summary>
    /// The name.
    /// </summary>
    public string Name
    {
        get => name;
        set
        {
            name = value;
            NameWithoutGeneric = RoslynHelper.NameWithoutGeneric(name);
        }
    }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    public T Type { get; set; } = default!;

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the from.
    /// </summary>
    public int From { get; set; }

    /// <summary>
    /// Gets or sets the to.
    /// </summary>
    public int To { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    public int Length { get; set; }

    // Base classes of MemberDeclarationSyntax are only CSharpSyntaxNode and SyntaxNode.
    /// <summary>
    /// Gets or sets the member.
    /// </summary>
    public MemberDeclarationSyntax? Member { get; set; }
}
