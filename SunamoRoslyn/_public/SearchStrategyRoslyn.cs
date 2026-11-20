namespace SunamoRoslyn._public;

/// <summary>
/// Represents the search strategy roslyn type.
/// </summary>
public enum SearchStrategyRoslyn
{
    // Contains
    /// <summary>
    /// The fixed space value.
    /// </summary>
    FixedSpace,
    // Splits the searched text (A1) and the search term (A2) by spaces and all parts of A2 must be present in A1
    /// <summary>
    /// The any spaces value.
    /// </summary>
    AnySpaces,
    // Is exactly the same
    /// <summary>
    /// The exactly name value.
    /// </summary>
    ExactlyName
}
