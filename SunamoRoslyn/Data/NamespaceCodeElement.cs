namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the namespace code element type.
/// </summary>
public class NamespaceCodeElement : CodeElement<NamespaceCodeElementsType>
{
    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sNamespaceCodeElements[Type] + " " + Name;
    }
}
