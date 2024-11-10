using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn._sunamo;
internal class SH
{
    public static string RemoveLastLetters(string v1, int v2)
    {
        if (v1.Length > v2) return v1.Substring(0, v1.Length - v2);
        return v1;
    }

    public static void IndentAsPreviousLine(List<string> lines)
    {
        var indentPrevious = string.Empty;
        string line = null;
        var sb = new StringBuilder();
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
                    sb.Clear();
                    foreach (var item in line)
                        if (char.IsWhiteSpace(item))
                            sb.Append(item);
                        else
                            break;
                    indentPrevious = sb.ToString();
                }
            }
        }
    }
}
