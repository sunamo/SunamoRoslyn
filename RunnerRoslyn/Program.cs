namespace RunnerRoslyn;
using SunamoRoslyn.Tests;

/// <summary>
/// Entry point for the runner application.
/// </summary>
internal class Program
{
    static void Main()
    {
        RoslynCommentServiceTests t = new();
        t.RemoveCommentsTest();
    }
}
