namespace SunamoRoslyn._public;

public enum SearchStrategyRoslyn
{
    // Contains
    FixedSpace,
    // Splits the searched text (A1) and the search term (A2) by spaces and all parts of A2 must be present in A1
    AnySpaces,
    // Is exactly the same
    ExactlyName
}
