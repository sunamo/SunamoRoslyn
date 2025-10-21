// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn.Roslyns;

public class ProcessFileBoolResult
{
    public ProcessFileBoolResult()
    {

    }

    public bool indexed { get; set; } = false;
    public SyntaxTree tree { get; set;}
    public CompilationUnitSyntax root { get; set; }
}
