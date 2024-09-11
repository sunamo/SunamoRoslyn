namespace SunamoRoslyn.Data;

public class CodeElement<T>
{
    /// <summary>
    ///     Index in document with start
    /// </summary>
    public int From;

    public int Index;
    public int Length;

    /// <summary>
    ///     Base classes of MemberDeclarationSyntax is only CSharpSyntaxNode and SyntaxNode
    /// </summary>
    public MemberDeclarationSyntax Member;

    private string name;
    public string NameWithoutGeneric;

    /// <summary>
    ///     Index in document with last char
    /// </summary>
    public int To;

    public T Type;

    public string Name
    {
        get => name;
        set
        {
            name = value;
            NameWithoutGeneric = RoslynHelper.NameWithoutGeneric(name);
        }
    }
}