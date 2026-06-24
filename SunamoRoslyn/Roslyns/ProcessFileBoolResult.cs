namespace SunamoRoslyn.Roslyns;

public class ProcessFileBoolResult
{
    public ProcessFileBoolResult()
    {

    }

    public bool Indexed { get; set; } = false;

    public SyntaxTree? Tree { get; set; }

    public CompilationUnitSyntax? Root { get; set; }
}
