namespace SunamoRoslyn.Data;

public class CodeElement<T>
{
    public string NameWithoutGeneric { get; set; } = string.Empty;

    string name = string.Empty;

    public string Name
    {
        get => name;
        set
        {
            name = value;
            NameWithoutGeneric = RoslynHelper.NameWithoutGeneric(name);
        }
    }

    public T Type { get; set; } = default!;

    public int Index { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public int Length { get; set; }

    // Base classes of MemberDeclarationSyntax are only CSharpSyntaxNode and SyntaxNode.
    public MemberDeclarationSyntax? Member { get; set; }
}
