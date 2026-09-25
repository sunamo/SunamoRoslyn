namespace SunamoRoslyn.Data;

/// <summary>
/// Represents a class-level code element with a classification type.
/// </summary>
public class ClassCodeElement : CodeElement<ClassCodeElementsType>
{
    /// <summary>
    /// Returns a string representation of the class code element including its type and name.
    /// </summary>
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sClassCodeElements[Type] + " " + Name;
    }
}
