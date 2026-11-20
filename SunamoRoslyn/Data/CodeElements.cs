namespace SunamoRoslyn.Data;

/// <summary>
/// Represents the code elements type.
/// </summary>
public class CodeElements
{
    // Used in DoSearch in Everyline and returned together with Classes from SourceCodeIndexerRoslyn.
    /// <summary>
    /// Gets or sets the namespaces.
    /// </summary>
    public Dictionary<string, NamespaceCodeElements> Namespaces { get; set; } = new Dictionary<string, NamespaceCodeElements>();

    // Used in DoSearch in Everyline and returned together with Namespaces from SourceCodeIndexerRoslyn.
    /// <summary>
    /// Gets or sets the classes.
    /// </summary>
    public Dictionary<string, ClassCodeElements> Classes { get; set; } = new Dictionary<string, ClassCodeElements>();
}
