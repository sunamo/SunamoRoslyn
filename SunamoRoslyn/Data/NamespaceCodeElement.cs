namespace SunamoRoslyn.Data;

/// <summary>
/// Represents a namespace-level code element with a classification type.
/// </summary>
public class NamespaceCodeElement : CodeElement<NamespaceCodeElementsType>
{
    /// <summary>
    /// Returns a string representation of the namespace code element including its type and name.
    /// </summary>
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sNamespaceCodeElements[Type] + " " + Name;
    }
}
