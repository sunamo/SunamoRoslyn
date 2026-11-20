namespace SunamoRoslyn;

/// <summary>
/// Represents the roslyn count type.
/// </summary>
public class RoslynCount
{
    /// <summary>
    /// Gets or sets the method count after.
    /// </summary>
    public int MethodCountAfter { get; set; }

    /// <summary>
    /// Gets or sets the method count before.
    /// </summary>
    public int MethodCountBefore { get; set; }

    /// <summary>
    /// Gets or sets the member count before.
    /// </summary>
    public int MemberCountBefore { get; set; }

    /// <summary>
    /// Gets or sets the member count after.
    /// </summary>
    public int MemberCountAfter { get; set; }

    /// <summary>
    /// Fill before.
    /// </summary>
    /// <param name="classDeclaration">The class declaration.</param>
    public void FillBefore(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountBefore = classDeclaration.Members.Count;
        MethodCountBefore = ChildNodes.Methods(classDeclaration).Count();
    }

    /// <summary>
    /// Fill after.
    /// </summary>
    /// <param name="classDeclaration">The class declaration.</param>
    /// <returns>The fill after result.</returns>
    public void FillAfter(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountAfter = classDeclaration.Members.Count;
        MethodCountAfter = ChildNodes.Methods(classDeclaration).Count();
    }

    /// <summary>
    /// Log.
    /// </summary>
    /// <param name="classDeclaration">The class declaration.</param>
    public void Log(string operation)
    {
        Console.WriteLine(operation + $": Before members {MemberCountBefore}, methods {MethodCountBefore}");
        Console.WriteLine(operation + $": After members {MemberCountAfter}, methods {MethodCountAfter}");
    }

    /// <summary>
    /// Throw exception.
    /// </summary>
    /// <param name="classDeclaration">The class declaration.</param>
    public void ThrowException()
    {
    }
}
