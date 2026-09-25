namespace SunamoRoslyn._sunamo;

/// <summary>
/// Helper methods for enum operations.
/// </summary>
internal class EnumHelper
{
    /// <summary>
    /// Gets all enum values excluding Nope and None.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>List of enum values.</returns>
    internal static List<T> GetValues<T>()
      where T : struct
    {
        return GetValues<T>(false, true);
    }

    /// <summary>
    /// Gets enum values with optional inclusion of Nope/Shared.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="isIncludingNope">Whether to include the Nope value.</param>
    /// <param name="isIncludingShared">Whether to include the Shared value.</param>
    /// <returns>List of enum values.</returns>
    internal static List<T> GetValues<T>(bool isIncludingNope, bool isIncludingShared)
        where T : struct
    {
        var enumType = typeof(T);
        var values = Enum.GetValues(enumType).Cast<T>().ToList();
        T parsedValue;

        if (!isIncludingNope)
        {
            if (Enum.TryParse<T>(CodeElementsConstants.NopeValue, out parsedValue))
            {
                values.Remove(parsedValue);
            }
        }

        if (!isIncludingShared)
        {
            if (enumType.Name == "MySites")
            {
                if (Enum.TryParse<T>("Shared", out parsedValue))
                {
                    values.Remove(parsedValue);
                }
            }
            else
            {
                if (Enum.TryParse<T>("Sha", out parsedValue))
                {
                    values.Remove(parsedValue);
                }
            }
        }

        if (Enum.TryParse<T>(CodeElementsConstants.NoneValue, out parsedValue))
        {
            values.Remove(parsedValue);
        }

        return values;
    }

    /// <summary>
    /// Converts enum values to a dictionary with lowercase string representations.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumType">The type of the enum.</param>
    /// <returns>Dictionary mapping enum values to their lowercase names.</returns>
    internal static Dictionary<T, string> EnumToString<T>(Type enumType)
        where T : notnull
    {
        return Enum.GetValues(enumType).Cast<T>().Select(enumValue => new
        {
            Key = enumValue,
            Value = (enumValue.ToString() ?? string.Empty).ToLower()
        }
        ).ToDictionary(entry => entry.Key, entry => entry.Value);
    }
}
