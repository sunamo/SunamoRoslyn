namespace SunamoRoslyn.Enums;

/// <summary>
/// Represents the namespace code elements type type.
/// </summary>
public enum NamespaceCodeElementsType
{
    /// <summary>
    /// The nope value.
    /// </summary>
    Nope = 0,
    /// <summary>
    /// The enum value.
    /// </summary>
    Enum = 1,
    /// <summary>
    /// The class value.
    /// </summary>
    Class = 2,
    /// <summary>
    /// The interface value.
    /// </summary>
    Interface = 4,
    /// <summary>
    /// The struct value.
    /// </summary>
    Struct = 8,
    /// <summary>
    /// The all value.
    /// </summary>
    All = 255
}
