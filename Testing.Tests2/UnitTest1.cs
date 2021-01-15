using Xunit;

namespace Testing.Tests2
{

[AutoTheory.UseTestOutputHelper] //This should not create a warning because we have DontAddAutoTheoryNamespaceAttribute
public partial class UnitTest1
{
    [Fact]
    public void Test1() { }
}

}
