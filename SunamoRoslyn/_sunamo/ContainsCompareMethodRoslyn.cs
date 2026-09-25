namespace SunamoRoslyn._sunamo;

/// <summary>
/// Specifies the comparison method for contains operations.
/// </summary>
internal enum ContainsCompareMethodRoslyn
{
    /// <summary>
    /// Compare using the whole input string.
    /// </summary>
    WholeInput,

    /// <summary>
    /// Split input into words before comparing.
    /// </summary>
    SplitToWords,

    /// <summary>
    /// Handle negations during comparison.
    /// </summary>
    Negations
}