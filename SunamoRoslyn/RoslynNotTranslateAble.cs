namespace SunamoRoslyn;

/// <summary>
/// Contains string constants for Roslyn code patterns that should not be translated.
/// </summary>
public class RoslynNotTranslateAble
{
    /// <summary>
    /// The "Cs cs" literal.
    /// </summary>
    public const string CsCs = "Cs cs";

    /// <summary>
    /// The protected void Page_Load method signature.
    /// </summary>
    public static string ProtectedVoidPageLoad = "protected void Page_Load";

    /// <summary>
    /// The public override void Page_Load method signature.
    /// </summary>
    public static string PublicOverrideVoidPageLoad = "public override void Page_Load";

    /// <summary>
    /// The class Dummy declaration text.
    /// </summary>
    public static string ClassDummy = "class Dummy";
}
