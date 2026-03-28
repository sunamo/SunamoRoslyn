namespace SunamoRoslyn.Enums;

/// <summary>
/// Specifies the types of namespace-level code elements to search for or filter.
/// </summary>
public enum NamespaceCodeElementsType
{
    /// <summary>
    /// No namespace-level code elements.
    /// </summary>
    Nope = 0,
    /// <summary>
    /// Enum declarations.
    /// </summary>
    Enum = 1,
    /// <summary>
    /// Class declarations.
    /// </summary>
    Class = 2,
    /// <summary>
    /// Interface declarations.
    /// </summary>
    Interface = 4,
    /// <summary>
    /// Struct declarations.
    /// </summary>
    Struct = 8,
    /// <summary>
    /// All namespace-level code elements.
    /// </summary>
    All = 255
}