namespace SunamoRoslyn.Enums;

/// <summary>
/// Specifies the types of class-level code elements to search for or filter.
/// </summary>
public enum ClassCodeElementsType
{
    /// <summary>
    /// No class-level code elements.
    /// </summary>
    Nope = 0,
    /// <summary>
    /// Method declarations.
    /// </summary>
    Method = 1,
    /// <summary>
    /// All class-level code elements.
    /// </summary>
    All = 255
}