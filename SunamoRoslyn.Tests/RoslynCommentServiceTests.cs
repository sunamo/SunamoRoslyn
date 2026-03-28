using FluentAssertions;
using SunamoRoslyn.Services;
using SunamoRoslyn.Tests._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn.Tests;

/// <summary>
/// Tests for the RoslynCommentService class.
/// </summary>
public class RoslynCommentServiceTests
{
    /// <summary>
    /// Tests that RemoveComments correctly removes single-line, multi-line, and XML doc comments while preserving string literals containing comment-like patterns.
    /// </summary>
    [Fact]
    public void RemoveCommentsTest()
    {
        string input = "class A { string a = \"https://\"" + Environment.NewLine + @"//b
c
d /*e*/
/*haf
baf*/
f
}";
        // after d there is a space
        var expected = "class A { string a = \"https://\"" +
            Environment.NewLine + @"
c
d

f
}";
        RoslynCommentService roslynComment = new();
        var actual = roslynComment.RemoveComments(input);
        actual.Should().BeEquivalentTo(expected);
    }
}
