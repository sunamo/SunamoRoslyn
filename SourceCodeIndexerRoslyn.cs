namespace SunamoRoslyn;

using static CsFileFilter;

public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    ///     Type of NamespaceCodeElementsType
    /// </summary>
    private static readonly Type namespaceCodeElementsType2 = typeof(NamespaceCodeElementsType);

    private static readonly Type classCodeElementsType2 = typeof(ClassCodeElementsType);

    /// <summary>
    ///     Map NamespaceCodeElementsType to keywords used in C#
    /// </summary>
    public static Dictionary<NamespaceCodeElementsType, string> e2sNamespaceCodeElements =
        EnumHelper.EnumToString<NamespaceCodeElementsType>(namespaceCodeElementsType2);

    public static Dictionary<ClassCodeElementsType, string> e2sClassCodeElements =
        EnumHelper.EnumToString<ClassCodeElementsType>(classCodeElementsType2);

    public static SourceCodeIndexerRoslyn Instance = new();
    private readonly ClassCodeElementsType allClassCodeElements = ClassCodeElementsType.Method;

    /// <summary>
    ///     All code elements
    /// </summary>
    private readonly NamespaceCodeElementsType allNamespaceCodeElements = NamespaceCodeElementsType.Class;

    /// <summary>
    ///     In key are full file path, in value parsed code elements
    /// </summary>
    public FsWatcherDictionary<string, List<ClassCodeElement>> classCodeElements = new();

    public List<string> containsOther = null;

    public List<string> endsOther = null;

    public bool isLoadingFromFile = false;

    //public Dictionary<string, TU<string, int>> foundedLines = new Dictionary<string, TU<string, int>>();
    /// <summary>
    ///     TODRIVE
    ///     In A1 is full path, in A2 lines with letter content
    /// </summary>
    public IDictionary<string, List<string>> linesWithContent = new Dictionary<string, List<string>>();

    /// <summary>
    ///     TODRIVE
    ///     Obsahuje řádky které nemají žádné písmeno
    /// </summary>
    public IDictionary<string, List<int>> linesWithIndexes = new Dictionary<string, List<int>>();

    /// <summary>
    ///     In key are full file path, in value parsed code elements
    /// </summary>
    public FsWatcherDictionary<string, List<NamespaceCodeElement>> namespaceCodeElements = new();

    /// <summary>
    ///     NO TODRIVE - používají se jen klíče a ty jsou i v linesWithContent, linesWithIndexes
    ///     Syntax root is the same as root - contains all code (include usings)
    /// </summary>
    public FsWatcherDictionary<string, SourceFileTree> sourceFileTrees = new();

    private Type type = typeof(SourceCodeIndexerRoslyn);

    public FileSystemWatchers watchers = null;


    /// <summary>
    ///     15-6-20 Make it private & singleton
    /// </summary>
    private SourceCodeIndexerRoslyn()
    {
        var arr = Enum.GetValues(typeof(NamespaceCodeElementsType));
        foreach (NamespaceCodeElementsType item in arr)
            if (item != NamespaceCodeElementsType.Nope && item != NamespaceCodeElementsType.Class)
                allNamespaceCodeElements |= item;

        // TODO: dělám to teď na asynchronní. nebudu řešit ještě watchery
        //watchers = new FileSystemWatchers( ProcessFile, RemoveFile);
    }
    //public IDictionary<string, List<int>> linesWithNonTextContent = new Dictionary<string, List<int>>();

    public void Nuke()
    {
        linesWithContent.Clear();
        linesWithIndexes.Clear();
        sourceFileTrees.Clear();
        namespaceCodeElements.Clear();
        classCodeElements.Clear();
    }

    public bool IsIndexed(string pathFile)
    {
        if (isLoadingFromFile) return false;

        return linesWithContent.ContainsKey(pathFile);
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
            foreach (var method2 in methods)
            {
                var method = method2;
                var s = method.Span;
                var location = method.GetLocation();
                var fileLinePositionSpan = location.GetLineSpan();
                var methodName = method.Identifier.ToString();
                var element = new ClassCodeElement
                {
                    Index = fileLinePositionSpan.StartLinePosition.Line, Name = methodName,
                    Type = ClassCodeElementsType.Method, From = s.Start, To = s.End, Length = s.Length, Member = method
                };

                DictionaryHelperSE.AddOrCreate(classCodeElements, pathFile, element);
            }
        }
    }

    /// <summary>
    ///     A1 cant be null because is taked from MainWindowEveryLine.Instance2.chblIndexableExtensions.CheckedStrings() and
    ///     this is not available in Roslyn project
    /// </summary>
    /// <param name="loadExtensions"></param>
    /// <param name="term"></param>
    /// <param name="includeEmpty"></param>
    /// <param name="inComments"></param>
    /// <returns></returns>
    public Dictionary<string, List<FoundedCodeElement>> SearchInContent(List<string> loadExtensions, string term,
        bool includeEmpty, bool? inComments)
    {
        var result = new Dictionary<string, List<FoundedCodeElement>>();
        var include = false;
        foreach (var item in linesWithContent)
        {
            if (!CA.EndsWith(item.Key, loadExtensions)) continue;
#if DEBUG
            //if (Path.GetFileName( item.Key) == "MainWindow.cs")
            //{
            //}
#endif
            var indexes = linesWithIndexes[item.Key];
            include = false;
            // return with zero elements - in item.Value is only lines with content. I need lines with exactly content of file to localize searched results
            var founded = CA.ReturnWhichContainsIndexes(item.Value, term, SearchStrategy.AnySpaces);

            if (inComments.HasValue)
                //var lines = SHGetLines.GetLines
                for (var i = founded.Count - 1; i >= 0; i--)
                {
                    var line = item.Value[i].Trim();
                    if (line.StartsWith(CodeElementsConstants.SingleCommentCsharp))
                    {
                        if (!inComments.Value) founded.RemoveAt(i);
                    }
                    else
                    {
                        if (inComments.Value) founded.RemoveAt(i);
                    }
                }

            if (founded.Count == 0)
            {
                if (includeEmpty) include = true;
            }
            else
            {
                include = true;
            }

            var founded2 = new List<FoundedCodeElement>();
            foreach (var item2 in founded) founded2.Add(new FoundedCodeElement(indexes[item2], -1, 0));

            if (include) result.Add(item.Key, founded2);
        }

        return result;
    }

    /// <summary>
    ///     A4 = search for exact occur. otherwise split both to words
    /// </summary>
    /// <param name="text"></param>
    /// <param name="type"></param>
    /// <param name="classType"></param>
    /// <param name="searchStrategy"></param>
    public CodeElements FindNamespaceElement(List<string> loadExtensions, string text, NamespaceCodeElementsType type,
        ClassCodeElementsType classType, SearchStrategy searchStrategy = SearchStrategy.FixedSpace)
    {
        var makeChecking = type != NamespaceCodeElementsType.All;
        var result = new Dictionary<string, NamespaceCodeElements>();
        var resultClass = new Dictionary<string, ClassCodeElements>();
        var add = true;

        if (type != NamespaceCodeElementsType.Nope)
            foreach (var item in namespaceCodeElements)
            {
                if (!CA.EndsWith(item.Key, loadExtensions)) continue;

                var d = new NamespaceCodeElements();
                foreach (var item2 in item.Value)
                {
                    if (makeChecking)
                    {
                        add = false;
                        if (item2.Type == type)
                            // Nope there cannot be passed
                            add = true;
                        else if (classType == ClassCodeElementsType.All) add = true;
                    }

                    if (add)
                        if (SH.Contains(item2.NameWithoutGeneric, text, searchStrategy))
                            d.Add(item2);
                }

                if (d.Count > 0) result.Add(item.Key, d);
            }

        if (classType != ClassCodeElementsType.Nope)
            foreach (var item in classCodeElements)
            {
                var d = new ClassCodeElements();
                foreach (var item2 in item.Value)
                {
                    if (makeChecking)
                    {
                        add = false;
                        if (item2.Type == classType)
                            // Nope there cannot be passed
                            add = true;
                        else if (classType == ClassCodeElementsType.All) add = true;
                    }

                    if (add)
                        if (SH.Contains(item2.NameWithoutGeneric, text, searchStrategy))
                            d.Add(item2);
                }

                if (d.Count > 0) resultClass.Add(item.Key, d);
            }

        return new CodeElements { classes = resultClass, namespaces = result };
    }
    // Ve LoadAllFiles mi odstraní soubory jež nebyly ve selectMoreFolders
    // Z indexu odstraním ty z EndArgs aj. souvisejících v ProcessFile kde odjakživa se s tímto pracovalo

    #region All 4 for which is checked

    public EndArgs endArgs = null;
    public ContainsArgs containsArgs = null;
    public PpkOnDrive fileNames = null;
    public PpkOnDrive fileNamesExactly = null;
    public PpkOnDrive pathStarts = null;

    #endregion

    #region Working method

    /// <summary>
    ///     Je veřejná jen kvůli testu
    /// </summary>
    /// <param name="pathFile"></param>
    /// <param name="namespaceCodeElementsType"></param>
    /// <param name="classCodeElementsType"></param>
    /// <param name="removeRegions"></param>
    /// <param name="fromFileSystemWatcher"></param>
    public
#if ASYNC
        async Task
#else
        void
#endif
        ProcessFile(string pathFile, NamespaceCodeElementsType namespaceCodeElementsType,
            ClassCodeElementsType classCodeElementsType, bool removeRegions, bool fromFileSystemWatcher)
    {
        var removed = false;


        ProcessFileBoolResult result = null;
        result =
#if ASYNC
            await
#endif
                ProcessFileBool(pathFile, namespaceCodeElementsType, classCodeElementsType, removeRegions,
                    fromFileSystemWatcher);

        var _tree = result.tree;
        var root = result.root;

        if (result.indexed)
        {
            if (sourceFileTrees.ContainsKey(pathFile))
            {
                removed = true;
                sourceFileTrees.Remove(pathFile);
            }

            // Watcher.Start I can call after loading all files - otherwise there are million of events and app never start
            // Check whether folder is already indexing
            //if (!watchers.IsIndexindDirectory(pathFile))
            //{
            //watchers.Start(Path.GetDirectoryName( pathFile));
            //}
            if (!sourceFileTrees.ContainsKey(pathFile))
                sourceFileTrees.Add(pathFile, new SourceFileTree { root = root, tree = _tree });
            else
                sourceFileTrees[pathFile] = new SourceFileTree { root = root, tree = _tree };
        }
        else
        {
            RemoveFile(pathFile);
        }
    }

    public void RemoveFile(string t, bool fromFileSystemWatcher = false)
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

    #endregion
}