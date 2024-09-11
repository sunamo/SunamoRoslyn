namespace SunamoRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    public bool isCallingIsToIndexed = false;


    /// <summary>
    ///     used only in FileSystemWatcher
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fromFileSystemWatcher"></param>
    public
#if ASYNC
        async Task ProcessFileAsync
#else
        void ProcessFile
#endif
        (string file, bool fromFileSystemWatcher)
    {
#if ASYNC
        await
#endif
            ProcessFile(file, NamespaceCodeElementsType.All, ClassCodeElementsType.All, false, fromFileSystemWatcher);
    }

    public bool IsToIndexedFolder(string pathFile, bool alsoEnds)
    {
        var uf = UnindexableFiles.uf;
        var end2 = false;

        if (!CsFileFilter.AllowOnly(pathFile, endArgs, containsArgs, ref end2, alsoEnds))
        {
            if (end2)
                uf.unindexablePathEndsFiles.Add(pathFile);
            else
                uf.unindexablePathPartsFiles.Add(pathFile);

            return false;
        }

        if (CA.StartWith(pathStarts, pathFile) != null)
        {
            uf.unindexablePathStartsFiles.Add(pathFile);

            return false;
        }

        return true;
    }

    public bool IsToIndexed(string pathFile)
    {
        #region All 4 for which is checked

        if (CA.ReturnWhichContainsIndexes(endsOther, pathFile).Count > 0) return false;

        var uf = UnindexableFiles.uf;

        var fn = Path.GetFileName(pathFile);
        if (CA.ReturnWhichContainsIndexes(fileNames, fn).Count > 0)
        {
            uf.unindexableFileNamesFiles.Add(pathFile);

            return false;
        }

        #endregion

        return IsToIndexedFolder(pathFile, true);
    }

    /// <summary>
    ///     SourceCodeIndexerRoslyn.ProcessFile
    ///     True if file wasnt indexed yet
    ///     False is file was already indexed
    /// </summary>
    /// <param name="pathFile"></param>
    /// <param name="namespaceCodeElementsType"></param>
    /// <param name="classCodeElementsType"></param>
    /// <param name="tree"></param>
    /// <param name="root"></param>
    /// <param name="removeRegions"></param>
    private
#if ASYNC
        async Task<ProcessFileBoolResult> ProcessFileBool
#else
        ProcessFileBoolResult ProcessFileBool
#endif
        (string pathFile, NamespaceCodeElementsType namespaceCodeElementsType,
            ClassCodeElementsType classCodeElementsType, bool removeRegions, bool fromFileSystemWatcher)
    {
        SyntaxTree tree = null;
        CompilationUnitSyntax root = null;

        // A2 must be false otherwise read file twice
        if (!File.Exists(pathFile)) return new ProcessFileBoolResult();

        if (!isCallingIsToIndexed)
            if (!IsToIndexed(pathFile))
                return new ProcessFileBoolResult();

        if (!linesWithContent.ContainsKey(pathFile) || isLoadingFromFile)
        {
            IList<NamespaceCodeElementsType> namespaceCodeElementsAll =
                EnumHelper.GetValues<NamespaceCodeElementsType>();
            IList<ClassCodeElementsType> classodeElementsAll = EnumHelper.GetValues<ClassCodeElementsType>();
            var namespaceCodeElementsKeywords = new List<string>();
            var classCodeElementsKeywords = new List<string>();
            var fileContent = string.Empty;

            var linesWithContent = Instance.linesWithContent;
            //var linesWithNonTextContent = SourceCodeIndexerRoslyn.Instance.linesWithNonTextContent;
            var linesWithIndexes = Instance.linesWithIndexes;

            List<string> lines = null;
            if (fromFileSystemWatcher)
            {
                lines = (
#if ASYNC
                    await
#endif
                        File.ReadAllLinesAsync(pathFile)).ToList();
            }
            else
            {
                if (File.Exists(pathFile))
                {
                    if (linesWithContent.ContainsKey(pathFile))
                    {
                        lines = linesWithContent[pathFile];

                        if (pathFile.EndsWith("\\SH.cs"))
                        {
                        }

                        var between = GetLinesBetween(linesWithIndexes[pathFile], true);

                        for (var i = 0; i < between.Count; i++) lines.Insert(between[i], string.Empty);
                    }
                    else
                    {
                        (
#if ASYNC
                            await
#endif
                                File.ReadAllLinesAsync(pathFile)).ToList();
                    }
                }
            }

            fileContent = string.Join(Environment.NewLine, lines);
            if (pathFile.EndsWith(@"\RunAutomatically2.cs"))
            {
                var gf = CompareFilesPaths.GetFile(CompareExt.cs, 1);
                await File.WriteAllTextAsync(gf, fileContent);
            }

            var linesAll = lines; // SHGetLines.GetLines(fileContent);
            // nechápu proč to obaluji mezerou ale nevadí
            linesAll = CA.WrapWith(linesAll, AllStringsSE.space).ToList();
            var FullFileIndex = new List<int>();
            for (var i = linesAll.Count - 1; i >= 0; i--)
            {
                if (linesAll[i].Contains("dates.Clear()"))
                {
                }

                var item = linesAll[i];
                // nemůžu kontrolovat jen pokud nemá písmeno - do FasterStartup musím vložit i zárovky jinak bez nich nejsem schopen poskládat opět vstupní soubor
                //) //

                /*
Na jednu stranu potřebuji uložit výstupní soubor i se závorkami

                 */


                if (item.Trim() == string.Empty) //!SH.HasLetter(item))
                    linesAll.RemoveAt(i);
                else
                    //var b1 = item.Trim() != String.Empty;
                    //if(b1)
                    //{
                    //}
                    // Přidám pokud má nějaké písmeno
                    FullFileIndex.Add(i);
            }

            FullFileIndex.Reverse();
            ThrowEx.DifferentCountInLists("lines", linesAll.Count, "FullFileIndex", FullFileIndex.Count);
            // Probably was add on background again due to watch for changes
            if (this.linesWithContent.ContainsKey(pathFile)) this.linesWithContent.Remove(pathFile);
            this.linesWithContent.Add(pathFile, linesAll);
            if (linesWithIndexes.ContainsKey(pathFile)) linesWithIndexes.Remove(pathFile);

            // Přidám řádky jež mají nějaké písmeno
            linesWithIndexes.AddIfNotExists(pathFile, FullFileIndex);
            foreach (var item in namespaceCodeElementsAll)
                if (namespaceCodeElementsType.HasFlag(item))
                    namespaceCodeElementsKeywords.Add(SH.WrapWith(item.ToString().ToLower(), AllStringsSE.space));

            foreach (var item in namespaceCodeElementsKeywords)
            {
                var elementTypeString = item.Trim();
                var namespaceCodeElementType =
                    (NamespaceCodeElementsType)Enum.Parse(namespaceCodeElementsType2, item, true);
                List<int> indexes;
                var linesCodeElements = CA.ReturnWhichContains(linesAll, item, out indexes);
                for (var i = 0; i < linesCodeElements.Count; i++)
                {
                    var lineCodeElements = linesCodeElements[i];
                    var namespaceElementName = SH.WordAfter(lineCodeElements,
                        e2sNamespaceCodeElements[namespaceCodeElementType]);
                    if (namespaceElementName.Length > 1)
                        if (char.IsUpper(namespaceElementName[0]))
                        {
                            var element = new NamespaceCodeElement
                            {
                                Index = FullFileIndex[indexes[i]], Name = namespaceElementName,
                                Type = namespaceCodeElementType
                            };
                            DictionaryHelperSE.AddOrCreate(namespaceCodeElements, pathFile, element);
                        }
                }
            }

            var classCodeElementsTypeToFind = ClassCodeElementsType.All;
            if (classCodeElementsType.HasFlag(ClassCodeElementsType.All))
                classCodeElementsTypeToFind |= ClassCodeElementsType.Method;

            tree = CSharpSyntaxTree.ParseText(fileContent);
            root = (CompilationUnitSyntax)tree.GetRoot();
            var c = classCodeElements;
            var ns = root.DescendantNodes();
            IList<NamespaceDeclarationSyntax> namespaces = ns.OfType<NamespaceDeclarationSyntax>().ToList();
            foreach (var nameSpace in namespaces)
                if (classCodeElementsTypeToFind.HasFlag(ClassCodeElementsType.Method))
                {
                    var ancestor = nameSpace;
                    AddMethodsFrom(ancestor, pathFile);
                }

            AddMethodsFrom(root, pathFile);
            return new ProcessFileBoolResult { indexed = true, tree = tree, root = root };
        }

        return new ProcessFileBoolResult();
    }

    private List<int> GetLinesBetween(List<int> i2, bool fromZeroIndex)
    {
        var l = new List<int>();
        i2.Sort();

        if (fromZeroIndex)
            if (i2[0] != 0)
                i2.Insert(0, 0);

        for (var i = 0; i < i2.Count - 1; i++)
        {
            var a1 = i2[i] + 1;
            var a2 = i2[i + 1];
            if (a1 != a2)
                for (var y = a1; y < a2; y++)
                    l.Add(y);
        }

        return l;
    }
}