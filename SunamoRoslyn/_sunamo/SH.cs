namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper methods.
/// </summary>
internal class SH
{
    /// <summary>
    /// Wraps a string value with the specified wrapper on both sides.
    /// </summary>
    /// <param name="text">The text to wrap.</param>
    /// <param name="wrapper">The wrapper text.</param>
    /// <returns>The wrapped string.</returns>
    internal static string WrapWith(string text, string wrapper)
    {
        return wrapper + text + wrapper;
    }

    /// <summary>
    /// Wraps a string value with the specified character.
    /// </summary>
    /// <param name="text">The text to wrap.</param>
    /// <param name="wrapCharacter">The character to wrap with.</param>
    /// <param name="isTrimming">Whether to trim the value before wrapping.</param>
    /// <param name="isWrappingWhitespaceOrEmpty">Whether to wrap whitespace or empty strings.</param>
    /// <returns>The wrapped string.</returns>
    internal static string WrapWithChar(string text, char wrapCharacter, bool isTrimming = false,
        bool isWrappingWhitespaceOrEmpty = true)
    {
        if (string.IsNullOrWhiteSpace(text) && !isWrappingWhitespaceOrEmpty) return string.Empty;
        return WrapWith(isTrimming ? text.Trim() : text, wrapCharacter.ToString());
    }

    /// <summary>
    /// Gets the word after the specified marker word in the input.
    /// </summary>
    /// <param name="input">The input text.</param>
    /// <param name="word">The marker word to search for.</param>
    /// <returns>The word following the marker.</returns>
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
    /// <summary>
    /// Converts the first character of the text to uppercase (by reference).
    /// </summary>
    /// <param name="text">The text to modify.</param>
    internal static void FirstCharUpper(ref string text)
    {
        text = FirstCharUpper(text);
    }

    /// <summary>
    /// Converts the first character of the text to uppercase.
    /// </summary>
    /// <param name="text">The text to modify.</param>
    /// <returns>The text with first character uppercased.</returns>
    internal static string FirstCharUpper(string text)
    {
        if (text.Length == 1)
        {
            return text.ToUpper();
        }
        string remainder = text.Substring(1);
        return text[0].ToString().ToUpper() + remainder;
    }
    #endregion

    /// <summary>
    /// Splits the text at the specified position into before and after parts.
    /// </summary>
    /// <param name="before">Output: text before the position.</param>
    /// <param name="after">Output: text after the position.</param>
    /// <param name="text">The text to split.</param>
    /// <param name="position">The split position.</param>
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
            if (text.Length > position + 1)
                after = text.Substring(position + 1);
            else
                after = string.Empty;
        }
    }

    /// <summary>
    /// Splits the text by the specified character delimiters, removing empty entries.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <param name="delimiters">The delimiter characters.</param>
    /// <returns>List of split parts.</returns>
    internal static List<string> SplitChar(string text, params char[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// Checks if the input contains the term using the specified search strategy.
    /// </summary>
    /// <param name="input">The input text to search.</param>
    /// <param name="searchTerm">The term to search for.</param>
    /// <param name="searchStrategy">The search strategy to use.</param>
    /// <param name="isCaseSensitive">Whether to use case-sensitive comparison.</param>
    /// <param name="isEnoughPartialContainsOfSplitted">Whether partial contains of split parts is sufficient.</param>
    /// <returns>True if the input contains the term.</returns>
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

            bool isContainingAll = true;
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

    /// <summary>
    /// Removes the specified number of characters from the end of the string.
    /// </summary>
    /// <param name="text">The text to truncate.</param>
    /// <param name="count">Number of characters to remove.</param>
    /// <returns>The truncated string.</returns>
    internal static string RemoveLastLetters(string text, int count)
    {
        if (text.Length > count) return text.Substring(0, text.Length - count);
        return text;
    }

    /// <summary>
    /// Indents each line to match the indentation of the previous line.
    /// </summary>
    /// <param name="lines">List of lines to process.</param>
    internal static void IndentAsPreviousLine(List<string> lines)
    {
        var previousIndent = string.Empty;
        string? currentLine = null;
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < lines.Count - 1; i++)
        {
            currentLine = lines[i];
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
