namespace SunamoRoslyn;

/// <summary>
/// A Roslyn diagnostic analyzer that reports named types containing lowercase letters in their names.
/// </summary>
#pragma warning disable RS1036, RS1038, RS1041
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RoslynAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The unique identifier for diagnostics produced by this analyzer.
    /// </summary>
    public const string DiagnosticId = "Roslyn";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Naming";

#pragma warning disable RS2008
    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
#pragma warning restore RS2008

    /// <summary>
    /// Gets the set of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

    /// <summary>
    /// Initializes the analyzer by registering analysis actions.
    /// </summary>
    /// <param name="context">The analysis context to register actions on.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        // Find just those named type symbols with names containing lowercase letters.
        if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
        {
            // For all such symbols, produce a diagnostic.
            var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
#pragma warning restore RS1036, RS1038, RS1041
