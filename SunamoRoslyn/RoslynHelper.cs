namespace SunamoRoslyn;

public partial class RoslynHelper
{
    public static List<Type> GetTypesInAssembly(Assembly assembly, string contains)
    {
        var types = assembly.GetTypes();
        return types.Where(temp => temp.Name.Contains(contains)).ToList();
    }

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

    // VariableDeclarationSyntax->CSharpSyntaxNode
    // FieldDeclarationSyntax->BaseFieldDeclarationSyntax->MemberDeclarationSyntax->CSharpSyntaxNode
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

#if !NETSTANDARD2_0
    // If you want only projects listed directly in the .sln file, use AP.GetProjectsInSlnFile instead.
    public static
        async Task<List<Project>>
    GetAllProjectsInSolution(string slnPath, bool isSkippingUnrecognizedProjects = false)
    {
        var formattingOptions = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        var msWorkspace = MSBuildWorkspace.Create();
        msWorkspace.SkipUnrecognizedProjects = isSkippingUnrecognizedProjects;
        msWorkspace.LoadMetadataForReferencedProjects = false;
        var solution =
            await msWorkspace.OpenSolutionAsync(slnPath);
        return solution.Projects.ToList();
    }
#endif

    public static string WrapIntoClass(string code)
    {
        return RoslynNotTranslateAble.ClassDummy + " {" + code + "}";
    }

    public static SyntaxTree GetSyntaxTree(string code, bool isWrappingIntoClass = false)
    {
        if (isWrappingIntoClass)
        {
            code = WrapIntoClass(code);
        }

        return CSharpSyntaxTree.ParseText(code);
    }

    public static ClassDeclarationSyntax? GetClass(SyntaxNode root)
    {
        SyntaxNode? syntaxNode;
        return GetClass(root, out syntaxNode);
    }

    public static SyntaxNode? FindNode(SyntaxNode parent, SyntaxNode child, bool isOnlyDirectSub)
    {
        int foundIndex;
        return FindNode(parent, child, isOnlyDirectSub, out foundIndex);
    }
}
