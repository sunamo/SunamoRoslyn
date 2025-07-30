namespace SunamoRoslyn._sunamo;

internal static class StringBuilderExtensions
{
    internal static void AddItem(this StringBuilder sb, string postfix, string text)
    {
        sb.Append(text + postfix);
    }
}