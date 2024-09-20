
namespace SunamoRoslyn;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using SunamoRoslyn._sunamo;

public class RoslynHelper
{
    private static Type type = typeof(RoslynHelper);

    public static void a()
    {
        var ass = typeof(ClassDeclarationSyntax).Assembly;

        var types = GetTypesInAssembly(ass, "DeclarationSyntax");
        var s = types.Select(d => d.Name).ToList();
        ClipboardHelper.SetLines(s);
    }

    public static List<Type> GetTypesInAssembly(Assembly assembly, string contains)
    {
        var types = assembly.GetTypes();
        return types.Where(t => t.Name.Contains(contains)).ToList();
    }

    /// <summary>
    ///     A1 can be SyntaxNode or string
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string AddWhereIsUsedVariablesInMethods(object o)
    {
        var root = RoslynParser.SyntaxNodeFromObjectOrString(o);
        var methods = ChildNodes.MethodsDescendant(root);
        var fields = ChildNodes.FieldsDescendant(root);

        string before = null;
        string after = null;
        var i = 0;

        var sb = new StringBuilder();
        sb.Append(root.ToFullString());

        Tuple<List<string>, List<string>> ls = null;

        var dict = new Dictionary<string, List<string>>();
        string methodName = null;

        foreach (var oldMethodNode in methods)
        {
            ls = RoslynParser.ParseVariables(oldMethodNode);

            methodName = oldMethodNode.Identifier.Text;

            foreach (var item in ls.Item2) DictionaryHelperSE.AddOrCreate(dict, item, methodName);

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
                usedIn = dict[variableName];
            else
                continue;

            CA.Prepend("/// ", usedIn);

            var doc = @"
/// <summary>
" + string.Join(Environment.NewLine, usedIn) + @"
/// </summary>
";
            var p = oldMethodNode.Parent;

            if (IsGlobalVariable(oldMethodNode))
            {
                var lt = SyntaxFactory.Comment(doc);
                var oldMethodNode2 = oldMethodNode.WithLeadingTrivia(SyntaxTriviaList.Create(lt));


                before = oldMethodNode.ToFullString();
                //root = root.ReplaceNode(oldMethodNode, oldMethodNode2);
                after = oldMethodNode2.ToFullString();

                sb = sb.Replace(before, after);
            }
        }

        var result = sb.ToString(); // root.ToFullString();
        return result;
    }

    /// <summary>
    ///     VariableDeclarationSyntax->CSharpSyntaxNode
    ///     FieldDeclarationSyntax->BaseFieldDeclarationSyntax->MemberDeclarationSyntax->CSharpSyntaxNode
    /// </summary>
    /// <param name="oldMethodNode"></param>
    /// <returns></returns>
    private static bool IsGlobalVariable(CSharpSyntaxNode oldMethodNode)
    {
        var parent = oldMethodNode.Parent;
        while (parent != null)
        {
            if (parent is BlockSyntax)
                return false;
            if (parent is ClassDeclarationSyntax) return true;
            parent = parent.Parent;
        }

        return false;
    }

    /// <summary>
    ///     Return also referenced projects (sunamo return also duo and Resources, although is not in sunamo)
    ///     If I want what is only in sln, use APSH.GetProjectsInSlnFile
    /// </summary>
    /// <param name="slnPath"></param>
    /// <param name="SkipUnrecognizedProjects"></param>
    public static
#if ASYNC
        async Task<List<Project>>
#else
    List<Project>
#endif
        GetAllProjectsInSolution(string slnPath, bool SkipUnrecognizedProjects = false)
    {
        var _ = typeof(CSharpFormattingOptions);

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
        ////DebugLogger.Instance.DumpObject("solution", solution, DumpProvider.Yaml);
        //foreach (var project in solution.Projects)
        //{
        //    //DebugLogger.Instance.DumpObject("", project, DumpProvider.Yaml);
        //}
        return solution.Projects.ToList();
    }

    public static string WrapIntoClass(string code)
    {
        return RoslynNotTranslateAble.classDummy + " {" + code + "}";
    }

    /// <summary>
    ///     A2 - first must be class or namespace
    /// </summary>
    /// <param name="code"></param>
    /// <param name="wrapIntoClass"></param>
    public static SyntaxTree GetSyntaxTree(string code, bool wrapIntoClass = false)
    {
        if (wrapIntoClass) code = WrapIntoClass(code);

        return CSharpSyntaxTree.ParseText(code);
    }

    public static string Format3(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.rBrTagCaseInsensitive.ToString(), string.Empty);

        // Create SyntaxTree
        var firstTree = CSharpSyntaxTree.ParseText(format);

        var root = firstTree.GetRoot();

        // Format() is in Roslyn.Services which is not actually on nuget
        //root.Format(FormattingOptions.GetDefaultOptions()).GetFormattedRoot().GetText().ToString();

        return null;
    }

    /// <summary>
    ///     Format good
    ///     Format2 = remove empty lines Format = keep empty lines
    ///     code must be compilable
    ///     when isnt ; i) instead od ; i++), private in variables return input without changes
    /// </summary>
    /// <param name="format"></param>
    public static string Format2(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.rBrTagCaseInsensitive.ToString(), string.Empty);

        var workspace = MSBuildWorkspace.Create();

        // Create SyntaxTree
        var firstTree = CSharpSyntaxTree.ParseText(format);

        var root = firstTree.GetRoot();

        var vr = Formatter.Format(root, workspace);
        // Instert space between all tokens, replace all nl by spaces
        //vr = root.NormalizeWhitespace();
        // With ToFullString and ToString is output the same - good but without new lines
        var formattedText = vr.ToFullString();


        return FinalizeFormat(formattedText);
    }

    /// <summary>
    ///     Format good
    ///     Format2 = remove empty lines Format = keep empty lines
    ///     code must be compilable
    ///     when isnt ; i) instead od ; i++), private in variables return input without changes
    /// </summary>
    /// <param name="format"></param>
    public static string Format(string format)
    {
        // Decode from HTML -
        format = WebUtility.HtmlDecode(format);
        // Replace all <br> with empty
        format = Regex.Replace(format, RegexHelper.rBrTagCaseInsensitive.ToString(), string.Empty);

        var workspace = MSBuildWorkspace.Create();
        var sb = new StringBuilder();

        // Create SyntaxTree
        var firstTree = CSharpSyntaxTree.ParseText(format);

        // Get root of ST
        var firstRoot = firstTree.GetRoot();


        #region Process all incomplete nodes in ChildNodesAndTokens and insert into syntaxNodes2

        // Get all Child nodes
        var syntaxNodes = firstRoot.ChildNodesAndTokens();

        // Whether at least one in syntaxNodes is SyntaxNodeOrToken - take its parent
        var token = false;
        // Whether at least one in syntaxNodes is SyntaxNode - take itself
        var node2 = false;

        // Parent if token or itself if node2
        SyntaxNode node = null;

        var syntaxNodes2 = new List<SyntaxNode>();

        // Process all incomplete members

        #region If its only code fragment, almost everything will be here IncompleteMember and on end will be delete from code

        //foreach (var item in syntaxNodes.Where(d => d.Kind() == SyntaxKind.IncompleteMember))
        //{
        //    // zde nevím co to dělá
        //    var node3 = item.AsNode();

        //    // insert output of AsNode
        //    syntaxNodes2.Add(node3);
        //}

        #endregion

        #endregion

        // Again get ChildNodesAndTokens, dont know why because should be immutable
        syntaxNodes = firstRoot.ChildNodesAndTokens();
        // WTF? Process all syntaxNodes but output of all elements is save to 2 variables?
        // Must be set only to one variable due to raise exception
        foreach (var syntaxNode in syntaxNodes)
        {
            var syntaxNodeType = syntaxNode.GetType();
            var s = syntaxNodeType.FullName;
            if (syntaxNodeType == typeof(SyntaxNodeOrToken))
            {
                token = true;
                node = syntaxNode.Parent;
                break;
            }

            if (syntaxNodeType == typeof(SyntaxNode))
            {
                node2 = true;
                node = (SyntaxNode)syntaxNode;
            }
            else
            {
                ThrowEx.NotImplementedCase(syntaxNodeType);
            }
        }

        if (node2 && token)
            throw new Exception(sess.i18n(XlfKeys.CantProcessTokenAndSyntaxNodeOutputCouldBeDuplicated));

        // Early if token we get Parent, so now we dont get Parent again
        var node4 = node.Parent;
        if (token) node4 = node;
        // Remove nodes which was marked as Incomplete members
        node = node4.ReplaceNode(node, node.RemoveNodes(syntaxNodes2, SyntaxRemoveOptions.AddElasticMarker));
        // Only for debugging
        var nodesAndTokens = node.ChildNodesAndTokens();

        // Dont get to OptionSet - abstract. DocumentOptionSet - sealed, no static member, no ctor.
        //OptionSet os = new DocumentOptionSet()


        var formattedResult = Formatter.Format(node, workspace);


        sb.AppendLine(formattedResult.ToFullString().Trim());

        var result = sb.ToString();


        //var formattedResult2 = RoslynServicesHelper.Format(result);

        return FinalizeFormat(result);
    }

    private static string FinalizeFormat(string result)
    {
        var lines = SHGetLines.GetLines(result);

        //SH.MultiWhitespaceLineToSingle(lines);

        SH.IndentAsPreviousLine(lines);
        // Important, otherwise is every line delimited by empty
        CA.RemoveStringsEmpty2(lines);

        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var line = lines[i];
            var trimmedLine = lines[i].Trim();
            if (trimmedLine.StartsWith(CSharpConsts.lc) && !trimmedLine.StartsWith("///"))
                if (i != 0)
                    if (lines[i - 1].Trim() != AllStringsSE.lcub)
                        lines.Insert(i, string.Empty);
        }

        var nl = string.Join(Environment.NewLine, lines);

        nl = nl.Replace(CsKeywords.ns, Environment.NewLine + CsKeywords.ns);


        return nl;
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

    /// <summary>
    ///     Because of searching is very unreliable
    ///     Into A1 I have to insert class when I search in classes. If I insert root/ns/etc, method will be return to me whole
    ///     class, because its contain method
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public static SyntaxNode FindNode(SyntaxNode parent, SyntaxNode child, bool onlyDirectSub, out int dx)
    {
        dx = -1;

        #region MyRegion

        if (false)
        {
            if (onlyDirectSub)
                // toto mi vratí např. jen public, nikoliv celou stránku
                //var ss = cl.ChildThatContainsPosition(cl.GetLocation().SourceSpan.Start);
                foreach (var item in parent.ChildNodes())
                {
                    // Má tu lokaci trochu dál protože obsahuje zároveň celou třídu
                    var l1 = item.GetLocation().ToString();
                    var l2 = child.GetLocation().ToString();

                    var s = child.Span;
                    var s2 = child.FullSpan;
                    var s3 = child.GetReference();

                    if (l1 == l2) return item;
                }
            else
                return parent.FindNode(child.FullSpan, false, true).WithoutLeadingTrivia().WithoutTrailingTrivia();

            return null;
        }

        #endregion

        var childType = child.GetType().FullName;
        var parentType = parent.GetType().FullName;

        SyntaxNode result = null;

        if (child is MethodDeclarationSyntax && parent is ClassDeclarationSyntax)
        {
            var cl = (ClassDeclarationSyntax)parent;
            var method = (MethodDeclarationSyntax)child;

            foreach (var item in cl.Members)
            {
                dx++;
                if (item is MethodDeclarationSyntax)
                {
                    var method2 = (MethodDeclarationSyntax)item;
                    var same = true;

                    if (method.Identifier.Text != method2.Identifier.Text) same = false;

                    if (same)
                        if (!RoslynComparer.Modifiers(method.Modifiers, method2.Modifiers))
                            same = false;

                    if (same)
                    {
                        var p1 = GetParameters(method.ParameterList);
                        var p2 = GetParameters(method2.ParameterList);
                        if (p1 != p2) same = false;
                    }

                    if (same)
                    {
                        result = method2;
                        break;
                    }
                }
            }
        }
        else if (child is BaseTypeDeclarationSyntax && parent is NamespaceDeclarationSyntax)
        {
            var ns = (NamespaceDeclarationSyntax)parent;
            var method = (BaseTypeDeclarationSyntax)child;

            foreach (BaseTypeDeclarationSyntax item in ns.Members)
            {
                dx++;
                if (method.Identifier.Value == item.Identifier.Value)
                {
                    result = method;
                    break;
                }
            }
        }
        else if (child is NamespaceDeclarationSyntax && parent is CompilationUnitSyntax)
        {
            var ns = (CompilationUnitSyntax)parent;
            var method = (NamespaceDeclarationSyntax)child;

            foreach (NamespaceDeclarationSyntax item in ns.Members)
            {
                dx++;
                var fs1 = method.Name.ToFullString();
                var fs2 = item.Name.ToFullString();
                if (fs1 == fs2)
                {
                    result = method;
                    break;
                }
            }
        }
        else if (child is ClassDeclarationSyntax && parent is CompilationUnitSyntax)
        {
            var ns = (CompilationUnitSyntax)parent;
            var method = (ClassDeclarationSyntax)child;

            foreach (ClassDeclarationSyntax item in ns.Members)
            {
                dx++;
                var fs1 = method.Identifier.ToFullString();
                var fs2 = item.Identifier.ToFullString();
                if (fs1 == fs2)
                {
                    result = method;
                    break;
                }
            }
        }
        else
        {
            ThrowEx.NotImplementedCase(SHJoin.JoinPairs(XlfKeys.Parent, parent.ToFullString(), XlfKeys.Child,
                child.ToFullString()));
        }


        return result;
        //return nsShared.FindNode(cl.FullSpan, false, true).WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    /// <summary>
    ///     IUN
    /// </summary>
    /// <param name="cl2"></param>
    /// <param name="method"></param>
    /// <param name="keepDirectives"></param>
    public static ClassDeclarationSyntax RemoveNode(ClassDeclarationSyntax cl2, SyntaxNode method,
        SyntaxRemoveOptions keepDirectives)
    {
        #region MyRegion

        //var children = method.ChildNodesAndTokens().ToList();
        //for (int i = children.Count() - 1; i >= 0; i--)
        //{
        //    var t = children[i].GetType().FullName;
        //    if (!(children[i] is MethodDeclarationSyntax))
        //    {
        //        int i2 = 0;
        //    }
        //    else
        //    {
        //        children.RemoveAt(i);
        //    }
        //}
        //return null;

        //FindNode()
        //cl2.Members.
        return null;

        #endregion
    }

    /// <summary>
    ///     A1 can be SyntaxTree, string
    ///     if it List<string>, use RoslynParser.Usings
    /// </summary>
    /// <param name="t"></param>
    public static IList<string> Usings(object t)
    {
        List<string> lines;
        return Usings(t, out lines);
    }

    /// <summary>
    ///     A1 can be SyntaxTree, string
    ///     if it List<string>, use RoslynParser.Usings
    /// </summary>
    /// <param name="t"></param>
    /// <param name="lines"></param>
    public static IList<string> Usings(object t, out List<string> lines)
    {
        string text = null;
        if (t is SyntaxTree || t is string) text = t.ToString();

        lines = SHGetLines.GetLines(text);
        return CSharpHelper.Usings(lines).c;
    }

    /// <summary>
    ///     Return null if
    ///     Into A2 insert first member of A1 - Namespace/Class
    ///     A1 should be rather Tree/CompilationUnitSyntax than Node because of Members - Node.ChildNodes.First is usings
    /// </summary>
    /// <param name="root"></param>
    /// <param name="ns"></param>
    public static ClassDeclarationSyntax GetClass(SyntaxNode root2, out SyntaxNode ns)
    {
        ns = null;
        ClassDeclarationSyntax helloWorldDeclaration = null;

        //(CompilationUnitSyntax)
        var root = root2;
        //var root = (CompilationUnitSyntax)tree.GetRoot();

        // Returns usings and ns
        var childNodes = root.ChildNodes();
        if (childNodes.OfType<ClassDeclarationSyntax>().Count() > 1) return null;
        SyntaxNode firstMember = null;
        firstMember = ChildNodes.NamespaceOrClass(root);
        //firstMember = (SyntaxNode)root.ChildNodes().OfType<NamespaceDeclarationSyntax>().FirstOrNull();
        //if (firstMember == null)
        //{
        //    firstMember = root.ChildNodes().OfType<ClassDeclarationSyntax>().First();
        //}

        if (firstMember is NamespaceDeclarationSyntax)
        {
            ns = (NamespaceDeclarationSyntax)firstMember;
            var i = 0;
            var fm = ((NamespaceDeclarationSyntax)ns).Members[i++];
            while (fm.GetType() != typeof(ClassDeclarationSyntax)) fm = ((NamespaceDeclarationSyntax)ns).Members[i++];
            helloWorldDeclaration = (ClassDeclarationSyntax)fm;
        }
        else if (firstMember is ClassDeclarationSyntax)
        {
            helloWorldDeclaration = (ClassDeclarationSyntax)firstMember;
            // keep ns as null
            //ns = nu;
        }
        else
        {
            ThrowEx.NotImplementedCase(firstMember);
        }

        return helloWorldDeclaration;
    }

    public static List<string> HeadersOfMethod(IList<SyntaxNode> enumerable, bool alsoModifier = true)
    {
        var clMethodsSharedNew = new List<string>();

        foreach (MethodDeclarationSyntax m in enumerable)
        {
            var h = GetHeaderOfMethod(m, alsoModifier);
            clMethodsSharedNew.Add(h);
            var i = 0;
        }

        return clMethodsSharedNew;
    }

    public static SyntaxNode WithoutAllTrivia(SyntaxNode sn)
    {
        return sn.WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    public static string GetHeaderOfMethod(MethodDeclarationSyntax m, bool alsoModifier = true)
    {
        m = m.WithoutTrivia();
        var sb = new InstantSB(AllStringsSE.space);

        if (alsoModifier) sb.AddItem(RoslynParser.GetAccessModifiers(m.Modifiers));

        var isStatic = IsStatic(m.Modifiers);
        if (isStatic) sb.AddItem("static");
        sb.AddItem(m.ReturnType.WithoutTrivia().ToFullString());
        sb.AddItem(m.Identifier.WithoutTrivia().Text);
        // in brackets, newline
        //string parameters = m.ParameterList.ToFullString();
        // only text
        var p2 = GetParameters(m.ParameterList);
        sb.AddItem(AllStringsSE.lb + p2 + AllStringsSE.rb);

        var s = sb.ToString();
        return s;
    }

    /// <summary>
    ///     CompilationUnitSyntax is also SyntaxNode
    ///     After line must be A1 = A2 or some RoslynHelper.Get* methods
    /// </summary>
    /// <param name="cl"></param>
    /// <param name="cl2"></param>
    /// <param name="root"></param>
    public static void ReplaceNode(SyntaxNode cl, SyntaxNode cl2, out SyntaxNode root)
    {
        ReplaceNode<SyntaxNode>(cl, cl2, out root);
    }

    /// <summary>
    ///     CompilationUnitSyntax is also SyntaxNode
    ///     After line must be A1 = A2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cl"></param>
    /// <param name="cl2"></param>
    public static T ReplaceNode<T>(SyntaxNode cl, SyntaxNode cl2, out SyntaxNode root) where T : SyntaxNode
    {
        var first = true;
        T result = default;
        while (cl is SyntaxNode)
        {
            if (cl.Parent == null) break;
            cl = cl.Parent.ReplaceNode(cl, cl2);

            if (first)
            {
                result = (T)cl2;
                first = false;
            }

            cl2 = cl;
            cl = cl.Parent;
        }

        root = cl2;
        if (result == null)
        {
        }

        return result;
    }

    private static string GetParameters(ParameterListSyntax parameterList)
    {
        var c1 = parameterList.ChildNodes();
        //var c2 = parameterList.ChildNodesAndTokens();
        var sb = new StringBuilder();
        foreach (var item in c1) sb.Append(item.ToFullString() + ", ");
        var r = SH.RemoveLastLetters(sb.ToString(), 2);
        return r;
    }

    public static bool IsStatic(SyntaxTokenList modifiers)
    {
        return modifiers.Where(e => e.Value.ToString() == "static").Count() > 0;
    }

    public static string NameWithoutGeneric(string name)
    {
        return SHParts.RemoveAfterFirst(name, AllStringsSE.lt);
    }
}