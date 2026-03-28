namespace SunamoRoslyn.Data;

/// <summary>
/// Represents a code element with a name, type, position information, and syntax member reference.
/// </summary>
/// <typeparam name="T">The type of the code element classification.</typeparam>
public class CodeElement<T>
{
    /// <summary>
    /// Gets or sets the name without generic type parameters.
    /// </summary>
    public string NameWithoutGeneric { get; set; } = string.Empty;

    string name = string.Empty;

    /// <summary>
    /// Gets or sets the name. Setting this also updates <see cref="NameWithoutGeneric"/>.
    /// </summary>
    public string Name
    {
        get { return name; }
        set
        {
            name = value;
            NameWithoutGeneric = RoslynHelper.NameWithoutGeneric(name);
        }
    }

    /// <summary>
    /// Gets or sets the classification type of this code element.
    /// </summary>
    public T Type { get; set; } = default!;

    /// <summary>
    /// Gets or sets the index of this code element within its parent collection.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the start index in the document.
    /// </summary>
    public int From { get; set; }

    /// <summary>
    /// Gets or sets the end index in the document (last character position).
    /// </summary>
    public int To { get; set; }

    /// <summary>
    /// Gets or sets the length of this code element.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Gets or sets the member declaration syntax node.
    /// Base classes of MemberDeclarationSyntax are only CSharpSyntaxNode and SyntaxNode.
    /// </summary>
    public MemberDeclarationSyntax? Member { get; set; }
}
