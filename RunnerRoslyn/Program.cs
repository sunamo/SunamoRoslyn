namespace RunnerRoslyn;
using SunamoRoslyn.Tests;

internal class Program
{
    static void Main()
    {
        RoslynCommentServiceTests t = new();
        t.RemoveCommentsTest();
    }
}