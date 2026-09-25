namespace SunamoRoslyn.Data;

public class NamespaceCodeElement : CodeElement<NamespaceCodeElementsType>
{
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sNamespaceCodeElements[Type] + " " + Name;
    }
}
