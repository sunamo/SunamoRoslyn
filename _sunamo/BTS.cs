namespace SunamoRoslyn._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BTS
{
    public static bool Is(bool binFp, bool n)
    {
        if (n) return !binFp;
        return binFp;
    }
}