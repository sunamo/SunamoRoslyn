namespace SunamoRoslyn.Data;

/// <summary>
/// Contains collections of namespace and class code elements indexed by name.
/// </summary>
public class CodeElements
{
    /// <summary>
    /// Gets or sets the dictionary of namespace code elements, keyed by namespace name.
    /// Used in DoSearch in Everyline and returned together with Classes from SourceCodeIndexerRoslyn.
    /// </summary>
    public Dictionary<string, NamespaceCodeElements> Namespaces { get; set; } = new Dictionary<string, NamespaceCodeElements>();

    /// <summary>
    /// Gets or sets the dictionary of class code elements, keyed by class name.
    /// Used in DoSearch in Everyline and returned together with Namespaces from SourceCodeIndexerRoslyn.
    /// </summary>
    public Dictionary<string, ClassCodeElements> Classes { get; set; } = new Dictionary<string, ClassCodeElements>();
}
