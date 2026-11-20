namespace SunamoRoslyn;

/// <summary>
/// Represents the roslyn comparer type.
/// </summary>
public class RoslynComparer
{
    /// <summary>
    /// Modifiers.
    /// </summary>
    /// <param name="modifiers1">The modifiers 1.</param>
    /// <param name="modifiers2">The modifiers 2.</param>
    /// <returns>The modifiers result.</returns>
    public static bool Modifiers(SyntaxTokenList modifiers1, SyntaxTokenList modifiers2)
    {
        if (modifiers1.Count != modifiers2.Count)
        {
            return false;
        }

        for (int i = 0; i < modifiers2.Count; i++)
        {
            if (modifiers2[i].Value != modifiers1[i].Value)
            {
                return false;
            }
        }

        return true;
    }
}
