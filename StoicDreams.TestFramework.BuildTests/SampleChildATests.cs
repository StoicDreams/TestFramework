namespace StoicDreams.Tests;

public class SampleChildATests : TestFramework
{
    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomething_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleChildA>()
        .Act(arrangement => arrangement.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            string? result = arrangement.GetResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Something A: {input}");
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomethingElse_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleChildA>()
        .Act(arrangement => arrangement.Service.DoSomethingElse(input))
        .Assert(arrangement =>
        {
            string? result = arrangement.Service.Value;
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Something Else A: {input}");
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_RunPrivateMethod(string input)
    {
        ArrangeUnitTest<SampleChildA>()
        .Act(arrangement => arrangement.Service.RunPrivateMethod<SampleChildA, string>("PrivateMethod", input))
        .Assert(arrangement =>
        {
            string? result = arrangement.GetResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo(input);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_RunPrivateStaticMethod(string input)
    {
        ArrangeUnitTest<SampleChildA>()
        .Act(arrangement => arrangement.Service.RunPrivateMethod<SampleChildA, string>("PrivateStaticMethod", input))
        .Assert(arrangement =>
        {
            string? result = arrangement.GetResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo(input);
        });
    }
}