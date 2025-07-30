using FluentAssertions;
using SunamoRoslyn.Services;
using SunamoRoslyn.Tests._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoRoslyn.Tests;
public class RoslynCommentServiceTests
{
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
        // za d je mezera
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
