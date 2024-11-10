using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn._sunamo;
internal static class StringBuilderExtensions
{
    public static void AddItem(this StringBuilder sb, string postfix, string text)
    {
        sb.Append(text + postfix);
    }
}
