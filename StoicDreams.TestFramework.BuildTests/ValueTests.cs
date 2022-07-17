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

		actions.Act(value => TestTranslation((string)value));

		actions.Assert(result => result.Should().Be(expectedResult));
	}

	private string TestTranslation(string input)
	{
		char[] array = input.ToCharArray();
		Array.Reverse(array);
		return string.Join("", array);
	}
}
