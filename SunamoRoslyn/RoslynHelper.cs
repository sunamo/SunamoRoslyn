namespace SunamoRoslyn;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class RoslynHelper
{
    static Type type = typeof(RoslynHelper);
    public static List<Type> GetTypesInAssembly(Assembly assembly, string contains)
    {
        var types = assembly.GetTypes();
        return types.Where(temp => temp.Name.Contains(contains)).ToList();
    }

    /// <summary>
    /// A1 can be SyntaxNode or string
    /// </summary>
    /// <param name = "o"></param>
    /// <returns></returns>
    public static string AddWhereIsUsedVariablesInMethods(object o)
    {
        SyntaxNode root = RoslynParser.SyntaxNodeFromObjectOrString(o);
        var methods = ChildNodes.MethodsDescendant(root);
        var fields = ChildNodes.FieldsDescendant(root);
        string before = null;
        string after = null;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(root.ToFullString());
        Tuple<List<string>, List<string>> ls = null;
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        string methodName = null;
        foreach (MethodDeclarationSyntax oldMethodNode in methods)
        {
            ls = RoslynParser.ParseVariables(oldMethodNode);
            methodName = oldMethodNode.Identifier.Text;
            foreach (var item in ls.Item2)
            {
                DictionaryHelper.AddOrCreate<string, string, object>(dict, item, methodName);
            }
#region MyRegion
        //        var testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
        //        SyntaxKind.SingleLineDocumentationCommentTrivia,
        //        SyntaxFactory.List<XmlNodeSyntax>(
        //            new XmlNodeSyntax[]{
        //    SyntaxFactory.XmlText()
        //    .WithTextTokens(
        //        SyntaxFactory.TokenList(
        //            SyntaxFactory.XmlTextLiteral(
        //                SyntaxFactory.TriviaList(
        //                    SyntaxFactory.DocumentationCommentExterior("///")),
        //                " ",
        //                " ",
        //                SyntaxFactory.TriviaList()))),
        //    SyntaxFactory.XmlElement(
        //        SyntaxFactory.XmlElementStartTag(
        //            SyntaxFactory.XmlName(
        //                SyntaxFactory.Identifier("summary"))),
        //        SyntaxFactory.XmlElementEndTag(
        //            SyntaxFactory.XmlName(
        //                SyntaxFactory.Identifier("summary"))))
        //    .WithContent(
        //        SyntaxFactory.SingletonList<XmlNodeSyntax>(
        //            SyntaxFactory.XmlText()
        //            .WithTextTokens(
        //                SyntaxFactory.TokenList(
        //                    SyntaxFactory.XmlTextLiteral(
        //                        SyntaxFactory.TriviaList(),
        //                        "test",
        //                        "test",
        //                        SyntaxFactory.TriviaList()))))),
        //    SyntaxFactory.XmlText()
        //    .WithTextTokens(
        //        SyntaxFactory.TokenList(
        //            SyntaxFactory.XmlTextNewLine(
        //                SyntaxFactory.TriviaList(),
        //                "\n",
        //                "\n",
        //                SyntaxFactory.TriviaList())))}));
        //        var newMethodNode = oldMethodNode.WithModifiers(
        //SyntaxFactory.TokenList(
        //    new[]{
        //    SyntaxFactory.Token(
        //        SyntaxFactory.TriviaList(
        //            SyntaxFactory.Trivia(testDocumentation)), // xmldoc
        //            SyntaxKind.PublicKeyword, // original 1st token
        //            SyntaxFactory.TriviaList())
        //        //SyntaxFactory.Token(SyntaxKind.StaticKeyword)
        //    }));
        //var leadingTrivia = oldMethodNode.GetLeadingTrivia();
        //for (i = leadingTrivia.Count - 1; i >= 0; i--)
        //{
        //    leadingTrivia.RemoveAt(i);
        //}
#endregion
        }

        string variableName = null;
        List<string> usedIn = null;
        foreach (var oldMethodNode in fields)
        {
            variableName = oldMethodNode.Declaration.Variables.First().Identifier.Text;
            if (dict.ContainsKey(variableName))
            {
                usedIn = dict[variableName];
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
            var parameter = oldMethodNode.Parent;
            if (IsGlobalVariable(oldMethodNode))
            {
                var lt = SyntaxFactory.Comment(doc);
                var oldMethodNode2 = oldMethodNode.WithLeadingTrivia(SyntaxTriviaList.Create(lt));
                before = oldMethodNode.ToFullString();
                //root = root.ReplaceNode(oldMethodNode, oldMethodNode2);
                after = oldMethodNode2.ToFullString();
                stringBuilder = stringBuilder.Replace(before, after);
            }
        }

        var result = stringBuilder.ToString(); // root.ToFullString();
        return result;
    }

    /// <summary>
    /// VariableDeclarationSyntax->CSharpSyntaxNode
    /// FieldDeclarationSyntax->BaseFieldDeclarationSyntax->MemberDeclarationSyntax->CSharpSyntaxNode
    /// </summary>
    /// <param name = "oldMethodNode"></param>
    /// <returns></returns>
    private static bool IsGlobalVariable(CSharpSyntaxNode oldMethodNode)
    {
        var parent = oldMethodNode.Parent;
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
    /// Return also referenced projects (sunamo return also duo and Resources, although is not in sunamo)
    /// If I want what is only in sln, use AP.GetProjectsInSlnFile
    /// </summary>
    /// <param name = "slnPath"></param>
    /// <param name = "SkipUnrecognizedProjects"></param>
    public static 
#if ASYNC
        async Task<List<Project>>
#else
    List<Project> 
#endif
    GetAllProjectsInSolution(string slnPath, bool SkipUnrecognizedProjects = false)
    {
        var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        var msWorkspace = MSBuildWorkspace.Create();
        // Will include also referenced file
        msWorkspace.SkipUnrecognizedProjects = SkipUnrecognizedProjects;
        msWorkspace.LoadMetadataForReferencedProjects = false;
        //msWorkspace.Options.WithChangedOption(OptionKey.)
        //msWorkspace.Properties.
        //msWorkspace.Services.
        var solution = 
#if ASYNC
            await msWorkspace.OpenSolutionAsync(slnPath);
#else
        // not have non async
        msWorkspace.OpenSolutionAsync(slnPath).Result;
#endif
        // Solution or project cant be dumped with Yaml
        //////DebugLogger.Instance.DumpObject("solution", solution, DumpProvider.Yaml);
        //foreach (var project in solution.Projects)
        //{
        //    ////DebugLogger.Instance.DumpObject("", project, DumpProvider.Yaml);
        //}
        return solution.Projects.ToList();
    }

    public static string WrapIntoClass(string code)
    {
        return RoslynNotTranslateAble.classDummy + " {" + code + "}";
    }

    /// <summary>
    /// A2 - first must be class or namespace
    /// </summary>
    /// <param name = "code"></param>
    /// <param name = "wrapIntoClass"></param>
    public static SyntaxTree GetSyntaxTree(string code, bool wrapIntoClass = false)
    {
        if (wrapIntoClass)
        {
            code = WrapIntoClass(code);
        }

        return CSharpSyntaxTree.ParseText(code);
    }

    public static ClassDeclarationSyntax GetClass(SyntaxNode root)
    {
        SyntaxNode sn;
        return GetClass(root, out sn);
    }

    public static SyntaxNode FindNode(SyntaxNode parent, SyntaxNode child, bool onlyDirectSub)
    {
        int dx;
        return FindNode(parent, child, onlyDirectSub, out dx);
    }
}