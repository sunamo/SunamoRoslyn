namespace SunamoRoslyn;

using static CsFileFilterRoslyn;

/// <summary>
/// Contains search and find methods for SourceCodeIndexerRoslyn.
/// </summary>
public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// Searches indexed file content for a term and returns matching code elements.
    /// </summary>
    /// <param name="loadExtensions">File extensions to include in the search.</param>
    /// <param name="term">The search term to look for.</param>
    /// <param name="includeEmpty">Whether to include files with no matches in the result.</param>
    /// <param name="inComments">If true, search only in comments; if false, exclude comments; if null, search everywhere.</param>
    /// <returns>Dictionary mapping file paths to lists of found code elements.</returns>
    public Dictionary<string, List<FoundedCodeElement>> SearchInContent(List<string> loadExtensions, string term, bool includeEmpty, bool? inComments)
    {
        Dictionary<string, List<FoundedCodeElement>> result = new Dictionary<string, List<FoundedCodeElement>>();
        bool include = false;
        foreach (var item in LinesWithContent)
        {
            if (!CA.EndsWith(item.Key, loadExtensions))
            {
                continue;
            }

            var indexes = LinesWithIndexes[item.Key];
            include = false;
            List<int> foundIndices = CA.ReturnWhichContainsIndexes(item.Value, term);
            if (inComments.HasValue)
            {
                for (int i = foundIndices.Count - 1; i >= 0; i--)
                {
                    var line = item.Value[i].Trim();
                    if (line.StartsWith(CodeElementsConstants.SingleCommentCsharp))
                    {
                        if (!inComments.Value)
                        {
                            foundIndices.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (inComments.Value)
                        {
                            foundIndices.RemoveAt(i);
                        }
                    }
                }
            }

            if (foundIndices.Count == 0)
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

            var foundCodeElements = new List<FoundedCodeElement>();
            foreach (var foundIndex in foundIndices)
            {
                foundCodeElements.Add(new FoundedCodeElement(indexes[foundIndex], -1, 0));
            }

            if (include)
            {
                result.Add(item.Key, foundCodeElements);
            }
        }

        return result;
    }

    /// <summary>
    /// Finds namespace and class code elements matching the search criteria.
    /// </summary>
    /// <param name="loadExtensions">File extensions to include in the search.</param>
    /// <param name="text">The text to search for in element names.</param>
    /// <param name="type">The namespace code element type filter.</param>
    /// <param name="classType">The class code element type filter.</param>
    /// <param name="searchStrategy">The search strategy to use for matching.</param>
    /// <returns>Combined namespace and class code elements matching the criteria.</returns>
    public CodeElements FindNamespaceElement(List<string> loadExtensions, string text, NamespaceCodeElementsType type, ClassCodeElementsType classType, SearchStrategyRoslyn searchStrategy = SearchStrategyRoslyn.FixedSpace)
    {
        bool makeChecking = type != NamespaceCodeElementsType.All;
        Dictionary<string, NamespaceCodeElements> result = new Dictionary<string, NamespaceCodeElements>();
        Dictionary<string, ClassCodeElements> resultClass = new Dictionary<string, ClassCodeElements>();
        bool shouldAdd = true;
        if (type != NamespaceCodeElementsType.Nope)
        {
            foreach (var item in namespaceCodeElements)
            {
                if (!CA.EndsWith(item.Key, loadExtensions))
                {
                    continue;
                }

                NamespaceCodeElements namespaceElements = new NamespaceCodeElements();
                foreach (var codeElement in item.Value)
                {
                    if (makeChecking)
                    {
                        shouldAdd = false;
                        if (codeElement.Type == type)
                        {
                            shouldAdd = true;
                        }
                        else if (classType == ClassCodeElementsType.All)
                        {
                            shouldAdd = true;
                        }
                    }

                    if (shouldAdd)
                    {
                        if (SH.Contains(codeElement.NameWithoutGeneric, new StringOrStringList(text), searchStrategy))
                        {
                            namespaceElements.Add(codeElement);
                        }
                    }
                }

                if (namespaceElements.Count > 0)
                {
                    result.Add(item.Key, namespaceElements);
                }
            }
        }

        if (classType != ClassCodeElementsType.Nope)
        {
            foreach (var item in classCodeElements)
            {
                ClassCodeElements classElements = new ClassCodeElements();
                foreach (var codeElement in item.Value)
                {
                    if (makeChecking)
                    {
                        shouldAdd = false;
                        if (codeElement.Type == classType)
                        {
                            shouldAdd = true;
                        }
                        else if (classType == ClassCodeElementsType.All)
                        {
                            shouldAdd = true;
                        }
                    }

                    if (shouldAdd)
                    {
                        if (SH.Contains(codeElement.NameWithoutGeneric, new StringOrStringList(text), searchStrategy))
                        {
                            classElements.Add(codeElement);
                        }
                    }
                }

                if (classElements.Count > 0)
                {
                    resultClass.Add(item.Key, classElements);
                }
            }
        }

        return new CodeElements()
        {
            Classes = resultClass,
            Namespaces = result
        };
    }
}
