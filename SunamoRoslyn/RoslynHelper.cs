namespace SunamoRoslyn;

/// <summary>
/// Provides helper methods for working with Roslyn syntax trees, including variable analysis,
/// project loading, code wrapping, and syntax node operations.
/// </summary>
public partial class RoslynHelper
{
    /// <summary>
    /// Returns all types in the specified assembly whose names contain the given substring.
    /// </summary>
    /// <param name="assembly">The assembly to search for types.</param>
    /// <param name="contains">The substring to match against type names.</param>
    /// <returns>A list of types whose names contain the specified substring.</returns>
    public static List<Type> GetTypesInAssembly(Assembly assembly, string contains)
    {
        var types = assembly.GetTypes();
        return types.Where(temp => temp.Name.Contains(contains)).ToList();
    }

    /// <summary>
    /// Analyzes global variables in the given code and adds XML documentation comments
    /// listing which methods use each variable.
    /// The argument can be a <see cref="SyntaxNode"/> or a string of C# code.
    /// </summary>
    /// <param name="codeObject">A SyntaxNode or string representing the C# code to analyze.</param>
    /// <returns>The modified source code with usage documentation added to global variables.</returns>
    public static string AddWhereIsUsedVariablesInMethods(object codeObject)
    {
        SyntaxNode root = RoslynParser.SyntaxNodeFromObjectOrString(codeObject);
        var methods = ChildNodes.MethodsDescendant(root);
        var fields = ChildNodes.FieldsDescendant(root);
        string before;
        string after;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(root.ToFullString());
        Tuple<List<string>, List<string>> parsedVariables;
        Dictionary<string, List<string>> variableUsageMap = new Dictionary<string, List<string>>();
        string methodName;
        foreach (MethodDeclarationSyntax oldMethodNode in methods)
        {
            parsedVariables = RoslynParser.ParseVariables(oldMethodNode);
            methodName = oldMethodNode.Identifier.Text;
            foreach (var item in parsedVariables.Item2)
            {
                DictionaryHelper.AddOrCreate<string, string, object>(variableUsageMap, item, methodName);
            }
        }

        string variableName;
        List<string> usedIn;
        foreach (var oldMethodNode in fields)
        {
            variableName = oldMethodNode.Declaration.Variables.First().Identifier.Text;
            if (variableUsageMap.ContainsKey(variableName))
            {
                usedIn = variableUsageMap[variableName];
            }
            else
            {
                continue;
            }

            CA.Prepend("/// ", usedIn);
            var doc = @"
/// <summary>
" + string.Join(Environment.NewLine, usedIn) + @"
/// </summary>
";
            var parentNode = oldMethodNode.Parent;
            if (IsGlobalVariable(oldMethodNode))
            {
                var lt = SyntaxFactory.Comment(doc);
                var oldMethodNode2 = oldMethodNode.WithLeadingTrivia(SyntaxTriviaList.Create(lt));
                before = oldMethodNode.ToFullString();
                after = oldMethodNode2.ToFullString();
                stringBuilder = stringBuilder.Replace(before, after);
            }
        }

        var result = stringBuilder.ToString();
        return result;
    }

    /// <summary>
    /// Determines whether the given syntax node represents a global (class-level) variable
    /// rather than a local variable inside a method body.
    /// VariableDeclarationSyntax->CSharpSyntaxNode
    /// FieldDeclarationSyntax->BaseFieldDeclarationSyntax->MemberDeclarationSyntax->CSharpSyntaxNode
    /// </summary>
    /// <param name="syntaxNode">The syntax node to check.</param>
    /// <returns>True if the node is a class-level field; false if it is inside a method block.</returns>
    private static bool IsGlobalVariable(CSharpSyntaxNode syntaxNode)
    {
        var parent = syntaxNode.Parent;
        while (parent != null)
        {
            if (parent is BlockSyntax)
            {
                return false;
            }
            else if (parent is ClassDeclarationSyntax)
            {
                return true;
            }

            parent = parent.Parent;
        }

        return false;
    }

    /// <summary>
    /// Returns all projects in the specified solution, including referenced projects.
    /// If you want only projects listed directly in the .sln file, use AP.GetProjectsInSlnFile instead.
    /// </summary>
    /// <param name="slnPath">The file path to the solution (.sln) file.</param>
    /// <param name="isSkippingUnrecognizedProjects">Whether to skip projects that cannot be recognized by MSBuild.</param>
    /// <returns>A list of all projects in the solution.</returns>
    public static
#if ASYNC
        async Task<List<Project>>
#else
    List<Project>
#endif
    GetAllProjectsInSolution(string slnPath, bool isSkippingUnrecognizedProjects = false)
    {
        var formattingOptions = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        var msWorkspace = MSBuildWorkspace.Create();
        msWorkspace.SkipUnrecognizedProjects = isSkippingUnrecognizedProjects;
        msWorkspace.LoadMetadataForReferencedProjects = false;
        var solution =
#if ASYNC
            await msWorkspace.OpenSolutionAsync(slnPath);
#else
        msWorkspace.OpenSolutionAsync(slnPath).Result;
#endif
        return solution.Projects.ToList();
    }

    /// <summary>
    /// Wraps the given code snippet inside a dummy class declaration.
    /// </summary>
    /// <param name="code">The C# code to wrap.</param>
    /// <returns>The code wrapped in a class declaration block.</returns>
    public static string WrapIntoClass(string code)
    {
        return RoslynNotTranslateAble.ClassDummy + " {" + code + "}";
    }

    /// <summary>
    /// Parses the given C# code into a syntax tree.
    /// The code must start with a class or namespace declaration unless wrapped.
    /// </summary>
    /// <param name="code">The C# code to parse.</param>
    /// <param name="isWrappingIntoClass">Whether to wrap the code in a dummy class before parsing.</param>
    /// <returns>The parsed syntax tree.</returns>
    public static SyntaxTree GetSyntaxTree(string code, bool isWrappingIntoClass = false)
    {
        if (isWrappingIntoClass)
        {
            code = WrapIntoClass(code);
        }

        return CSharpSyntaxTree.ParseText(code);
    }

    /// <summary>
    /// Gets the first class declaration from the given root syntax node.
    /// </summary>
    /// <param name="root">The root syntax node to search.</param>
    /// <returns>The class declaration syntax node, or null if multiple classes exist.</returns>
    public static ClassDeclarationSyntax? GetClass(SyntaxNode root)
    {
        SyntaxNode? syntaxNode;
        return GetClass(root, out syntaxNode);
    }

    /// <summary>
    /// Finds a syntax node within a parent that matches the given child node, searching only direct children.
    /// </summary>
    /// <param name="parent">The parent syntax node to search within.</param>
    /// <param name="child">The child syntax node to find.</param>
    /// <param name="isOnlyDirectSub">Whether to search only direct child nodes.</param>
    /// <returns>The found syntax node, or null if not found.</returns>
    public static SyntaxNode? FindNode(SyntaxNode parent, SyntaxNode child, bool isOnlyDirectSub)
    {
        int foundIndex;
        return FindNode(parent, child, isOnlyDirectSub, out foundIndex);
    }
}
