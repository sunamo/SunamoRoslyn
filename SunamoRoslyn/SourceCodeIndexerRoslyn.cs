namespace SunamoRoslyn;

using static CsFileFilterRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    // Ve LoadAllFiles mi odstraní soubory jež nebyly ve selectMoreFolders
    // Z indexu odstraním ty z EndArgs aj. souvisejících v ProcessFile kde odjakživa se text tímto pracovalo
    public EndArgs endArgs = null;
    public ContainsArgs containsArgs = null;
    public List<string> fileNames = null;
    public List<string> fileNamesExactly = null;
    public List<string> pathStarts = null;
    public List<string> endsOther = null;
    public List<string> containsOther = null;
    /// <summary>
    /// Je veřejná jen kvůli testu
    /// </summary>
    /// <param name = "pathFile"></param>
    /// <param name = "namespaceCodeElementsType"></param>
    /// <param name = "classCodeElementsType"></param>
    /// <param name = "removeRegions"></param>
    /// <param name = "fromFileSystemWatcher"></param>
    public 
#if ASYNC
        async Task
#else
    void 
#endif
    ProcessFile(string pathFile, NamespaceCodeElementsType namespaceCodeElementsType, ClassCodeElementsType classCodeElementsType, bool removeRegions, bool fromFileSystemWatcher)
    {
        ProcessFileBoolResult result = null;
        result = 
#if ASYNC
         await
#endif
        ProcessFileBool(pathFile, namespaceCodeElementsType, classCodeElementsType, removeRegions, fromFileSystemWatcher);
        SyntaxTree _tree = result.tree;
        CompilationUnitSyntax root = result.root;
        if (result.indexed)
        {
            if (sourceFileTrees.ContainsKey(pathFile))
            {
                sourceFileTrees.Remove(pathFile);
            }
            else
            {
            // Watcher.Start I can call after loading all files - otherwise there are million of events and app never start
            // Check whether folder is already indexing
            //if (!watchers.IsIndexindDirectory(pathFile))
            //{
            //watchers.Start(Path.GetDirectoryName( pathFile));
            //}
            }

            if (!sourceFileTrees.ContainsKey(pathFile))
            {
                sourceFileTrees.Add(pathFile, new SourceFileTree { root = root, tree = _tree });
            }
            else
            {
                sourceFileTrees[pathFile] = new SourceFileTree
                {
                    root = root,
                    tree = _tree
                };
            }
        }
        else
        {
            RemoveFile(pathFile);
        }
    }

    public void RemoveFile(string t)
    {
        linesWithContent.Remove(t);
        linesWithIndexes.Remove(t);
        sourceFileTrees.Remove(t);
        classCodeElements.Remove(t);
        namespaceCodeElements.Remove(t);
    // watcher I cant stop, its one for all!!
    //if (!fromFileSystemWatcher)
    //{
    //    // will be raise in FileSystemWatchers
    //    watchers.Stop(t);
    //}
    }

    /// <summary>
    /// NO TODRIVE - používají se jen klíče a ty jsou i v linesWithContent, linesWithIndexes
    /// Syntax root is the same as root - contains all code (include usings)
    /// </summary>
    FsWatcherDictionary<string, SourceFileTree> sourceFileTrees = new FsWatcherDictionary<string, SourceFileTree>();
    Type type = typeof(SourceCodeIndexerRoslyn);
    //public Dictionary<string, TU<string, int>> foundedLines = new Dictionary<string, TU<string, int>>();
    /// <summary>
    /// TODRIVE
    /// In A1 is full path, in A2 lines with letter content
    /// </summary>
    public IDictionary<string, List<string>> linesWithContent = new Dictionary<string, List<string>>();
    /// <summary>
    /// TODRIVE
    /// Obsahuje řádky které nemají žádné písmeno
    /// </summary>
    public IDictionary<string, List<int>> linesWithIndexes = new Dictionary<string, List<int>>();
    //public IDictionary<string, List<int>> linesWithNonTextContent = new Dictionary<string, List<int>>();
    public void Nuke()
    {
        linesWithContent.Clear();
        linesWithIndexes.Clear();
        sourceFileTrees.Clear();
        namespaceCodeElements.Clear();
        classCodeElements.Clear();
    }

    /// <summary>
    /// Type of NamespaceCodeElementsType
    /// </summary>
    static Type namespaceCodeElementsType2 = typeof(NamespaceCodeElementsType);
    static Type classCodeElementsType2 = typeof(ClassCodeElementsType);
    /// <summary>
    /// In key are full file path, in value parsed code elements
    /// </summary>
    FsWatcherDictionary<string, List<NamespaceCodeElement>> namespaceCodeElements = new FsWatcherDictionary<string, List<NamespaceCodeElement>>();
    /// <summary>
    /// In key are full file path, in value parsed code elements
    /// </summary>
    internal FsWatcherDictionary<string, List<ClassCodeElement>> classCodeElements = new FsWatcherDictionary<string, List<ClassCodeElement>>();
    /// <summary>
    /// All code elements
    /// </summary>
    NamespaceCodeElementsType allNamespaceCodeElements = NamespaceCodeElementsType.Class;
    ClassCodeElementsType allClassCodeElements = ClassCodeElementsType.Method;
    /// <summary>
    /// Map NamespaceCodeElementsType to keywords used in C#
    /// </summary>
    public static Dictionary<NamespaceCodeElementsType, string> e2sNamespaceCodeElements = EnumHelper.EnumToString<NamespaceCodeElementsType>(namespaceCodeElementsType2);
    public static Dictionary<ClassCodeElementsType, string> e2sClassCodeElements = EnumHelper.EnumToString<ClassCodeElementsType>(classCodeElementsType2);
    public FileSystemWatchers watchers = null;
    public bool isLoadingFromFile = false;
    public bool IsIndexed(string pathFile)
    {
        if (isLoadingFromFile)
        {
            return false;
        }

        return linesWithContent.ContainsKey(pathFile);
    }

    public static SourceCodeIndexerRoslyn Instance = new SourceCodeIndexerRoslyn();
    /// <summary>
    /// 15-6-20 Make it private & singleton
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
    // TODO: dělám to teď na asynchronní. nebudu řešit ještě watchery
    //watchers = new FileSystemWatchers( ProcessFile, RemoveFile);
    }

    public 
#if ASYNC
        async Task
#else
    void 
#endif
    ProcessAllCodeElementsInFiles(string file, bool fromFileSystemWatcher, bool removeRegions = false)
    {
#if ASYNC
        await
#endif
        ProcessFile(file, allNamespaceCodeElements, allClassCodeElements, removeRegions, fromFileSystemWatcher);
    }

    private void AddMethodsFrom(CSharpSyntaxNode ancestor, string pathFile)
    {
        // ancestor.DescendantNodes() returns all recursive
        //var cls = ancestor.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
        var cls = ancestor.ChildNodes().OfType<ClassDeclarationSyntax>().ToList();
        foreach (var classEl in cls)
        {
            var methods = classEl.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            foreach (MethodDeclarationSyntax method2 in methods)
            {
                var method = method2;
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