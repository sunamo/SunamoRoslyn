namespace SunamoRoslyn._sunamo;

internal class EnumHelper
{
    internal static List<T> GetValues<T>()
      where T : struct
    {
        return GetValues<T>(false, true);
    }

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
