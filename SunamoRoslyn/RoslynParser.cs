namespace SunamoRoslyn;

/// <summary>
/// Parses C# code using Roslyn syntax tree classes.
/// RoslynParserText handles text/indexer-based parsing without Roslyn classes.
/// </summary>
public class RoslynParser
{
    /// <summary>
    /// Determines whether the given input string is valid C# code.
    /// </summary>
    /// <param name="input">The source code text to validate.</param>
    /// <returns>True if the input can be parsed as C# code; otherwise, false.</returns>
    public static bool IsCSharpCode(string input)
    {
        SyntaxTree? syntaxTree = null;
        try
        {
            syntaxTree = CSharpSyntaxTree.ParseText(input);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        var text = syntaxTree?.GetText().ToString();
        return syntaxTree != null;
    }

    /// <summary>
    /// Parses a method header string into a <see cref="MethodDeclarationSyntax"/>.
    /// </summary>
    /// <param name="methodHeader">The method header text to parse (body will be appended automatically).</param>
    /// <returns>The parsed method declaration syntax node.</returns>
    public static MethodDeclarationSyntax Method(string methodHeader)
    {
        methodHeader = methodHeader + "{}";
        var tree = CSharpSyntaxTree.ParseText(methodHeader);
        var root = tree.GetRoot();
        var childNodes = root.ChildNodes();
        return (MethodDeclarationSyntax)childNodes.First();
    }

    /// <summary>
    /// Extracts code elements from classes in ASPX code-behind files and writes them to a target folder.
    /// Reads comments inside for implementation details.
    /// </summary>
    /// <param name="folderFrom">The source folder containing ASPX code-behind files.</param>
    /// <param name="folderTo">The destination folder for output files.</param>
    /// <returns>Always returns null; files are written to the destination folder.</returns>
    public
#if ASYNC
async Task<List<string>?>
#else
List<string>?
#endif
GetCodeOfElementsClass(string folderFrom, string folderTo)
    {
        FS.WithEndSlash(ref folderFrom);
        FS.WithEndSlash(ref folderTo);
        var files = Directory.GetFiles(folderFrom, "*.aspx.cs", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
#if ASYNC
await
#endif
File.ReadAllTextAsync(file));
            List<string> result = new List<string>();
            SyntaxNode? syntaxNode;
            var classDeclaration = RoslynHelper.GetClass(tree.GetRoot(), out syntaxNode);
            if (classDeclaration == null || syntaxNode == null)
            {
                continue;
            }
            SyntaxAnnotation syntaxNodeAnnotation = new SyntaxAnnotation();
            syntaxNode = syntaxNode.WithAdditionalAnnotations(syntaxNodeAnnotation);
            SyntaxAnnotation classAnnotation = new SyntaxAnnotation();
            classDeclaration = classDeclaration.WithAdditionalAnnotations(classAnnotation);
            var root = tree.GetRoot();
            int count = classDeclaration.Members.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var item = classDeclaration.Members[i];
                classDeclaration = classDeclaration.RemoveNode(item, SyntaxRemoveOptions.KeepEndOfLine)!;
            }
            syntaxNode = syntaxNode.TrackNodes(classDeclaration);
            root = root.TrackNodes(syntaxNode);
            var data = syntaxNode.SyntaxTree.ToString();
            var fileTo = file.Replace(folderFrom, folderTo);
            await File.WriteAllTextAsync(fileTo, data);
        }
        return null;
    }

    /// <summary>
    /// Finds the top-most parent node in the syntax tree.
    /// </summary>
    /// <param name="classDeclaration">The syntax node to traverse upward from.</param>
    /// <returns>The root syntax node with no parent.</returns>
    private SyntaxNode FindTopParent(SyntaxNode classDeclaration)
    {
        var result = classDeclaration;
        while (result.Parent != null)
        {
            result = result.Parent;
        }
        return result;
    }

    /// <summary>
    /// Extracts variable declarations from a C# compilation unit.
    /// The first argument must be CompilationUnitSyntax because global usings
    /// are only available on that type.
    /// </summary>
    /// <param name="root">The compilation unit syntax root to extract variables from.</param>
    /// <param name="usings">Output list of using directives found in the compilation unit.</param>
    /// <returns>A collection of variable type-name pairs found in the class.</returns>
    public static ABCRoslyn GetVariablesInCsharp(CompilationUnitSyntax root, out List<string> usings)
    {
        ABCRoslyn result = new ABCRoslyn();
        usings = CSharpHelper.Usings(root);
        ClassDeclarationSyntax? classDeclaration = RoslynHelper.GetClass(root);
        if (classDeclaration == null)
        {
            return result;
        }
        var variableDeclarations = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>();
        foreach (var variableDeclaration in variableDeclarations)
        {
            string variableName = variableDeclaration.Declaration.Type.ToString();
            variableName = SHReplace.ReplaceOnce(variableName, "global::", "");
            int lastIndex = variableName.LastIndexOf('.');
            string namespaceName, className;
            SH.GetPartsByLocation(out namespaceName, out className, variableName, lastIndex);
            usings.Add(namespaceName);
            // in key type, in value name
            result.Add(ABRoslyn.Get(className, variableDeclaration.Declaration.Variables.First().Identifier.Text));
        }
        usings = usings.Distinct().ToList();
        return result;
    }

    /// <summary>
    /// Gets the access modifier keyword from a list of syntax tokens.
    /// </summary>
    /// <param name="modifiers">The list of modifier tokens to search.</param>
    /// <returns>The access modifier text, or empty string if none found.</returns>
    public static string GetAccessModifiers(SyntaxTokenList modifiers)
    {
        foreach (var item in modifiers)
        {
            switch (item.Kind())
            {
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.PrivateKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.ProtectedKeyword:
                    return item.WithoutTrivia().ToFullString();
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Parses declared and assigned variables from a code fragment.
    /// </summary>
    /// <param name="code">The code to parse; can be a string or a <see cref="SyntaxNode"/>.</param>
    /// <returns>A tuple of (declaredVariables, assignedVariables) lists.</returns>
    public static Tuple<List<string>, List<string>> ParseVariables(object code)
    {
        SyntaxNode syntaxRoot = SyntaxNodeFromObjectOrString(code);
        var variableDeclarations = syntaxRoot.DescendantNodes().OfType<VariableDeclarationSyntax>();
        var variableAssignments = syntaxRoot.DescendantNodes().OfType<AssignmentExpressionSyntax>();
        List<string> declaredVariables = new List<string>(variableDeclarations.Count());
        List<string> assignedVariables = new List<string>(variableAssignments.Count());
        foreach (var variableDeclaration in variableDeclarations)
            declaredVariables.Add(variableDeclaration.Variables.First().Identifier.Value?.ToString() ?? string.Empty);
        foreach (var variableAssignment in variableAssignments)
            assignedVariables.Add(variableAssignment.Left.ToString());
        return new Tuple<List<string>, List<string>>(declaredVariables, assignedVariables);
    }

    /// <summary>
    /// Creates a <see cref="SyntaxNode"/> from either a string of code or an existing SyntaxNode.
    /// </summary>
    /// <param name="code">The code to convert; can be a string or a <see cref="SyntaxNode"/>.</param>
    /// <returns>The resulting syntax node.</returns>
    public static SyntaxNode SyntaxNodeFromObjectOrString(object code)
    {
        if (code is SyntaxNode syntaxNode)
        {
            return syntaxNode;
        }
        else if (code is string codeText)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(codeText);
            return tree.GetRoot();
        }
        else
        {
            ThrowEx.NotImplementedCase("else");
            throw new InvalidOperationException("Unsupported code type");
        }
    }

    /// <summary>
    /// Gets all variables used in every method within the given source code.
    /// </summary>
    /// <param name="text">The C# source code text to analyze.</param>
    /// <returns>A dictionary mapping method names to their assigned variable names.</returns>
    public static Dictionary<string, List<string>> GetVariablesInEveryMethod(string text)
    {
        Dictionary<string, List<string>> methodVariables = new Dictionary<string, List<string>>();
        var tree = CSharpSyntaxTree.ParseText(text);
        var root = tree.GetRoot();
        IList<MethodDeclarationSyntax> methods = root
          .DescendantNodes()
          .OfType<MethodDeclarationSyntax>().ToList();
        foreach (var method in methods)
        {
            var value = ParseVariables(method);
            methodVariables.Add(method.Identifier.Text, value.Item2);
        }
        return methodVariables;
    }
}
