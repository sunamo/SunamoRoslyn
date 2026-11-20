namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the class code element type.
/// </summary>
public class ClassCodeElement : CodeElement<ClassCodeElementsType>
{
    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sClassCodeElements[Type] + " " + Name;
    }
}
