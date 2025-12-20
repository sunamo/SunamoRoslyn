// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn;
using static CsFileFilterRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// A1 cant be null because is taked from MainWindowEveryLine.Instance2.chblIndexableExtensions.CheckedStrings() and this is not available in Roslyn project
    /// </summary>
    /// <param name = "loadExtensions"></param>
    /// <param name = "term"></param>
    /// <param name = "includeEmpty"></param>
    /// <param name = "inComments"></param>
    /// <returns></returns>
    public Dictionary<string, List<FoundedCodeElement>> SearchInContent(List<string> loadExtensions, string term, bool includeEmpty, bool? inComments)
    {
        Dictionary<string, List<FoundedCodeElement>> result = new Dictionary<string, List<FoundedCodeElement>>();
        bool include = false;
        foreach (var item in linesWithContent)
        {
            if (!CA.EndsWith(item.Key, loadExtensions))
            {
                continue;
            }

#if DEBUG
            //if (Path.GetFileName( item.Key) == "MainWindow.cs")
            //{
            //}
#endif
            var indexes = linesWithIndexes[item.Key];
            include = false;
            // return with zero elements - in item.Value is only lines with content. I need lines with exactly content of file to localize searched results
            List<int> founded = CA.ReturnWhichContainsIndexes(item.Value, term /*, SearchStrategyRoslyn.AnySpaces*/);
            if (inComments.HasValue)
            {
                //var lines = SHGetLines.GetLines
                for (int i = founded.Count - 1; i >= 0; i--)
                {
                    var line = item.Value[i].Trim();
                    if (line.StartsWith(CodeElementsConstants.SingleCommentCsharp))
                    {
                        if (!inComments.Value)
                        {
                            founded.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (inComments.Value)
                        {
                            founded.RemoveAt(i);
                        }
                    }
                }
            }

            if (founded.Count == 0)
            {
                if (includeEmpty)
                {
                    include = true;
                }
            }
            else
            {
                include = true;
            }

            var founded2 = new List<FoundedCodeElement>();
            foreach (var item2 in founded)
            {
                founded2.Add(new FoundedCodeElement(indexes[item2], -1, 0));
            }

            if (include)
            {
                result.Add(item.Key, founded2);
            }
        }

        return result;
    }

    /// <summary>
    /// A4 = search for exact occur. otherwise split both to words
    /// </summary>
    /// <param name = "text"></param>
    /// <param name = "type"></param>
    /// <param name = "classType"></param>
    /// <param name = "searchStrategy"></param>
    public CodeElements FindNamespaceElement(List<string> loadExtensions, string text, NamespaceCodeElementsType type, ClassCodeElementsType classType, SearchStrategyRoslyn searchStrategy = SearchStrategyRoslyn.FixedSpace)
    {
        bool makeChecking = type != NamespaceCodeElementsType.All;
        Dictionary<string, NamespaceCodeElements> result = new Dictionary<string, NamespaceCodeElements>();
        Dictionary<string, ClassCodeElements> resultClass = new Dictionary<string, ClassCodeElements>();
        bool add = true;
        if (type != NamespaceCodeElementsType.Nope)
        {
            foreach (var item in namespaceCodeElements)
            {
                if (!CA.EndsWith(item.Key, loadExtensions))
                {
                    continue;
                }

                NamespaceCodeElements d = new NamespaceCodeElements();
                foreach (var item2 in item.Value)
                {
                    if (makeChecking)
                    {
                        add = false;
                        if (item2.Type == type)
                        {
                            // Nope there cannot be passed
                            add = true;
                        }
                        else if (classType == ClassCodeElementsType.All)
                        {
                            add = true;
                        }
                    }

                    if (add)
                    {
                        if (SH.Contains(item2.NameWithoutGeneric, new StringOrStringList(text), searchStrategy))
                        {
                            d.Add(item2);
                        }
                    }
                }

                if (d.Count > 0)
                {
                    result.Add(item.Key, d);
                }
            }
        }

        if (classType != ClassCodeElementsType.Nope)
        {
            foreach (var item in classCodeElements)
            {
                ClassCodeElements d = new ClassCodeElements();
                foreach (var item2 in item.Value)
                {
                    if (makeChecking)
                    {
                        add = false;
                        if (item2.Type == classType)
                        {
                            // Nope there cannot be passed
                            add = true;
                        }
                        else if (classType == ClassCodeElementsType.All)
                        {
                            add = true;
                        }
                    }

                    if (add)
                    {
                        if (SH.Contains(item2.NameWithoutGeneric, new StringOrStringList(text), searchStrategy))
                        {
                            d.Add(item2);
                        }
                    }
                }

                if (d.Count > 0)
                {
                    resultClass.Add(item.Key, d);
                }
            }
        }

        return new CodeElements()
        {
            classes = resultClass,
            namespaces = result
        };
    }
}