namespace SunamoRoslyn._sunamo;

/// <summary>
/// Represents either a single string or a list of strings with lazy conversion between the two.
/// </summary>
internal class StringOrStringList
{
    /// <summary>
    /// Creates an instance from a single string.
    /// </summary>
    /// <param name="text">The string value.</param>
    internal StringOrStringList(string text)
    {
        String = text;
    }

    /// <summary>
    /// Creates an instance from a list of strings.
    /// </summary>
    /// <param name="list">The list of strings.</param>
    internal StringOrStringList(List<string> list)
    {
        List = list;
    }

    /// <summary>
    /// Gets the string representation.
    /// </summary>
    internal string? String { get; private set; }

    /// <summary>
    /// Gets the list representation.
    /// </summary>
    internal List<string>? List { get; private set; }

    /// <summary>
    /// Gets or lazily creates the string representation.
    /// </summary>
    /// <returns>The string value.</returns>
    internal string GetString()
    {
        if (String != null)
        {
            return String;
        }
        if (List != null)
        {
            if (String == null)
            {
                String = string.Join(" ", List);
            }
            return String;
        }
        throw new Exception("Both is null");
    }

    /// <summary>
    /// Gets or lazily creates the list representation by splitting the string.
    /// </summary>
    /// <returns>The list of strings.</returns>
    internal List<string> GetList()
    {
        if (String != null)
        {
            if (List == null)
            {
                var nonLetterNumberChars = String.Where(character => !char.IsLetterOrDigit(character)).ToArray();
                List = SH.SplitChar(String, nonLetterNumberChars);
            }
            return List;
        }
        if (List != null)
        {
            return List;
        }
        throw new Exception("Both is null");
    }
}
