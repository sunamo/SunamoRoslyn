namespace SunamoRoslyn;

/// <summary>
///     RoslynParser - use roslyn classes
///     RoslynParserText - no use roslyn classes, only text or indexer
/// </summary>
public class RoslynParser
{
    // TODO: take also usings

    private static Type type = null;

    public static bool IsCSharpCode(string input)
    {
        SyntaxTree d = null;
        try
        {
            d = CSharpSyntaxTree.ParseText(input);
        }
        catch (Exception ex)
        {
            // throwed Method not found: 'Boolean Microsoft.CodeAnalysis.StackGuard.IsInsufficientExecutionStackException(System.Exception)'.' for non cs code
        }

        var s = d.GetText().ToString();
        return d != null;
    }

    public static MethodDeclarationSyntax Method(string item)
    {
        item = item + "{}";
        //StringReader sr = new StringReader(item);
        //CSharpSyntaxNode.DeserializeFrom();
        var tree = CSharpSyntaxTree.ParseText(item);
        var root = tree.GetRoot();
        //return (MethodDeclarationSyntax)root.DescendantNodesAndTokensAndSelf().OfType<MethodDeclarationSyntax>().FirstOrNull();

        // Only root I cannot cast -> cannot cast CSU to MethodDeclSyntax

        var childNodes = root.ChildNodes();
        return (MethodDeclarationSyntax)childNodes.First();
    }


    /// <summary>
    ///     Úplně nevím k čemu toto mělo sloužit.
    ///     Read comments inside
    /// </summary>
    /// <param name="folderFrom"></param>
    /// <param name="folderTo"></param>
    public
#if ASYNC
        async Task<List<string>>
#else
List<string>
#endif
        GetCodeOfElementsClass(string folderFrom, string folderTo)
    {
        FSSE.WithEndSlash(ref folderFrom);
        FSSE.WithEndSlash(ref folderTo);

        var files = Directory.GetFiles(folderFrom, FS.MascFromExtension(".aspx.cs"), SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            var tree = CSharpSyntaxTree.ParseText(
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(file));

            var result = new List<string>();
            // Here probable it mean SpaceName, ale když není namespace, uloží třídu
            SyntaxNode sn;
            var cl = RoslynHelper.GetClass(tree.GetRoot(), out sn);

            var saSn = new SyntaxAnnotation();
            sn = sn.WithAdditionalAnnotations(saSn);

            var saCl = new SyntaxAnnotation();
            cl = cl.WithAdditionalAnnotations(saCl);
            //ClassDeclarationSyntax cl2 = cl.Parent.)

            var root = tree.GetRoot();

            var count = cl.Members.Count;
            for (var i = count - 1; i >= 0; i--)
            {
                var item = cl.Members[i];
                //cl.Members.RemoveAt(i);
                //cl.Members.Remove(item);
                cl = cl.RemoveNode(item, SyntaxRemoveOptions.KeepEndOfLine);
            }

            // záměna namespace za class pak dělá problémy tady
            sn = sn.TrackNodes(cl);
            root = root.TrackNodes(sn);

            var d = sn.SyntaxTree.ToString();
            var fileTo = SHReplace.Replace(file, folderFrom, folderTo);
            await File.WriteAllTextAsync(fileTo, d);
        }

        return null;
    }

    private SyntaxNode FindTopParent(SyntaxNode cl)
    {
        var result = cl;
        while (result.Parent != null) result = result.Parent;
        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="root"></param>
    /// <param name="wrapIntoClass"></param>
    public static ABC GetVariablesInCsharp(SyntaxNode root)
    {
        var lines = new List<string>();
        CollectionWithoutDuplicates<string> usings;

        return GetVariablesInCsharp(root, lines, out usings);
    }

    public static Dictionary<string, string> GetVariablesInCsharp(SyntaxTree tree, out List<string> usings)
    {
        usings = new List<string>();
        var result = new Dictionary<string, string>();
        var root = (CompilationUnitSyntax)tree.GetRoot();

        var firstMember = root.Members[0];

        var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

        var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];

        var variableDeclarations = programDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>();

        foreach (var variableDeclaration in variableDeclarations)
        {
            //CL.WriteLine(variableDeclaration.Variables.First().Identifier.);
            //CL.WriteLine(variableDeclaration.Variables.First().Identifier.Value);
            var variableName = variableDeclaration.Declaration.Type.ToString();
            variableName = SHReplace.ReplaceOnce(variableName, "global::", "");
            var lastIndex = variableName.LastIndexOf(AllCharsSE.dot);
            string ns, cn;
            SH.GetPartsByLocation(out ns, out cn, variableName, lastIndex);
            usings.Add(ns);
            result.Add(cn, variableDeclaration.Declaration.Variables.First().Identifier.Text);
        }

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="root"></param>
    /// <param name="lines"></param>
    /// <param name="usings"></param>
    public static ABC GetVariablesInCsharp(SyntaxNode root, List<string> lines,
        out CollectionWithoutDuplicates<string> usings)
    {
        var result = new ABC();
        usings = CSharpHelper.Usings(lines);

        ClassDeclarationSyntax helloWorldDeclaration = null;
        helloWorldDeclaration = RoslynHelper.GetClass(root);

        var variableDeclarations = helloWorldDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>();

        foreach (var variableDeclaration in variableDeclarations)
        {
            //CL.WriteLine(variableDeclaration.Variables.First().Identifier.);
            //CL.WriteLine(variableDeclaration.Variables.First().Identifier.Value);
            var variableName = variableDeclaration.Declaration.Type.ToString();
            variableName = SHReplace.ReplaceOnce(variableName, "global::", "");
            var lastIndex = variableName.LastIndexOf(AllCharsSE.dot);
            string ns, cn;
            SH.GetPartsByLocation(out ns, out cn, variableName, lastIndex);
            usings.Add(ns);
            // in key type, in value name
            result.Add(AB.Get(cn, variableDeclaration.Declaration.Variables.First().Identifier.Text));
        }

        return result;
    }


    public static string GetAccessModifiers(SyntaxTokenList modifiers)
    {
        foreach (var item in modifiers)
            switch (item.Kind())
            {
                case SyntaxKind.PublicKeyword:

                case SyntaxKind.PrivateKeyword:

                case SyntaxKind.InternalKeyword:

                case SyntaxKind.ProtectedKeyword:
                    return item.WithoutTrivia().ToFullString();
            }

        return string.Empty;
    }

    /// <summary>
    ///     return declaredVariables, assignedVariables
    ///     A1 can be string or CompilationUnitSyntax
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static Tuple<List<string>, List<string>> ParseVariables(object code)
    {
        SyntaxNode root = null;
        string code2 = null;

        //MethodDeclarationSyntax;

        root = SyntaxNodeFromObjectOrString(code);

        var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>();
        var variableAssignments = root.DescendantNodes().OfType<AssignmentExpressionSyntax>();

        var declaredVariables = new List<string>(variableDeclarations.Count());
        var assignedVariables = new List<string>(variableAssignments.Count());

        foreach (var variableDeclaration in variableDeclarations)
            declaredVariables.Add(variableDeclaration.Variables.First().Identifier.Value.ToString());

        foreach (var variableAssignment in variableAssignments)
            assignedVariables.Add(variableAssignment.Left.ToString());

        return new Tuple<List<string>, List<string>>(declaredVariables, assignedVariables);
    }

    public static SyntaxNode SyntaxNodeFromObjectOrString(object code)
    {
        SyntaxNode root = null;

        if (code is SyntaxNode)
        {
            root = (SyntaxNode)code;
        }
        else if (code is string)
        {
            var tree = CSharpSyntaxTree.ParseText(code.ToString());
            root = tree.GetRoot();
        }
        else
        {
            ThrowEx.NotImplementedCase("else");
        }

        return root;
    }

    public static Dictionary<string, List<string>> GetVariablesInEveryMethod(string s)
    {
        var m = new Dictionary<string, List<string>>();

        var tree = CSharpSyntaxTree.ParseText(s);
        var root = tree.GetRoot();

        IList<MethodDeclarationSyntax> methods = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>().ToList();

        foreach (var method in methods)
        {
            var v = ParseVariables(method);
            m.Add(method.Identifier.Text, v.Item2);
        }

        return m;
    }
}