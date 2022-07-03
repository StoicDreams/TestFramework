using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoicDreams.Tests;

public class SampleChildBTests : TestFramework
{
	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_DoSomething_ReturnsExpectedData(string input)
	{
		IActions<SampleChildB> actions = ArrangeUnitTest<SampleChildB>(options =>
		{
			options.GetMock<ISampleChildA>().Setup(m => m.DoSomething(input)).Returns($"Mock A: {input}");
		});

		actions.Act(arrangment => arrangment.Service.DoSomething(input));

		actions.Assert(arrangement =>
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
		IActions<SampleChildB> actions = ArrangeUnitTest<SampleChildB>(options =>
		{
			options.GetMock<ISampleChildA>(mock =>
			{
				mock.Setup(m => m.DoSomethingElse(input)).Verifiable();
				mock.Setup(m => m.Value).Returns($"Mock A: {input}");
			});
		});

		actions.Act(arrangment => arrangment.Service.DoSomethingElse(input));

		actions.Assert(arrangement =>
		{
			string? result = arrangement.Service.Value;
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().BeEquivalentTo($"Something Else B: Mock A: {input}");
			arrangement.GetMock<ISampleChildA>().Verify();
		});
	}
}
