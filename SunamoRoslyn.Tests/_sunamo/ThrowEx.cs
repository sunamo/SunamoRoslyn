using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn.Tests._sunamo;
internal class ThrowEx
{
    public static void Custom(string ex)
    {
        Debugger.Break();
        throw new Exception(ex);
    }
}
