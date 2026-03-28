namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper for splitting text into lines.
/// </summary>
internal class SHGetLines
{
    /// <summary>
    /// Splits the text into lines handling all newline formats.
    /// </summary>
    /// <param name="text">The text to split into lines.</param>
    /// <returns>List of lines.</returns>
    internal static List<string> GetLines(string text)
    {
        var parts = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(parts);
        return parts;
    }

    private static void SplitByUnixNewline(List<string> list)
    {
        SplitBy(list, "\r");
        SplitBy(list, "\n");
    }

    private static void SplitBy(List<string> list, string delimiter)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var carriageReturnNewlineParts = list[i].Split(new[] { "\r\n" }, StringSplitOptions.None);
                var newlineCarriageReturnParts = list[i].Split(new[] { "\n\r" }, StringSplitOptions.None);

                if (carriageReturnNewlineParts.Length > 1)
                    ThrowEx.Custom("cannot contain any \r\name, pass already split by this pattern");
                else if (newlineCarriageReturnParts.Length > 1) ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
            }

            var splitParts = list[i].Split(new[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1) InsertOnIndex(list, splitParts.ToList(), i);
        }
    }

    private static void InsertOnIndex(List<string> list, List<string> insertList, int index)
    {
        insertList.Reverse();

        list.RemoveAt(index);

        foreach (var item in insertList) list.Insert(index, item);
    }
}
