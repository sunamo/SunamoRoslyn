// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn.Data;

    public class NamespaceCodeElement : CodeElement<NamespaceCodeElementsType>
    {
        public override string ToString()
        {
            return SourceCodeIndexerRoslyn.e2sNamespaceCodeElements[Type] + " " + Name;
        }
    }
