namespace SunamoRoslyn.Data;

public class CodeElements
{
    // Used in DoSearch in Everyline and returned together with Classes from SourceCodeIndexerRoslyn.
    public Dictionary<string, NamespaceCodeElements> Namespaces { get; set; } = new Dictionary<string, NamespaceCodeElements>();

    // Used in DoSearch in Everyline and returned together with Namespaces from SourceCodeIndexerRoslyn.
    public Dictionary<string, ClassCodeElements> Classes { get; set; } = new Dictionary<string, ClassCodeElements>();
}
