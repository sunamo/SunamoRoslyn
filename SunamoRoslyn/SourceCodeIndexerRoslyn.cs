namespace SunamoRoslyn;

using static CsFileFilterRoslyn;

/// <summary>
/// Indexes source code files and extracts code elements using Roslyn.
/// </summary>
public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// Filter arguments for path endings. Removes files from index in ProcessFile that match these criteria.
    /// </summary>
    public EndArgs? EndArgs { get; set; } = null;

    /// <summary>
    /// Filter arguments for path content matching.
    /// </summary>
    public ContainsArgs? ContainsArgs { get; set; } = null;

    /// <summary>
    /// File name patterns to exclude from indexing.
    /// </summary>
    public List<string>? FileNames { get; set; } = null;

    /// <summary>
    /// Exact file names to exclude from indexing.
    /// </summary>
    public List<string>? FileNamesExactly { get; set; } = null;

    /// <summary>
    /// Path prefixes to exclude from indexing.
    /// </summary>
    public List<string>? PathStarts { get; set; } = null;

    /// <summary>
    /// Other path endings to exclude from indexing.
    /// </summary>
    public List<string>? EndsOther { get; set; } = null;

    /// <summary>
    /// Other path content patterns to exclude from indexing.
    /// </summary>
    public List<string>? ContainsOther { get; set; } = null;

    /// <summary>
    /// Processes a source file and indexes its code elements.
    /// Public only for testing purposes.
    /// </summary>
    /// <param name="pathFile">Full path to the source file.</param>
    /// <param name="namespaceCodeElementsType">Types of namespace-level code elements to extract.</param>
    /// <param name="classCodeElementsType">Types of class-level code elements to extract.</param>
    /// <param name="isRemovingRegions">Whether to remove region directives during processing.</param>
    /// <param name="isFromFileSystemWatcher">Whether the call originates from a file system watcher event.</param>
    public
#if ASYNC
        async Task
#else
    void
#endif
    ProcessFile(string pathFile, NamespaceCodeElementsType namespaceCodeElementsType, ClassCodeElementsType classCodeElementsType, bool isRemovingRegions, bool isFromFileSystemWatcher)
    {
        ProcessFileBoolResult result =
#if ASYNC
         await
#endif
        ProcessFileBool(pathFile, namespaceCodeElementsType, classCodeElementsType, isRemovingRegions, isFromFileSystemWatcher);
        SyntaxTree? syntaxTree = result.Tree;
        CompilationUnitSyntax? root = result.Root;
        if (result.Indexed)
        {
            if (sourceFileTrees.ContainsKey(pathFile))
            {
                sourceFileTrees.Remove(pathFile);
            }

            if (!sourceFileTrees.ContainsKey(pathFile))
            {
                sourceFileTrees.Add(pathFile, new SourceFileTree { Root = root, Tree = syntaxTree });
            }
            else
            {
                sourceFileTrees[pathFile] = new SourceFileTree
                {
                    Root = root,
                    Tree = syntaxTree
                };
            }
        }
        else
        {
            RemoveFile(pathFile);
        }
    }

    /// <summary>
    /// Removes a file and all its indexed data from the indexer.
    /// </summary>
    /// <param name="pathFile">Full path to the file to remove.</param>
    public void RemoveFile(string pathFile)
    {
        LinesWithContent.Remove(pathFile);
        LinesWithIndexes.Remove(pathFile);
        sourceFileTrees.Remove(pathFile);
        classCodeElements.Remove(pathFile);
        namespaceCodeElements.Remove(pathFile);
    }

    /// <summary>
    /// Syntax root is the same as root - contains all code (include usings).
    /// Keys are also present in LinesWithContent and LinesWithIndexes.
    /// </summary>
    FsWatcherDictionary<string, SourceFileTree> sourceFileTrees = new FsWatcherDictionary<string, SourceFileTree>();

    /// <summary>
    /// In key is full path, in value lines with letter content.
    /// </summary>
    public IDictionary<string, List<string>> LinesWithContent { get; set; } = new Dictionary<string, List<string>>();

    /// <summary>
    /// Contains lines that have no letter content (indexes of empty/non-text lines).
    /// </summary>
    public IDictionary<string, List<int>> LinesWithIndexes { get; set; } = new Dictionary<string, List<int>>();

    /// <summary>
    /// Clears all indexed data.
    /// </summary>
    public void Nuke()
    {
        LinesWithContent.Clear();
        LinesWithIndexes.Clear();
        sourceFileTrees.Clear();
        namespaceCodeElements.Clear();
        classCodeElements.Clear();
    }

    /// <summary>
    /// Type of NamespaceCodeElementsType enum.
    /// </summary>
    static Type NamespaceCodeElementsTypeType = typeof(NamespaceCodeElementsType);

    /// <summary>
    /// Type of ClassCodeElementsType enum.
    /// </summary>
    static Type ClassCodeElementsTypeType = typeof(ClassCodeElementsType);

    /// <summary>
    /// In key are full file path, in value parsed code elements.
    /// </summary>
    FsWatcherDictionary<string, List<NamespaceCodeElement>> namespaceCodeElements = new FsWatcherDictionary<string, List<NamespaceCodeElement>>();

    /// <summary>
    /// In key are full file path, in value parsed code elements.
    /// </summary>
    internal FsWatcherDictionary<string, List<ClassCodeElement>> classCodeElements = new FsWatcherDictionary<string, List<ClassCodeElement>>();

    /// <summary>
    /// All namespace code element types combined.
    /// </summary>
    NamespaceCodeElementsType allNamespaceCodeElements = NamespaceCodeElementsType.Class;

    /// <summary>
    /// All class code element types combined.
    /// </summary>
    ClassCodeElementsType allClassCodeElements = ClassCodeElementsType.Method;

    /// <summary>
    /// Maps NamespaceCodeElementsType enum values to their C# keyword strings.
    /// </summary>
    public static Dictionary<NamespaceCodeElementsType, string> E2sNamespaceCodeElements = EnumHelper.EnumToString<NamespaceCodeElementsType>(NamespaceCodeElementsTypeType);

    /// <summary>
    /// Maps ClassCodeElementsType enum values to their C# keyword strings.
    /// </summary>
    public static Dictionary<ClassCodeElementsType, string> E2sClassCodeElements = EnumHelper.EnumToString<ClassCodeElementsType>(ClassCodeElementsTypeType);

    /// <summary>
    /// File system watchers for monitoring file changes.
    /// </summary>
    public FileSystemWatchers? Watchers { get; set; } = null;

    /// <summary>
    /// Whether the indexer is currently loading from a file.
    /// </summary>
    public bool IsLoadingFromFile { get; set; } = false;

    /// <summary>
    /// Checks whether a file has been indexed.
    /// </summary>
    /// <param name="pathFile">Full path to the file.</param>
    /// <returns>True if the file is indexed; false if loading from file or not indexed.</returns>
    public bool IsIndexed(string pathFile)
    {
        if (IsLoadingFromFile)
        {
            return false;
        }

        return LinesWithContent.ContainsKey(pathFile);
    }

    /// <summary>
    /// Singleton instance of the indexer.
    /// </summary>
    public static SourceCodeIndexerRoslyn Instance = new SourceCodeIndexerRoslyn();

    /// <summary>
    /// Private constructor for singleton pattern.
    /// </summary>
    private SourceCodeIndexerRoslyn()
    {
        var arr = Enum.GetValues(typeof(NamespaceCodeElementsType));
        foreach (NamespaceCodeElementsType item in arr)
        {
            if (item != NamespaceCodeElementsType.Nope && item != NamespaceCodeElementsType.Class)
            {
                allNamespaceCodeElements |= item;
            }
        }
    }

    /// <summary>
    /// Processes all code element types in a file.
    /// </summary>
    /// <param name="file">Full path to the source file.</param>
    /// <param name="isFromFileSystemWatcher">Whether the call originates from a file system watcher event.</param>
    /// <param name="isRemovingRegions">Whether to remove region directives during processing.</param>
    public
#if ASYNC
        async Task
#else
    void
#endif
    ProcessAllCodeElementsInFiles(string file, bool isFromFileSystemWatcher, bool isRemovingRegions = false)
    {
#if ASYNC
        await
#endif
        ProcessFile(file, allNamespaceCodeElements, allClassCodeElements, isRemovingRegions, isFromFileSystemWatcher);
    }

    /// <summary>
    /// Extracts method declarations from a syntax node and adds them to class code elements.
    /// </summary>
    /// <param name="ancestor">The syntax node to search for methods.</param>
    /// <param name="pathFile">Full path to the source file.</param>
    private void AddMethodsFrom(CSharpSyntaxNode ancestor, string pathFile)
    {
        var cls = ancestor.ChildNodes().OfType<ClassDeclarationSyntax>().ToList();
        foreach (var classEl in cls)
        {
            var methods = classEl.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            foreach (MethodDeclarationSyntax method in methods)
            {
                var text = method.Span;
                var location = method.GetLocation();
                FileLinePositionSpan fileLinePositionSpan = location.GetLineSpan();
                string methodName = method.Identifier.ToString();
                ClassCodeElement element = new ClassCodeElement()
                {
                    Index = fileLinePositionSpan.StartLinePosition.Line,
                    Name = methodName,
                    Type = ClassCodeElementsType.Method,
                    From = text.Start,
                    To = text.End,
                    Length = text.Length,
                    Member = method
                };
                DictionaryHelper.AddOrCreate<string, ClassCodeElement, object>(classCodeElements, pathFile, element);
            }
        }
    }
}
