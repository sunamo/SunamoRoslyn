using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn.Tests._sunamo;

/// <summary>
/// Helper for throwing formatted exceptions.
/// </summary>
internal class ThrowEx
{
    /// <summary>
    /// Breaks the debugger and throws an exception with the specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public static void Custom(string message)
    {
        Debugger.Break();
        throw new Exception(message);
    }
}
