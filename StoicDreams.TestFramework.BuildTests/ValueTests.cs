namespace StoicDreams.Tests;

public class ValueTests : TestFramework
{
	[Theory]
	[InlineData("", "")]
	[InlineData("cba", "abc")]
	[InlineData("crba", "abrc")]
	public void Verify_Testing_Translation_Of_Data(string expectedResult, string input)
	{
		IActions actions = ArrangeUnitTest(() => input);

		actions.Act((string value) => TestTranslation(value));

		actions.Assert((string? result) => result.Should().Be(expectedResult));
	}

	[Fact]
	public void Verify_Exception_Thowing_Test()
	{
		IActions actions = ArrangeUnitTest();

		actions.ActThrowsException(() => throw new Exception("Test"));

		actions.Assert((Exception? result) => result.IsNotNull().Message.Should().Be("Test"));
	}

	[Fact]
	public void Verify_Exception_Thowing_Test_Fails_To_Throw_Exception()
	{
		IActions actions = ArrangeUnitTest();

		Assert.Throws<Exception>(() => actions.ActThrowsException(() => { }));
	}

	private string TestTranslation(string input)
	{
		char[] array = input.ToCharArray();
		Array.Reverse(array);
		return string.Join("", array);
	}
}
