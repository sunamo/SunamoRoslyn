namespace SunamoRoslyn._sunamo;

internal class SH
{
    /// <summary>
    /// Wraps a string value with the specified wrapper on both sides.
    /// </summary>
    /// <param name="text">The text to wrap.</param>
    /// <param name="wrapper">The wrapper text.</param>
    /// <returns>The wrapped string.</returns>
    internal static string WrapWith(string text, string wrapper) => wrapper + text + wrapper;

    internal static string WrapWithChar(string text, char wrapCharacter, bool isTrimming = false,
        bool isWrappingWhitespaceOrEmpty = true)
    {
        if (string.IsNullOrWhiteSpace(text) && !isWrappingWhitespaceOrEmpty) return string.Empty;
        return WrapWith(isTrimming ? text.Trim() : text, wrapCharacter.ToString());
    }

    internal static string WordAfter(string input, string word)
    {
        input = WrapWithChar(input, ' ');
        var index = input.IndexOf(word);
        var spaceIndex = input.IndexOf(' ', index + 1);
        var stringBuilder = new StringBuilder();
        if (spaceIndex != -1)
        {
            spaceIndex++;
            for (var i = spaceIndex; i < input.Length; i++)
            {
                var character = input[i];
                if (character != ' ')
                    stringBuilder.Append(character);
                else
                    break;
            }
        }
        return stringBuilder.ToString();
    }

    #region SH.FirstCharUpper
    internal static void FirstCharUpper(ref string text)
    {
        text = FirstCharUpper(text);
    }

    internal static string FirstCharUpper(string text)
    {
        if (text.Length == 1)
        {
            return text.ToUpper();
        }
        var remainder = text.Substring(1);
        return text[0].ToString().ToUpper() + remainder;
    }
    #endregion

    internal static void GetPartsByLocation(out string before, out string after, string text, int position)
    {
        if (position == -1)
        {
            before = text;
            after = "";
        }
        else
        {
            before = text.Substring(0, position);
            after = text.Length > position + 1 ? text.Substring(position + 1) : string.Empty;
        }
    }

    internal static List<string> SplitChar(string text, params char[] delimiters)
        => text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();

    internal static bool Contains(string input, StringOrStringList searchTerm, SearchStrategyRoslyn searchStrategy = SearchStrategyRoslyn.FixedSpace, bool isCaseSensitive = false, bool isEnoughPartialContainsOfSplitted = true)
    {
        string? term = null;
        if (!isCaseSensitive)
        {
            input = input.ToLower();
            term = searchTerm.GetString().ToLower();
        }

        if (searchStrategy == SearchStrategyRoslyn.ExactlyName)
        {
            return input == term;
        }

        if (searchStrategy == SearchStrategyRoslyn.AnySpaces)
        {
            var inputParts = input.Split(input.Where(character => !char.IsLetterOrDigit(character)).ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var termParts = searchTerm.GetList();

            if (inputParts.Length == 1)
            {
                foreach (var item in termParts)
                {
                    if (!input.Contains(item))
                    {
                        return false;
                    }
                }
            }

            if (isEnoughPartialContainsOfSplitted)
            {
                foreach (var item in termParts)
                {
                    if (!input.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }

            var isContainingAll = true;
            foreach (var item in termParts)
            {
                if (!inputParts.Contains(item))
                {
                    isContainingAll = false;
                    break;
                }
            }
            return isContainingAll;
        }

        return input.Contains(term!);
    }

    internal static string RemoveLastLetters(string text, int count)
    {
        if (text.Length > count) return text.Substring(0, text.Length - count);
        return text;
    }

    internal static void IndentAsPreviousLine(List<string> lines)
    {
        var previousIndent = string.Empty;
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < lines.Count - 1; i++)
        {
            var currentLine = lines[i];
            if (currentLine.Length > 0)
            {
                if (!char.IsWhiteSpace(currentLine[0]))
                {
                    lines[i] = previousIndent + lines[i];
                }
                else
                {
                    stringBuilder.Clear();
                    foreach (var item in currentLine)
                        if (char.IsWhiteSpace(item))
                            stringBuilder.Append(item);
                        else
                            break;
                    previousIndent = stringBuilder.ToString();
                }
            }
        }
    }
}
