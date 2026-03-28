namespace SunamoRoslyn;

/// <summary>
/// Provides a code fix that converts type names to uppercase for diagnostics reported by <see cref="RoslynAnalyzer"/>.
/// </summary>
public class RoslynCodeFixProvider : CodeFixProvider
{
    private const string title = "Make uppercase";

    /// <summary>
    /// Gets the diagnostic IDs that this provider can fix.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(RoslynAnalyzer.DiagnosticId); }
    }

    /// <summary>
    /// Gets the fix all provider for batch fixing.
    /// </summary>
    /// <returns>The batch fix all provider.</returns>
    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    /// <summary>
    /// Registers code fixes for the given diagnostics in the context.
    /// </summary>
    /// <param name="context">The code fix context containing diagnostics to fix.</param>
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

    /// <summary>
    /// Renames a type declaration to uppercase in the entire solution.
    /// </summary>
    /// <param name="document">The document containing the type declaration.</param>
    /// <param name="typeDeclaration">The type declaration syntax node to rename.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A new solution with the type name uppercased.</returns>
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
