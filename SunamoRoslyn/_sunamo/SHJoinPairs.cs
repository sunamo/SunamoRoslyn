namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper for joining paired elements.
/// </summary>
internal class SHJoinPairs
{
    /// <summary>
    /// Joins pairs of parts with alternating delimiters.
    /// </summary>
    /// <param name="firstDelimiter">Delimiter after odd-indexed parts.</param>
    /// <param name="secondDelimiter">Delimiter after even-indexed parts.</param>
    /// <param name="parts">The parts to join in pairs.</param>
    /// <returns>The joined string.</returns>
    internal static string JoinPairs(string firstDelimiter, string secondDelimiter, params string[] parts)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < parts.Length; i++)
        {
            stringBuilder.Append(parts[i++] + firstDelimiter);
            stringBuilder.Append(parts[i] + secondDelimiter);
        }
        return stringBuilder.ToString();
    }
}
