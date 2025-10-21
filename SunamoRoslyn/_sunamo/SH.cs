// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoRoslyn._sunamo;

internal class SH
{
    internal static string WrapWith(string value, string h)
    {
        return h + value + h;
    }
    internal static string WrapWithChar(string value, char v, bool _trimWrapping = false,
        bool alsoIfIsWhitespaceOrEmpty = true)
    {
        if (string.IsNullOrWhiteSpace(value) && !alsoIfIsWhitespaceOrEmpty) return string.Empty;
        // TODO: Make with StringBuilder, because of WordAfter and so
        return WrapWith(_trimWrapping ? value.Trim() : value, v.ToString());
    }
    internal static string WordAfter(string input, string word)
    {
        input = WrapWithChar(input, ' ');
        var dex = input.IndexOf(word);
        var dex2 = input.IndexOf(' ', dex + 1);
        var stringBuilder = new StringBuilder();
        if (dex2 != -1)
        {
            dex2++;
            for (var i = dex2; i < input.Length; i++)
            {
                var ch = input[i];
                if (ch != ' ')
                    stringBuilder.Append(ch);
                else
                    break;
            }
        }
        return stringBuilder.ToString();
    }
    #region SH.FirstCharUpper
    internal static void FirstCharUpper(ref string nazevPP)
    {
        nazevPP = FirstCharUpper(nazevPP);
    }
    internal static string FirstCharUpper(string nazevPP)
    {
        if (nazevPP.Length == 1)
        {
            return nazevPP.ToUpper();
        }
        string sb = nazevPP.Substring(1);
        return nazevPP[0].ToString().ToUpper() + stringBuilder;
    }
    #endregion
    internal static void GetPartsByLocation(out string pred, out string za, string text, int pozice)
    {
        if (pozice == -1)
        {
            pred = text;
            za = "";
        }
        else
        {
            pred = text.Substring(0, pozice);
            if (text.Length > pozice + 1)
                za = text.Substring(pozice + 1);
            else
                za = string.Empty;
        }
    }
    internal static List<string> SplitChar(string text, params char[] dot)
    {
        return text.Split(dot, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    internal static bool Contains(string input, StringOrStringList termO, SearchStrategyRoslyn searchStrategy = SearchStrategyRoslyn.FixedSpace, bool caseSensitive = false, bool isEnoughPartialContainsOfSplitted = true)
    {
        string term = null;
        if (!caseSensitive)
        {
            input = input.ToLower();
            term = termO.GetString().ToLower();
        }
        // musel bych dotáhnout min 2 metody a další enumy
        if (searchStrategy == SearchStrategyRoslyn.ExactlyName)
        {
            return input == term;
        }
        if (searchStrategy == SearchStrategyRoslyn.AnySpaces)
        {
            var pInput = input.Split(input.Where(ch => !char.IsLetterOrDigit(ch)).ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var pTerm = termO.GetList();
            if (pInput.Length == 1)
            {
                foreach (var item in pTerm)
                {
                    if (!input.Contains(item))
                    {
                        return false;
                    }
                }
            }
            if (isEnoughPartialContainsOfSplitted)
            {
                foreach (var item in pTerm)
                {
                    if (!input.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            bool containsAll = true;
            foreach (var item in pTerm)
            {
                if (!pInput.Contains(item))
                {
                    containsAll = false;
                    break;
                }
            }
            return containsAll;
        }
        return input.Contains(term);
    }
    internal static string RemoveLastLetters(string v1, int v2)
    {
        if (v1.Length > v2) return v1.Substring(0, v1.Length - v2);
        return v1;
    }
    internal static void IndentAsPreviousLine(List<string> lines)
    {
        var indentPrevious = string.Empty;
        string line = null;
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < lines.Count - 1; i++)
        {
            line = lines[i];
            if (line.Length > 0)
            {
                if (!char.IsWhiteSpace(line[0]))
                {
                    lines[i] = indentPrevious + lines[i];
                }
                else
                {
                    stringBuilder.Clear();
                    foreach (var item in line)
                        if (char.IsWhiteSpace(item))
                            stringBuilder.Append(item);
                        else
                            break;
                    indentPrevious = stringBuilder.ToString();
                }
            }
        }
    }
}