namespace SunamoRoslyn._sunamo;

internal class CA
{
    internal static string? StartWith(List<string> list, string text)
    {
        string? foundElement = null;
        return StartWith(list, text, out foundElement);
    }

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

    internal static List<string> WrapWith(List<string> list, string wrapper)
    {
        return WrapWith(list, wrapper, wrapper);
    }

    // Direct edit.
    internal static List<string> WrapWith(List<string> list, string before, string after)
    {
        for (var i = 0; i < list.Count; i++) list[i] = before + list[i] + after;
        return list;
    }

    internal static bool EndsWith(string text, List<string> suffixes)
    {
        foreach (var item in suffixes)
            if (text.EndsWith(item))
                return true;
        return false;
    }

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

    internal static List<string> Prepend(string prefix, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            if (!list[i].StartsWith(prefix))
                list[i] = prefix + list[i];
        return list;
    }

    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i].Trim() == string.Empty)
                list.RemoveAt(i);
        return list;
    }
}
