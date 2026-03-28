namespace SunamoRoslyn;

/// <summary>
/// Provides comparison methods for Roslyn syntax elements.
/// </summary>
public class RoslynComparer
{
    /// <summary>
    /// Compares two modifier lists for equality by checking each modifier value.
    /// </summary>
    /// <param name="modifiers1">The first modifier list to compare.</param>
    /// <param name="modifiers2">The second modifier list to compare.</param>
    /// <returns>True if both modifier lists contain the same values in the same order.</returns>
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
