namespace SunamoRoslyn._sunamo;

/// <summary>
/// Boolean utility helper for negation checks.
/// </summary>
internal class BTS
{
    /// <summary>
    /// Returns the value or its negation based on the flag.
    /// </summary>
    /// <param name="value">The boolean value to evaluate.</param>
    /// <param name="isNegating">Whether to negate the value.</param>
    /// <returns>The original or negated value.</returns>
    internal static bool Is(bool value, bool isNegating)
    {
        if (isNegating) return !value;
        return value;
    }
}
