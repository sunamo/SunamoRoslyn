namespace SunamoRoslyn._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class CSharpHelper
{
    public const string Using = "using ";

    public static List<string> Usings(List<string> lines, string keyword, bool remove = false)
    {
        List<int> removeLines = null;
        return Usings(lines, keyword, out removeLines, remove);
    }

    public static List<string> Usings(List<string> lines, bool remove = false)
    {
        return Usings(lines, Using, remove);
    }

    public static List<string> Usings(List<string> lines, string keyword, out List<int> removeLines, bool remove = false)
    {
        List<string> usings = new();
        removeLines = new List<int>();

        int i = -1;
        foreach (var item in lines)
        {
            i++;
            var line = item.Trim();
            if (line != string.Empty)
            {
                if (line.StartsWith(keyword))
                {
                    removeLines.Add(i);
                    usings.Add(line);
                }
                else //if (line.Contains("{"))
                {
                    break;
                }
            }
        }

        if (remove)
        {
            CA.RemoveLines(lines, removeLines);
        }

        return usings.Distinct().ToList();
    }
}