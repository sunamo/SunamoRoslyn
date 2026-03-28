namespace SunamoRoslyn._public;

/// <summary>
/// Defines the search strategy used for matching code elements.
/// </summary>
public enum SearchStrategyRoslyn
{
    /// <summary>
    /// Contains
    /// </summary>
    FixedSpace,
    /// <summary>
    /// Splits the searched text (A1) and the search term (A2) by spaces and all parts of A2 must be present in A1
    /// </summary>
    AnySpaces,
    /// <summary>
    /// Is exactly the same
    /// </summary>
    ExactlyName
}