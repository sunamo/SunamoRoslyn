namespace SunamoRoslyn._sunamo;

internal class Exceptions
{
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }

    internal static string? DifferentCountInLists(string before, string firstName, int firstCount, string secondName, int secondCount)
    {
        if (firstCount != secondCount)
            return CheckBefore(before) + " different count elements in collection" + " " +
            string.Concat(firstName + "-" + firstCount) + " vs. " +
            string.Concat(secondName + "-" + secondCount);
        return null;
    }

    internal static string? NotImplementedMethod(string before)
    {
        return CheckBefore(before) + "Not implemented method.";
    }

    internal static void TypeAndMethodName(string stackTraceLine, out string typeName, out string methodName)
    {
        var afterAtPart = stackTraceLine.Split("at ")[1].Trim();
        var fullName = afterAtPart.Split("(")[0];
        var parts = fullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = parts[^1];
        parts.RemoveAt(parts.Count - 1);
        typeName = string.Join(".", parts);
    }

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

    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }
}
