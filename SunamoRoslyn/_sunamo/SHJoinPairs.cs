namespace SunamoRoslyn._sunamo;

internal class SHJoinPairs
{
    internal static string JoinPairs(string firstDelimiter, string secondDelimiter, params string[] parts)
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < parts.Length; i++)
        {
            stringBuilder.Append(parts[i++] + firstDelimiter);
            stringBuilder.Append(parts[i] + secondDelimiter);
        }
        return stringBuilder.ToString();
    }
}
