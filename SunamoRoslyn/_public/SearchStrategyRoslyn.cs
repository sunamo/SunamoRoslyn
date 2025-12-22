namespace SunamoRoslyn._public;

public enum SearchStrategyRoslyn
{
    /// <summary>
    /// Contains
    /// </summary>
    FixedSpace,
    /// <summary>
    /// rozdělí prohledávané (A1) a hledané (A2) dle mezer a vše z A2 musí být v A1
    /// </summary>
    AnySpaces,
    /// <summary>
    /// Is exactly the same
    /// </summary>
    ExactlyName
}