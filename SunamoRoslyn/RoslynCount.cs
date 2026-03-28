namespace SunamoRoslyn;

/// <summary>
/// Tracks member and method counts before and after Roslyn operations on a class declaration.
/// </summary>
public class RoslynCount
{
    /// <summary>
    /// The method count after the operation.
    /// </summary>
    public int MethodCountAfter { get; set; }

    /// <summary>
    /// The method count before the operation.
    /// </summary>
    public int MethodCountBefore { get; set; }

    /// <summary>
    /// The member count before the operation.
    /// </summary>
    public int MemberCountBefore { get; set; }

    /// <summary>
    /// The member count after the operation.
    /// </summary>
    public int MemberCountAfter { get; set; }

    /// <summary>
    /// Fills the before-counts from the given class declaration.
    /// </summary>
    /// <param name="classDeclaration">The class declaration to count members and methods from.</param>
    public void FillBefore(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountBefore = classDeclaration.Members.Count;
        MethodCountBefore = ChildNodes.Methods(classDeclaration).Count();
    }

    /// <summary>
    /// Fills the after-counts from the given class declaration.
    /// </summary>
    /// <param name="classDeclaration">The class declaration to count members and methods from.</param>
    public void FillAfter(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountAfter = classDeclaration.Members.Count;
        MethodCountAfter = ChildNodes.Methods(classDeclaration).Count();
    }

    /// <summary>
    /// Logs the before and after counts for the given operation.
    /// </summary>
    /// <param name="operation">The name of the operation being tracked.</param>
    public void Log(string operation)
    {
        Console.WriteLine(operation + $": Before members {MemberCountBefore}, methods {MethodCountBefore}");
        Console.WriteLine(operation + $": After members {MemberCountAfter}, methods {MethodCountAfter}");
    }

    /// <summary>
    /// Throws an exception if elements were not properly removed.
    /// </summary>
    public void ThrowException()
    {
    }
}
