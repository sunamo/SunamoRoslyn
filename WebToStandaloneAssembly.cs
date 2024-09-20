
namespace SunamoRoslyn;
using SunamoStringFormat;

public class WebToStandaloneAssembly
{
    /// <summary>
    /// </summary>
    /// <param name="csClass"></param>
    /// <param name="ctorArgs"></param>
    private string GetContentOfNewAspxCs(string csClass, string ctorArgs)
    {
        const string template = @"
            protected void Page_Init(object sender, EventArgs e)
            [
                cs = new {0}({1});
            ]

            protected void Page_Load(object sender, EventArgs e)
            [
                cs.Page = ((Page)this).Page;
                cs.Page_Load(sender, e);
                cs.CreateTitle();
            ]";

        return SHFormat.Format(template, AllStringsSE.lsqb, AllStringsSE.rsqb, csClass, ctorArgs);
    }

    /// <summary>
    /// </summary>
    /// <param name="nsX"></param>
    /// <param name="className"></param>
    /// <param name="variables"></param>
    /// <param name="usings"></param>
    /// <param name="ctorArgs"></param>
    /// <param name="ctorInner"></param>
    /// <param name="baseClassCs"></param>
    /// <param name="nsBaseClassCs"></param>
    /// <param name="code"></param>
    public string GetContentOfPageCsFile(string nsX, string className, string variables, string usings, string ctorArgs,
        string ctorInner, string baseClassCs, string nsBaseClassCs, string code)
    {
        var template = SHFormat.Format(@"{3}
[
    public partial class {1}Cs : {6}
    [
        #region Variables & ctor
{2}

        public {1}Cs({4})
        [
            {5}
        ]
        #endregion

        [0]
    ]
]", AllStringsSE.lsqb, AllStringsSE.rsqb,
            nsX, className, variables, usings, ctorArgs, ctorInner, baseClassCs, nsBaseClassCs);
        template = SHFormat.Format3(template, code);
        return template;
    }

    /// <summary>
    ///     A1 = folder with aspx
    ///     A2 = folder with cs
    ///     A3 = LyricsPageCs etc.
    ///     A4 = Me, LyricsX etc.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="baseClassCs"></param>
    /// <param name="nsBaseClassCs"></param>
    public
#if ASYNC
        async Task
#else
void
#endif
        AspxCsToStandaloneAssembly(string from, string to, string baseClassCs, string nsBaseClassCs,
            List<string> skipAspx)
    {
        var files = Directory.GetFiles(from, FS.MascFromExtension(".aspx.cs"), SearchOption.TopDirectoryOnly);
        // Get namespace
        var ns = Path.GetFileName(from);
        var nsX = Path.GetFileName(to);


        foreach (var fileAspxCs in files)
        {
            var fnwoeAspxCs = Path.GetFileNameWithoutExtension(fileAspxCs);

            if (CA.IsEqualToAnyElement(Path.GetFileNameWithoutExtension(fnwoeAspxCs), skipAspx)) continue;

            var designer = Path.Combine(from, fnwoeAspxCs + ".aspx.designer.cs");

            var fullPathTo = Path.Combine(to, fnwoeAspxCs + "Cs.cs");

            #region Generate and save *Cs file

            CollectionWithoutDuplicates<string> usings;
            var v1 = File.Exists(designer);
            var v2 = !File.Exists(fullPathTo);
            if (v1 && v2)
            {
                #region Get variables in designer and *.aspx.cs

                var designerContent =
#if ASYNC
                    await
#endif
                        File.ReadAllTextAsync(designer);
                var fileAspxCsContent =
#if ASYNC
                    await
#endif
                        File.ReadAllTextAsync(fileAspxCs);
                // Move all html controls and variables from *.aspx.cs - everything must be in *Cs.cs
                var dict = RoslynParser.GetVariablesInCsharp(RoslynHelper.GetSyntaxTree(designerContent).GetRoot(),
                    SHGetLines.GetLines(fileAspxCsContent), out usings);
                usings.Add(ns);

                #endregion

                #region Get all other members in .cs

                var tree = CSharpSyntaxTree.ParseText(fileAspxCsContent);
                var swCode = new StringWriter();
                var cl = RoslynHelper.GetClass(tree.GetRoot());
                if (cl == null)
                {
                    ThisApp.Error(fnwoeAspxCs + " contains more classes");
                    continue;
                }

                SyntaxNode firstNode = null;

                var count = cl.Members.Count;
                for (var i = count - 1; i >= 0; i--)
                {
                    var item = cl.Members[i];
                    item.WriteTo(swCode);
                    //cl.Members.RemoveAt(i);
                    //cl.Members.Remove(item);
                    if (i == 0)
                    {
                        var firstTree = CSharpSyntaxTree.ParseText("            public " + fnwoeAspxCs +
                                                                   RoslynNotTranslateAble.CsCs + ";");
                        firstNode = firstTree.GetRoot().ChildNodes().First();
                        cl = cl.ReplaceNode(item, firstNode);
                    }
                    else
                    {
                        cl = cl.RemoveNode(item, SyntaxRemoveOptions.KeepEndOfLine);
                    }
                }

                swCode.Flush();

                #endregion

                var genVariables = new CSharpGenerator();
                foreach (var item2 in dict)
                    genVariables.Field(2, AccessModifiers.Private, false, VariableModifiers.None, item2.A,
                        item2.B.ToString(), false);

                var genUsings = new CSharpGenerator();
                foreach (var item2 in usings.c) genUsings.Using(item2);

                var onlyB = dict.OnlyBsList();
                var variables = genVariables.ToString();
                var usingsCode = genUsings.ToString();

                var ctorArgs =
                    SHJoin.JoinKeyValueCollection(dict.OnlyAs(), onlyB, AllStringsSE.space, AllStringsSE.comma);
                var ctorInner = CSharpHelper.GetCtorInner(3, onlyB);
                var code = swCode.ToString();
                code = SHReplace.ReplaceOnce(code, RoslynNotTranslateAble.protectedVoidPageLoad,
                    RoslynNotTranslateAble.publicOverrideVoidPageLoad);

                var c = GetContentOfNewAspxCs(fnwoeAspxCs + "Cs", string.Join(AllStringsSE.comma, onlyB));
                //SyntaxTree addedCode = CSharpSyntaxTree.ParseText(c);
                //var syntaxNodes = addedCode.GetRoot().ChildNodes().ToList();
                //var inserted = cl.SyntaxTree.GetRoot().InsertNodesAfter(firstNode, CAG.ToList<SyntaxNode>( syntaxNodes[0]));

                var contentFileNew = SHGetLines.GetLines(cl.SyntaxTree.ToString());
                var classIndex = -1;
                contentFileNew =
                    CSharpGenerator.AddIntoClass(contentFileNew, SHGetLines.GetLines(c), out classIndex, ns);

                var us = CAG.ToList(ns, ns + "X", XlfKeys.System, "System.Web.UI");
                var genUs = new CSharpGenerator();
                foreach (var item in us) genUs.Using(item);
                genUs.AppendLine();
                genUs.Namespace(0, ("sunamo.cz." + ns).TrimEnd(AllCharsSE.dot));


                contentFileNew.Insert(0, genUs.ToString());
                contentFileNew.Add(AllStringsSE.rcub);

                var content = GetContentOfPageCsFile(nsX, fnwoeAspxCs, variables, usingsCode, ctorArgs, ctorInner,
                    baseClassCs, nsBaseClassCs, code);
                content = SHReplace.ReplaceAll(content, string.Empty, "CreateEmpty();");

                // save .cs file
                await File.WriteAllLinesAsync(fileAspxCs, contentFileNew);
                // save new file
                await File.WriteAllTextAsync(fullPathTo, content);
            }

            #endregion
        }
    }
}