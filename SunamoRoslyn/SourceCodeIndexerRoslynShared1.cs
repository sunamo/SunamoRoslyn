namespace SunamoRoslyn;

/// <summary>
/// Contains the core file processing logic for SourceCodeIndexerRoslyn.
/// </summary>
public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// Processes a file and determines if it should be indexed.
    /// Returns true if file was not indexed yet, false if already indexed.
    /// </summary>
    /// <param name="pathFile">Full path to the source file.</param>
    /// <param name="namespaceCodeElementsType">Types of namespace-level code elements to extract.</param>
    /// <param name="classCodeElementsType">Types of class-level code elements to extract.</param>
    /// <param name="isRemovingRegions">Whether to remove region directives during processing.</param>
    /// <param name="isFromFileSystemWatcher">Whether the call originates from a file system watcher event.</param>
    /// <returns>Result indicating whether the file was indexed and providing the syntax tree.</returns>
    private
#if ASYNC
        async Task<ProcessFileBoolResult> ProcessFileBool
#else
    ProcessFileBoolResult ProcessFileBool
#endif
    (string pathFile, NamespaceCodeElementsType namespaceCodeElementsType, ClassCodeElementsType classCodeElementsType, bool isRemovingRegions, bool isFromFileSystemWatcher)
    {
        SyntaxTree? tree = null;
        CompilationUnitSyntax? root = null;
        if (!File.Exists(pathFile))
        {
            return new ProcessFileBoolResult();
        }

        if (!IsCallingIsToIndexed)
        {
            if (!IsToIndexed(pathFile))
            {
                return new ProcessFileBoolResult();
            }
        }

        if (!LinesWithContent.ContainsKey(pathFile) || IsLoadingFromFile)
        {
            IList<NamespaceCodeElementsType> namespaceCodeElementsAll = EnumHelper.GetValues<NamespaceCodeElementsType>();
            IList<ClassCodeElementsType> classCodeElementsAll = EnumHelper.GetValues<ClassCodeElementsType>();
            List<string> namespaceCodeElementsKeywords = new List<string>();
            List<string> classCodeElementsKeywords = new List<string>();
            string fileContent = string.Empty;
            var linesWithContent = SourceCodeIndexerRoslyn.Instance.LinesWithContent;
            var linesWithIndexes = SourceCodeIndexerRoslyn.Instance.LinesWithIndexes;
            List<string>? lines = null;
            if (isFromFileSystemWatcher)
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

                        var between = GetLinesBetween(linesWithIndexes[pathFile], true);
                        for (int i = 0; i < between.Count; i++)
                        {
                            lines.Insert(between[i], String.Empty);
                        }
                    }
                    else
                    {
                        lines = (
#if ASYNC
                        await
#endif
                        File.ReadAllLinesAsync(pathFile)).ToList();
                    }
                }
            }

            fileContent = string.Join(Environment.NewLine, lines!);
            List<string> linesAll = lines!;
            linesAll = CA.WrapWith(linesAll, " ").ToList();
            List<int> fullFileIndex = new List<int>();
            for (int i = linesAll.Count - 1; i >= 0; i--)
            {
                string lineText = linesAll[i];
                if (lineText.Trim() == String.Empty)
                {
                    linesAll.RemoveAt(i);
                }
                else
                {
                    fullFileIndex.Add(i);
                }
            }

            fullFileIndex.Reverse();
            ThrowEx.DifferentCountInLists("lines", linesAll.Count, "fullFileIndex", fullFileIndex.Count);
            if (this.LinesWithContent.ContainsKey(pathFile))
            {
                this.LinesWithContent.Remove(pathFile);
            }

            this.LinesWithContent.Add(pathFile, linesAll);
            if (linesWithIndexes.ContainsKey(pathFile))
            {
                linesWithIndexes.Remove(pathFile);
            }

            linesWithIndexes.AddIfNotExists(pathFile, fullFileIndex);
            foreach (var item in namespaceCodeElementsAll)
            {
                if (namespaceCodeElementsType.HasFlag(item))
                {
                    namespaceCodeElementsKeywords.Add(SH.WrapWith(item.ToString().ToLower(), " "));
                }
            }

            foreach (var item in namespaceCodeElementsKeywords)
            {
                string elementTypeString = item.Trim();
                NamespaceCodeElementsType namespaceCodeElementType = (NamespaceCodeElementsType)Enum.Parse(NamespaceCodeElementsTypeType, item, true);
                List<int> indexes;
                List<string> linesCodeElements = CA.ReturnWhichContains(linesAll, item, out indexes);
                for (int i = 0; i < linesCodeElements.Count; i++)
                {
                    var lineCodeElements = linesCodeElements[i];
                    string namespaceElementName = SH.WordAfter(lineCodeElements, E2sNamespaceCodeElements[namespaceCodeElementType]);
                    if (namespaceElementName.Length > 1)
                    {
                        if (char.IsUpper(namespaceElementName[0]))
                        {
                            NamespaceCodeElement element = new NamespaceCodeElement()
                            {
                                Index = fullFileIndex[indexes[i]],
                                Name = namespaceElementName,
                                Type = namespaceCodeElementType
                            };
                            DictionaryHelper.AddOrCreate<string, NamespaceCodeElement>(namespaceCodeElements, pathFile, element);
                        }
                    }
                }
            }

            ClassCodeElementsType classCodeElementsTypeToFind = ClassCodeElementsType.All;
            if (classCodeElementsType.HasFlag(ClassCodeElementsType.All))
            {
                classCodeElementsTypeToFind |= ClassCodeElementsType.Method;
            }

            tree = CSharpSyntaxTree.ParseText(fileContent);
            root = (CompilationUnitSyntax)tree.GetRoot();
            var classCodeElementsDict = classCodeElements;
            var descendantNodes = root.DescendantNodes();
            IList<NamespaceDeclarationSyntax> namespaces = descendantNodes.OfType<NamespaceDeclarationSyntax>().ToList();
            foreach (var nameSpace in namespaces)
            {
                if (classCodeElementsTypeToFind.HasFlag(ClassCodeElementsType.Method))
                {
                    var ancestor = nameSpace;
                    AddMethodsFrom(ancestor, pathFile);
                }
            }

            AddMethodsFrom(root, pathFile);
            return new ProcessFileBoolResult
            {
                Indexed = true,
                Tree = tree,
                Root = root
            };
        }

        return new ProcessFileBoolResult();
    }

    /// <summary>
    /// Gets the line indices between the given sorted indices, used for reconstructing empty lines.
    /// </summary>
    /// <param name="indices">List of line indices to find gaps between.</param>
    /// <param name="isFromZeroIndex">Whether to include index 0 as the starting point.</param>
    /// <returns>List of line indices that fall between the provided indices.</returns>
    private List<int> GetLinesBetween(List<int> indices, bool isFromZeroIndex)
    {
        List<int> list = new List<int>();
        indices.Sort();
        if (isFromZeroIndex)
        {
            if (indices[0] != 0)
            {
                indices.Insert(0, 0);
            }
        }

        for (int i = 0; i < indices.Count - 1; i++)
        {
            var currentIndex = indices[i] + 1;
            var nextIndex = indices[i + 1];
            if (currentIndex != nextIndex)
            {
                for (int j = currentIndex; j < nextIndex; j++)
                {
                    list.Add(j);
                }
            }
        }

        return list;
    }
}
