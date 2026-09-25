namespace SunamoRoslyn;

public class RoslynCount
{
    public int MethodCountAfter { get; set; }

    public int MethodCountBefore { get; set; }

    public int MemberCountBefore { get; set; }

    public int MemberCountAfter { get; set; }

    public void FillBefore(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountBefore = classDeclaration.Members.Count;
        MethodCountBefore = ChildNodes.Methods(classDeclaration).Count();
    }

    public void FillAfter(ClassDeclarationSyntax classDeclaration)
    {
        MemberCountAfter = classDeclaration.Members.Count;
        MethodCountAfter = ChildNodes.Methods(classDeclaration).Count();
    }

    public void Log(string operation)
    {
        Console.WriteLine(operation + $": Before members {MemberCountBefore}, methods {MethodCountBefore}");
        Console.WriteLine(operation + $": After members {MemberCountAfter}, methods {MethodCountAfter}");
    }

    public void ThrowException()
    {
    }
}
