namespace SunamoRoslyn;

public class RoslynParents
{
    private Dictionary<string, SyntaxNode> n = new();

    public void Add(string where, SyntaxNode n)
    {
        //this.n.Add(where, n);
        if (n != null)
        {
            ////DebugLogger.Instance.WriteLine(where + SH.NullToStringOrDefault(n.Parent, ("not null")));
        }
    }
}