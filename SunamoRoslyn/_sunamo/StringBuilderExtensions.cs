namespace SunamoRoslyn._sunamo;

internal static class StringBuilderExtensions
{
    internal static void AddItem(this StringBuilder stringBuilder, string postfix, string text)
    {
        stringBuilder.Append(text + postfix);
    }
}
