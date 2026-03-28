namespace SunamoRoslyn._sunamo;

/// <summary>
/// Helper for throwing formatted exceptions with context information.
/// </summary>
internal class ThrowEx
{
    /// <summary>
    /// Throws a custom exception with the specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="isReallyThrowing">Whether to actually throw or just return true.</param>
    /// <param name="secondMessage">Additional message text.</param>
    /// <returns>True if an exception would have been thrown.</returns>
    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? formattedMessage = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(formattedMessage, isReallyThrowing);
    }

    /// <summary>
    /// Throws if two collections have different counts.
    /// </summary>
    /// <param name="firstName">Name of the first collection.</param>
    /// <param name="firstCount">Count of the first collection.</param>
    /// <param name="secondName">Name of the second collection.</param>
    /// <param name="secondCount">Count of the second collection.</param>
    /// <returns>True if counts differ.</returns>
    internal static bool DifferentCountInLists(string firstName, int firstCount, string secondName, int secondCount)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), firstName, firstCount, secondName, secondCount));
    }

    /// <summary>
    /// Throws for an unimplemented case.
    /// </summary>
    /// <param name="notImplementedName">The unimplemented case identifier.</param>
    /// <returns>True if an exception was thrown.</returns>
    internal static bool NotImplementedCase(object notImplementedName)
    { return ThrowIsNotNull(Exceptions.NotImplementedCase, notImplementedName); }

    /// <summary>
    /// Throws if the result of the function is not null.
    /// </summary>
    /// <typeparam name="TArg">The argument type.</typeparam>
    /// <param name="exceptionFunction">The function that generates the exception message.</param>
    /// <param name="argument">The argument to pass to the function.</param>
    /// <returns>True if an exception was thrown.</returns>
    internal static bool ThrowIsNotNull<TArg>(Func<string, TArg, string?> exceptionFunction, TArg argument)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode(), argument);
        return ThrowIsNotNull(exception);
    }

    /// <summary>
    /// Gets the full name of the currently executed code from the stack trace.
    /// </summary>
    /// <returns>The full name of the executed code.</returns>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    /// <summary>
    /// Throws for a not-implemented method.
    /// </summary>
    /// <returns>True if an exception was thrown.</returns>
    internal static bool NotImplementedMethod() { return ThrowIsNotNull(Exceptions.NotImplementedMethod); }

    /// <summary>
    /// Throws if the result of the parameterless function is not null.
    /// </summary>
    /// <param name="exceptionFunction">The function that generates the exception message.</param>
    /// <returns>True if an exception was thrown.</returns>
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

    /// <summary>
    /// Throws an exception if the message is not null.
    /// </summary>
    /// <param name="exception">The exception message.</param>
    /// <param name="isReallyThrowing">Whether to actually throw.</param>
    /// <returns>True if the message was not null.</returns>
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
