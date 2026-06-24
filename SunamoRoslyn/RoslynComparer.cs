namespace SunamoRoslyn;

public class RoslynComparer
{
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
