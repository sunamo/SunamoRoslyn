namespace SunamoRoslyn;

/// <summary>
/// Represents the roslyn code fix provider type.
/// </summary>
public class RoslynCodeFixProvider : CodeFixProvider
{
    private const string title = "Make uppercase";

    /// <summary>
    /// The fixable diagnostic ids.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(RoslynAnalyzer.DiagnosticId); }
    }

    /// <summary>
    /// Get fix all provider.
    /// </summary>
    /// <param name="RoslynAnalyzer.DiagnosticId">The roslyn analyzer diagnostic id.</param>
    /// <returns>The get fix all provider result.</returns>
    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    /// <summary>
    /// Register code fixes async.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The register code fixes async result.</returns>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the type declaration identified by the diagnostic.
        var declaration = root!.FindToken(diagnosticSpan.Start).Parent!.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: title,
                createChangedSolution: cancellationToken => MakeUppercaseAsync(context.Document, declaration, cancellationToken),
                equivalenceKey: title),
            diagnostic);
    }

    private async Task<Solution> MakeUppercaseAsync(Document document, TypeDeclarationSyntax typeDeclaration, CancellationToken cancellationToken)
    {
        // Compute new uppercase name.
        var identifierToken = typeDeclaration.Identifier;
        var newName = identifierToken.Text.ToUpperInvariant();

        // Get the symbol representing the type to be renamed.
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        var typeSymbol = semanticModel!.GetDeclaredSymbol(typeDeclaration, cancellationToken);

        // Produce a new solution that has all references to that type renamed, including the declaration.
        var originalSolution = document.Project.Solution;
        var optionSet = originalSolution.Workspace.Options;
#pragma warning disable CS0618
        var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol!, newName, optionSet, cancellationToken).ConfigureAwait(false);
#pragma warning restore CS0618

        // Return the new solution with the now-uppercase type name.
        return newSolution;
    }
}
