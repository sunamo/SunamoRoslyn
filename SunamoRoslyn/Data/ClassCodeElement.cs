// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn.Data;

public class ClassCodeElement : CodeElement<ClassCodeElementsType>
{
    public override string ToString()
    {
        return SourceCodeIndexerRoslyn.e2sClassCodeElements[Type] + " " + Name;
    }
}
