namespace SunamoRoslyn;

using static CsFileFilterRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    public EndArgs? EndArgs { get; set; } = null;

    public ContainsArgs? ContainsArgs { get; set; } = null;

    public List<string>? FileNames { get; set; } = null;

    public List<string>? FileNamesExactly { get; set; } = null;

    public List<string>? PathStarts { get; set; } = null;

    public List<string>? EndsOther { get; set; } = null;

    public List<string>? ContainsOther { get; set; } = null;

    public
        async Task
    ProcessFile(string pathFile, NamespaceCodeElementsType namespaceCodeElementsType, ClassCodeElementsType classCodeElementsType, bool isRemovingRegions, bool isFromFileSystemWatcher)
    {
        ProcessFileBoolResult result =
         await
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

    public void RemoveFile(string pathFile)
    {
        LinesWithContent.Remove(pathFile);
        LinesWithIndexes.Remove(pathFile);
        sourceFileTrees.Remove(pathFile);
        classCodeElements.Remove(pathFile);
        namespaceCodeElements.Remove(pathFile);
    }

    // Syntax root is the same as root - contains all code (include usings).
    // Keys are also present in LinesWithContent and LinesWithIndexes.
    FsWatcherDictionary<string, SourceFileTree> sourceFileTrees = new FsWatcherDictionary<string, SourceFileTree>();

    // In key is full path, in value lines with letter content.
    public IDictionary<string, List<string>> LinesWithContent { get; set; } = new Dictionary<string, List<string>>();

    // Contains lines that have no letter content (indexes of empty/non-text lines).
    public IDictionary<string, List<int>> LinesWithIndexes { get; set; } = new Dictionary<string, List<int>>();

    public void Nuke()
    {
        LinesWithContent.Clear();
        LinesWithIndexes.Clear();
        sourceFileTrees.Clear();
        namespaceCodeElements.Clear();
        classCodeElements.Clear();
    }

    static Type NamespaceCodeElementsTypeType = typeof(NamespaceCodeElementsType);

    static Type ClassCodeElementsTypeType = typeof(ClassCodeElementsType);

    // In key are full file path, in value parsed code elements.
    FsWatcherDictionary<string, List<NamespaceCodeElement>> namespaceCodeElements = new FsWatcherDictionary<string, List<NamespaceCodeElement>>();

    // In key are full file path, in value parsed code elements.
    internal FsWatcherDictionary<string, List<ClassCodeElement>> classCodeElements = new FsWatcherDictionary<string, List<ClassCodeElement>>();

    NamespaceCodeElementsType allNamespaceCodeElements = NamespaceCodeElementsType.Class;

    ClassCodeElementsType allClassCodeElements = ClassCodeElementsType.Method;

    public static Dictionary<NamespaceCodeElementsType, string> E2sNamespaceCodeElements = EnumHelper.EnumToString<NamespaceCodeElementsType>(NamespaceCodeElementsTypeType);

    public static Dictionary<ClassCodeElementsType, string> E2sClassCodeElements = EnumHelper.EnumToString<ClassCodeElementsType>(ClassCodeElementsTypeType);

    public FileSystemWatchers? Watchers { get; set; } = null;

    public bool IsLoadingFromFile { get; set; } = false;

    public bool IsIndexed(string pathFile)
    {
        if (IsLoadingFromFile)
        {
            return false;
        }

        return LinesWithContent.ContainsKey(pathFile);
    }

    public static SourceCodeIndexerRoslyn Instance = new SourceCodeIndexerRoslyn();

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

    public
        async Task
    ProcessAllCodeElementsInFiles(string file, bool isFromFileSystemWatcher, bool isRemovingRegions = false)
    {
        await
        ProcessFile(file, allNamespaceCodeElements, allClassCodeElements, isRemovingRegions, isFromFileSystemWatcher);
    }

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
