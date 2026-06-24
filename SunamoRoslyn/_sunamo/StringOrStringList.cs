namespace SunamoRoslyn._sunamo;

internal class StringOrStringList
{
    internal StringOrStringList(string text)
    {
        String = text;
    }

    internal StringOrStringList(List<string> list)
    {
        List = list;
    }

    internal string? String { get; private set; }

    internal List<string>? List { get; private set; }

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
