namespace SunamoRoslyn.Data;

public class ClassCodeElement : CodeElement<ClassCodeElementsType>
{
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.E2sClassCodeElements[Type] + " " + Name;
    }
}
