namespace SunamoRoslyn._sunamo;

/// <summary>
/// Exception message formatting utilities.
/// </summary>
internal class Exceptions
{
    /// <summary>
    /// Creates a custom exception message.
    /// </summary>
    /// <param name="before">Prefix text for the message.</param>
    /// <param name="message">The exception message.</param>
    /// <returns>The formatted exception message.</returns>
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }

    /// <summary>
    /// Returns a message if two collections have different counts.
    /// </summary>
    /// <param name="before">Prefix text for the message.</param>
    /// <param name="firstName">Name of the first collection.</param>
    /// <param name="firstCount">Count of the first collection.</param>
    /// <param name="secondName">Name of the second collection.</param>
    /// <param name="secondCount">Count of the second collection.</param>
    /// <returns>The error message or null if counts match.</returns>
    internal static string? DifferentCountInLists(string before, string firstName, int firstCount, string secondName, int secondCount)
    {
        if (firstCount != secondCount)
            return CheckBefore(before) + " different count elements in collection" + " " +
            string.Concat(firstName + "-" + firstCount) + " vs. " +
            string.Concat(secondName + "-" + secondCount);
        return null;
    }

    /// <summary>
    /// Returns a not-implemented-method error message.
    /// </summary>
    /// <param name="before">Prefix text for the message.</param>
    /// <returns>The error message.</returns>
    internal static string? NotImplementedMethod(string before)
    {
        return CheckBefore(before) + "Not implemented method.";
    }

    /// <summary>
    /// Extracts the type and method name from a stack trace line.
    /// </summary>
    /// <param name="stackTraceLine">The stack trace line to parse.</param>
    /// <param name="typeName">Output: the extracted type name.</param>
    /// <param name="methodName">Output: the extracted method name.</param>
    internal static void TypeAndMethodName(string stackTraceLine, out string typeName, out string methodName)
    {
        var afterAtPart = stackTraceLine.Split("at ")[1].Trim();
        var fullName = afterAtPart.Split("(")[0];
        var parts = fullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = parts[^1];
        parts.RemoveAt(parts.Count - 1);
        typeName = string.Join(".", parts);
    }

    /// <summary>
    /// Gets the place of exception from the current stack trace.
    /// </summary>
    /// <param name="isFillAlsoFirstTwo">Whether to fill also first two fields.</param>
    /// <returns>Tuple of type name, method name, and full stack trace.</returns>
    internal static Tuple<string, string, string> PlaceOfException(bool isFillAlsoFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var stackTraceText = stackTrace.ToString();
        var lines = stackTraceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);

        string typeName = string.Empty;
        string methodName = string.Empty;

        for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
        {
            var currentLine = lines[lineIndex];
            if (isFillAlsoFirstTwo)
                if (!currentLine.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(currentLine, out typeName, out methodName);
                    isFillAlsoFirstTwo = false;
                }
            if (currentLine.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }

        return new Tuple<string, string, string>(typeName, methodName, string.Join(Environment.NewLine, lines));
    }

    /// <summary>
    /// Gets the name of the calling method at the specified depth.
    /// </summary>
    /// <param name="depth">Stack frame depth.</param>
    /// <returns>The calling method name.</returns>
    internal static string CallingMethod(int depth = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(depth)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }

    /// <summary>
    /// Returns a not-implemented-case error message.
    /// </summary>
    /// <param name="before">Prefix text for the message.</param>
    /// <param name="notImplementedName">The case that is not implemented.</param>
    /// <returns>The error message.</returns>
    internal static string? NotImplementedCase(string before, object notImplementedName)
    {
        var forText = string.Empty;
        if (notImplementedName != null)
        {
            forText = " for ";
            if (notImplementedName.GetType() == typeof(Type))
                forText += ((Type)notImplementedName).FullName;
            else
                forText += notImplementedName.ToString();
        }
        return CheckBefore(before) + "Not implemented case" + forText + " . internal program error. Please contact developer" +
        ".";
    }

    /// <summary>
    /// Formats the before prefix with a colon separator.
    /// </summary>
    /// <param name="before">The prefix to format.</param>
    /// <returns>The formatted prefix or empty string.</returns>
    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }
}
