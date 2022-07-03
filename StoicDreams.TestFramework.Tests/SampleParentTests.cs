namespace StoicDreams.Tests;

public class SampleParentTests : TestFramework
{
	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_DoSomething_ReturnsExpectedData(string input)
	{
		IActions<SampleParent> actions = ArrangeUnitTest<SampleParent>(options =>
		{
			options.GetMock<ISampleChildA>().Setup(m => m.DoSomething(input)).Returns($"Mock A: {input}");
			options.GetMock<ISampleChildB>().Setup(m => m.DoSomething(input)).Returns($"Mock B: {input}");
		});

		actions.Act(arrangment => arrangment.Service.DoSomething(input));

		actions.Assert(arrangement =>
		{
			string? result = arrangement.GetResult<string>();
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
		});
	}

	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_DoSomethingElse_ReturnsExpectedData(string input)
	{
		IActions<SampleParent> actions = ArrangeUnitTest<SampleParent>(options =>
		{
			options.GetMock<ISampleChildA>(mock =>
			{
				mock.Setup(m => m.DoSomethingElse(input)).Verifiable();
				mock.Setup(m => m.Value).Returns($"Mock A: {input}");
			});
			options.GetMock<ISampleChildB>(mock =>
			{
				mock.Setup(m => m.DoSomethingElse(input)).Verifiable();
				mock.Setup(m => m.Value).Returns($"Mock B: {input}");
			});
		});

		actions.Act(arrangment => arrangment.Service.DoSomethingElse(input));

		actions.Assert(arrangement =>
		{
			string? result = arrangement.Service.Value;
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
			arrangement.GetMock<ISampleChildA>().Verify();
			arrangement.GetMock<ISampleChildB>().Verify();
		});
	}
}
