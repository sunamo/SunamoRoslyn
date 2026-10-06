namespace SunamoRoslyn._sunamo;

internal class ThrowEx
{
    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? formattedMessage = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(formattedMessage, isReallyThrowing);
    }

    internal static bool DifferentCountInLists(string firstName, int firstCount, string secondName, int secondCount)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), firstName, firstCount, secondName, secondCount));
    }

    internal static bool NotImplementedCase(object notImplementedName)
    { return ThrowIsNotNull(Exceptions.NotImplementedCase, notImplementedName); }

    internal static bool ThrowIsNotNull<TArg>(Func<string, TArg, string?> exceptionFunction, TArg argument)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode(), argument);
        return ThrowIsNotNull(exception);
    }

    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    internal static bool NotImplementedMethod() { return ThrowIsNotNull(Exceptions.NotImplementedMethod); }

    internal static bool ThrowIsNotNull(Func<string, string?> exceptionFunction)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode());
        return ThrowIsNotNull(exception);
    }

    private static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }
            methodName = Exceptions.CallingMethod(depth);
        }

        string typeFullName;
        if (type is Type typeInstance)
        {
            typeFullName = typeInstance.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase methodBase)
        {
            typeFullName = methodBase.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = methodBase.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type objectType = type.GetType();
            typeFullName = objectType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    internal static bool ThrowIsNotNull(string? exception, bool isReallyThrowing = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (isReallyThrowing)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }
}
