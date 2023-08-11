namespace StoicDreams.Tests;

public class SampleChildBTests : TestFramework
{
    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomething_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleChildB>(options =>
        {
            options.GetService<ISampleChildA>().DoSomething(input).Returns($"Mock A: {input}");
        }, MockTypes.NSubstitute)
        .Act(arrangment => arrangment.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            string? result = arrangement.GetResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Something B: Mock A: {input}");
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomethingElse_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleChildB>(options =>
        {
            options.GetService<ISampleChildA>(mock =>
            {
                mock.Value.Returns($"Mock A: {input}");
            });
        }, MockTypes.NSubstitute)
        .Act(arrangment => arrangment.Service.DoSomethingElse(input))
        .Assert(arrangement =>
        {
            string? result = arrangement.Service.Value;
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Something Else B: Mock A: {input}");
            arrangement.GetService<ISampleChildA>().Received().DoSomethingElse(input);
        });
    }
}
