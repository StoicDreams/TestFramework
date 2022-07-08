namespace StoicDreams.Tests;

public class SampleChildATests : TestFramework
{
	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_DoSomething_ReturnsExpectedData(string input)
	{
		IActions<SampleChildA> actions = ArrangeUnitTest<SampleChildA>(options =>
		{
		});

		actions.Act(arrangment => arrangment.Service.DoSomething(input));

		actions.Assert(arrangement =>
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
		IActions<SampleChildA> actions = ArrangeUnitTest<SampleChildA>(options =>
		{
		});

		actions.Act(arrangment => arrangment.Service.DoSomethingElse(input));

		actions.Assert(arrangement =>
		{
			string? result = arrangement.Service.Value;
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().BeEquivalentTo($"Something Else A: {input}");
		});
	}
}