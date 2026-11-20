namespace SunamoRoslyn;

using static CsFileFilterRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// Search in content.
    /// </summary>
    /// <param name="loadExtensions">The load extensions.</param>
    /// <param name="term">The term.</param>
    /// <param name="includeEmpty">The include empty.</param>
    /// <param name="inComments">The in comments.</param>
    /// <returns>The search in content result.</returns>
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
    /// Find namespace element.
    /// </summary>
    /// <param name="loadExtensions">The load extensions.</param>
    /// <param name="text">The text.</param>
    /// <param name="type">The type.</param>
    /// <param name="classType">The class type.</param>
    /// <param name="searchStrategy">The search strategy.</param>
    /// <returns>The find namespace element result.</returns>
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
