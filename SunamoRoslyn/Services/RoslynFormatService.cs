namespace SunamoRoslyn.Services;

/// <summary>
/// Provides methods for formatting C# source code using Roslyn.
/// </summary>
internal class RoslynFormatService
{
    /// <summary>
    /// Formats C# source code by decoding HTML and parsing it into a syntax tree.
    /// Currently returns null as the formatting via Roslyn.Services is not available on NuGet.
    /// </summary>
    /// <param name="format">The C# source code to format, possibly HTML-encoded.</param>
    /// <returns>Currently returns null.</returns>
    public static string? Format3(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.BrTagCaseInsensitiveRegex.ToString(), string.Empty);
        // Create SyntaxTree
        SyntaxTree firstTree = CSharpSyntaxTree.ParseText(format);
        var root = firstTree.GetRoot();
        // Format() is in Roslyn.Services which is not actually on nuget
        //root.Format(FormattingOptions.GetDefaultOptions()).GetFormattedRoot().GetText().ToString();
        return null;
    }
    /// <summary>
    /// Formats C# source code and removes empty lines.
    /// Code must be compilable. When it is not (e.g. missing semicolons, private in variables),
    /// returns input without changes.
    /// Format2 removes empty lines, Format keeps empty lines.
    /// </summary>
    /// <param name="format">The C# source code to format, possibly HTML-encoded.</param>
    /// <returns>The formatted source code with empty lines removed.</returns>
    public static string Format2(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.BrTagCaseInsensitiveRegex.ToString(), string.Empty);
        var workspace = MSBuildWorkspace.Create();
        // Create SyntaxTree
        SyntaxTree firstTree = CSharpSyntaxTree.ParseText(format);
        var root = firstTree.GetRoot();
        var formattedNode = Microsoft.CodeAnalysis.Formatting.Formatter.Format(root, workspace);
        // Insert space between all tokens, replace all newlines by spaces
        //formattedNode = root.NormalizeWhitespace();
        // With ToFullString and ToString is output the same - good but without new lines
        var formattedText = formattedNode.ToFullString();
        return FinalizeFormat(formattedText);
    }
    /// <summary>
    /// Formats C# source code and keeps empty lines.
    /// Code must be compilable. When it is not (e.g. missing semicolons, private in variables),
    /// returns input without changes.
    /// Format2 removes empty lines, Format keeps empty lines.
    /// </summary>
    /// <param name="format">The C# source code to format, possibly HTML-encoded.</param>
    /// <returns>The formatted source code with empty lines preserved.</returns>
    public static string Format(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.BrTagCaseInsensitiveRegex.ToString(), string.Empty);
        var workspace = MSBuildWorkspace.Create();
        StringBuilder stringBuilder = new StringBuilder();
        // Create SyntaxTree
        SyntaxTree firstTree = CSharpSyntaxTree.ParseText(format);
        // Get root of ST
        var firstRoot = firstTree.GetRoot();
        #region Process all incomplete nodes in ChildNodesAndTokens and insert into syntaxNodes2
        // Get all Child nodes
        var syntaxNodes = firstRoot.ChildNodesAndTokens();
        // Whether at least one in syntaxNodes is SyntaxNodeOrToken - take its parent
        bool isToken = false;
        // Whether at least one in syntaxNodes is SyntaxNode - take itself
        bool isNode = false;
        // Parent if isToken or itself if isNode
        SyntaxNode? targetNode = null;
        List<SyntaxNode> syntaxNodes2 = new List<SyntaxNode>();
        // Process all incomplete members
        #region If its only code fragment, almost everything will be here IncompleteMember and on end will be delete from code
        //foreach (var item in syntaxNodes.Where(d => d.Kind() == SyntaxKind.IncompleteMember))
        //{
        //    var node3 = item.AsNode();
        //    // insert output of AsNode
        //    syntaxNodes2.Add(node3);
        //}
        #endregion
        #endregion
        // Again get ChildNodesAndTokens, dont know why because should be immutable
        syntaxNodes = firstRoot.ChildNodesAndTokens();
        // Process all syntaxNodes but output of all elements is saved to 2 variables
        // Must be set only to one variable due to raise exception
        foreach (var syntaxNode in syntaxNodes)
        {
            var syntaxNodeType = syntaxNode.GetType();
            string fullTypeName = syntaxNodeType.FullName?.ToString() ?? string.Empty;
            if (syntaxNodeType == typeof(SyntaxNodeOrToken))
            {
                isToken = true;
                targetNode = syntaxNode.Parent;
                break;
            }
            else if (syntaxNodeType == typeof(SyntaxNode))
            {
                isNode = true;
                targetNode = (SyntaxNode?)syntaxNode;
            }
            else
            {
                ThrowEx.NotImplementedCase(syntaxNodeType);
            }
        }
        if (isNode && isToken)
        {
            throw new Exception("CantProcessTokenAndSyntaxNodeOutputCouldBeDuplicated");
        }
        // Early if isToken we get Parent, so now we dont get Parent again
        var parentNode = targetNode!.Parent;
        if (isToken)
        {
            parentNode = targetNode;
        }
        // Remove nodes which was marked as Incomplete members
        targetNode = parentNode!.ReplaceNode(targetNode, targetNode.RemoveNodes(syntaxNodes2, SyntaxRemoveOptions.AddElasticMarker)!);
        // Only for debugging
        var nodesAndTokens = targetNode!.ChildNodesAndTokens();
        // Dont get to OptionSet - abstract. DocumentOptionSet - sealed, no static member, no ctor.
        //OptionSet os = new DocumentOptionSet()
        var formattedNode = Microsoft.CodeAnalysis.Formatting.Formatter.Format(targetNode, workspace);
        stringBuilder.AppendLine(formattedNode.ToFullString().Trim());
        string formattedText = stringBuilder.ToString();
        //var formattedResult2 = RoslynServicesHelper.Format(formattedText);
        return FinalizeFormat(formattedText);
    }

    /// <summary>
    /// Finalizes the formatting by adjusting indentation and inserting empty lines before comments.
    /// </summary>
    /// <param name="text">The formatted source code text to finalize.</param>
    /// <returns>The finalized source code with proper indentation and spacing.</returns>
    static string FinalizeFormat(string text)
    {
        var lines = SHGetLines.GetLines(text);
        //SH.MultiWhitespaceLineToSingle(lines);
        SH.IndentAsPreviousLine(lines);
        // Important, otherwise is every line delimited by empty
        CA.RemoveStringsEmptyTrimBefore(lines);
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            var line = lines[i];
            var trimmedLine = lines[i].Trim();
            if (trimmedLine.StartsWith(CSharpConsts.LineComment) && !trimmedLine.StartsWith("///"))
            {
                if (i != 0)
                {
                    if (lines[i - 1].Trim() != "{")
                    {
                        lines.Insert(i, string.Empty);
                    }
                }
            }
        }
        var joinedLines = string.Join(Environment.NewLine, lines);
        //joinedLines = joinedLines.Replace(CsKeywords.ns, Environment.NewLine + CsKeywords.ns);
        return joinedLines;
    }
}