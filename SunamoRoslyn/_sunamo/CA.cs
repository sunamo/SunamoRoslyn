namespace SunamoRoslyn._sunamo;

/// <summary>
/// Collection argument helper methods.
/// </summary>
internal class CA
{
    /// <summary>
    /// Returns the line if it starts with any element from the list.
    /// </summary>
    /// <param name="list">List of prefixes to check against.</param>
    /// <param name="text">The text to check.</param>
    /// <returns>The matching line or null.</returns>
    internal static string? StartWith(List<string> list, string text)
    {
        string? foundElement = null;
        return StartWith(list, text, out foundElement);
    }

    /// <summary>
    /// Returns the line if it starts with any element from the list, outputting the matched element.
    /// </summary>
    /// <param name="list">List of prefixes to check against.</param>
    /// <param name="text">The text to check.</param>
    /// <param name="foundElement">The matched prefix element.</param>
    /// <returns>The matching line or null.</returns>
    internal static string? StartWith(List<string> list, string text, out string? foundElement)
    {
        foundElement = null;
        if (list != null)
            foreach (var item in list)
                if (text.StartsWith(item))
                {
                    foundElement = item;
                    return text;
                }
        return null;
    }

    /// <summary>
    /// Returns lines that contain the specified term, along with their indices.
    /// </summary>
    /// <param name="lines">Lines to search through.</param>
    /// <param name="term">The term to search for.</param>
    /// <param name="foundIndices">Output list of matching indices.</param>
    /// <param name="parseNegations">The comparison method to use.</param>
    /// <returns>List of matching lines.</returns>
    internal static List<string> ReturnWhichContains(List<string> lines, string term, out List<int> foundIndices,
    ContainsCompareMethodRoslyn parseNegations = ContainsCompareMethodRoslyn.WholeInput)
    {
        foundIndices = new List<int>();
        var result = new List<string>();
        var currentIndex = 0;
        List<string>? words = null;

        if (parseNegations == ContainsCompareMethodRoslyn.SplitToWords ||
            parseNegations == ContainsCompareMethodRoslyn.Negations)
        {
            WhitespaceCharService whitespaceChar = new();
            words = SHSplit.SplitNone(term, whitespaceChar.WhiteSpaceChars.ConvertAll(character => character.ToString()).ToArray());
        }

        if (parseNegations == ContainsCompareMethodRoslyn.WholeInput)
            foreach (var item in lines)
            {
                if (item.Contains(term))
                {
                    foundIndices.Add(currentIndex);
                    result.Add(item);
                }
                currentIndex++;
            }
        else if (parseNegations == ContainsCompareMethodRoslyn.SplitToWords ||
                 parseNegations == ContainsCompareMethodRoslyn.Negations)
            foreach (var item in lines)
            {
                if (words!.All(word => item.Contains(word)))
                {
                    foundIndices.Add(currentIndex);
                    result.Add(item);
                }
                currentIndex++;
            }
        else
            ThrowEx.NotImplementedCase(parseNegations);

        return result;
    }

    /// <summary>
    /// Wraps each element in the list with the specified text on both sides.
    /// </summary>
    /// <param name="list">List to wrap.</param>
    /// <param name="wrapper">Text to wrap with.</param>
    /// <returns>The modified list.</returns>
    internal static List<string> WrapWith(List<string> list, string wrapper)
    {
        return WrapWith(list, wrapper, wrapper);
    }

    /// <summary>
    /// Wraps each element in the list with before and after text. Direct edit.
    /// </summary>
    /// <param name="list">List to wrap.</param>
    /// <param name="before">Text to prepend.</param>
    /// <param name="after">Text to append.</param>
    /// <returns>The modified list.</returns>
    internal static List<string> WrapWith(List<string> list, string before, string after)
    {
        for (var i = 0; i < list.Count; i++) list[i] = before + list[i] + after;
        return list;
    }

    /// <summary>
    /// Checks if the text ends with any of the specified suffixes.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <param name="suffixes">List of suffixes to check against.</param>
    /// <returns>True if the text ends with any suffix.</returns>
    internal static bool EndsWith(string text, List<string> suffixes)
    {
        foreach (var item in suffixes)
            if (text.EndsWith(item))
                return true;
        return false;
    }

    /// <summary>
    /// Returns indices of elements that contain the specified term.
    /// </summary>
    /// <param name="list">List to search.</param>
    /// <param name="term">The term to search for.</param>
    /// <returns>List of matching indices.</returns>
    internal static List<int> ReturnWhichContainsIndexes(IList<string> list, string term)
    {
        var result = new List<int>();
        var currentIndex = 0;
        if (list != null)
            foreach (var item in list)
            {
                if (item.Contains(term)) result.Add(currentIndex);
                currentIndex++;
            }
        return result;
    }

    /// <summary>
    /// Prepends the specified prefix to each element that does not already start with it.
    /// </summary>
    /// <param name="prefix">The prefix to prepend.</param>
    /// <param name="list">List to modify.</param>
    /// <returns>The modified list.</returns>
    internal static List<string> Prepend(string prefix, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            if (!list[i].StartsWith(prefix))
                list[i] = prefix + list[i];
        return list;
    }

    /// <summary>
    /// Removes empty or whitespace-only strings from the list after trimming.
    /// </summary>
    /// <param name="list">List to filter.</param>
    /// <returns>The filtered list.</returns>
    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i].Trim() == string.Empty)
                list.RemoveAt(i);
        return list;
    }
}
