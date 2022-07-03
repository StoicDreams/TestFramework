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

	[Theory]
	[InlineData("Test One", "Else One")]
	[InlineData("Test Two", "Else Two")]
	public void Verify_Raw_Integration_Test(string inputDoSomething, string inputDoSomethingElse)
	{
		IActions<ISampleParent> actions = ArrangeIntegrationTest<ISampleParent>(startupHandlers: SampleStartup.ConfigureServices);

		actions.Act(arrangement =>
		{
			arrangement.Service.DoSomethingElse(inputDoSomethingElse);
			return arrangement.Service.DoSomething(inputDoSomething);
		});

		actions.Assert(arrangement =>
		{
			string? somethingResult = arrangement.GetResult<string>();
			string elseResult = arrangement.Service.Value;
			somethingResult.Should().NotBeNullOrWhiteSpace();
			elseResult.Should().NotBeNullOrWhiteSpace();
			somethingResult.Should().NotBeEquivalentTo(elseResult);
			Assert.Equal($"Parent: Something A: {inputDoSomething} - Something B: Something A: {inputDoSomething}", somethingResult);
			Assert.Equal($"Parent: Something Else A: {inputDoSomethingElse} - Something Else B: Something Else A: {inputDoSomethingElse}", elseResult);
		});
	}

	[Theory]
	[InlineData("Test One", "Else One")]
	[InlineData("Test Two", "Else Two")]
	public void Verify_Itegration_Testing_Overwriting_Service_With_Mock(string inputDoSomething, string inputDoSomethingElse)
	{
		IActions<ISampleParent> actions = ArrangeIntegrationTest<ISampleParent>(options =>
		{
			options.ReplaceServiceWithMock<ISampleChildB>(mock =>
			{
				mock.Setup(m => m.DoSomething(inputDoSomething)).Returns($"Mocked {inputDoSomething}");
				mock.Setup(m => m.DoSomethingElse(inputDoSomethingElse)).Verifiable();
				mock.Setup(m => m.Value).Returns($"Mocked {inputDoSomethingElse}");
			});
		}, SampleStartup.ConfigureServices);

		actions.Act(arrangement =>
		{
			arrangement.Service.DoSomethingElse(inputDoSomethingElse);
			return arrangement.Service.DoSomething(inputDoSomething);
		});

		actions.Assert(arrangement =>
		{
			string? somethingResult = arrangement.GetResult<string>();
			string elseResult = arrangement.Service.Value;
			somethingResult.Should().NotBeNullOrWhiteSpace();
			elseResult.Should().NotBeNullOrWhiteSpace();
			somethingResult.Should().NotBeEquivalentTo(elseResult);
			Assert.Equal($"Parent: Something A: {inputDoSomething} - Mocked {inputDoSomething}", somethingResult);
			Assert.Equal($"Parent: Something Else A: {inputDoSomethingElse} - Mocked {inputDoSomethingElse}", elseResult);
		});
	}

	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_Custom_Test(string input)
	{
		IActions<ISampleParent> actions = ArrangeTest<ISampleParent>(options =>
		{
			options.AddService<ISampleParent, SampleParent>();
			options.AddMock<ISampleChildA>();
			options.AddMock<ISampleChildB>();
			options.AddService<SampleChildA>(() => { return new SampleChildA(); });
			options.AddService<SampleChildB>();
		});

		actions.Act(arrangement => arrangement.Service.DoSomething(input));

		actions.Assert(arrangement =>
		{
			Assert.Equal($"Parent:  - ", arrangement.GetResult<string>());
		});
	}
}
