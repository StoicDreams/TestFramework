namespace StoicDreams.Tests;

public class ValueTests : TestFramework
{
    [Theory]
    [InlineData("Test A")]
    [InlineData("Test B")]
    public void Verify_Test_Custom_Setup(string input)
    {
        IActions actions = ArrangeUnitTest(() => new Exception(input));

        actions.Act((Exception value) => value.Message);

        actions.Assert((string? result) => result.Should().Be(input));
    }


    [Theory]
    [InlineData("", "")]
    [InlineData("cba", "abc")]
    [InlineData("crba", "abrc")]
    public void Verify_Testing_Translation_Of_Data(string expectedResult, string input)
    {
        IActions actions = ArrangeUnitTest(() => input);

        actions.Act((string value) => MockReverseString(value));

        actions.Assert((string? result) => result.Should().Be(expectedResult));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("cba", "abc")]
    [InlineData("crba", "abrc")]
    public void Verify_Testing_Async_Translation_Of_Data(string expectedResult, string input)
    {
        IActions actions = ArrangeUnitTest(() => input);

        actions.Act(async (string value) => await MockReverseStringAsync(value));

        actions.Assert((string? result) => result.Should().Be(expectedResult));
    }

    [Fact]
    public void Verify_Exception_Thowing_Test()
    {
        IActions actions = ArrangeUnitTest();

        actions.ActThrowsException(() => MockReverseString("anything", true));

        actions.Assert((Exception? result) => result.IsNotNull().Message.Should().Be("Mocking an unexpected exception"));
    }

    [Fact]
    public void Verify_Exception_Thowing_Test_Fails_To_Throw_Exception()
    {
        IActions actions = ArrangeUnitTest();

        actions.ActThrowsException(() => actions.ActThrowsException(() => { }));

        actions.Assert((Exception? value) => value.IsNotNull().Message.Should().Be("Exception was expected but no exception was thrown"));
    }

    [Fact]
    public void Verify_Async_Exception()
    {
        IActions actions = ArrangeUnitTest();

        actions.ActThrowsException(async () => await MockReverseStringAsync("anything", true));

        actions.Assert((Exception? value) => value.IsNotNull().Message.Should().Be("Mocking an unexpected exception"));
    }

    [Fact]
    public void Verify_IsNotNull_ReleasesNullStateCheck()
    {
        string? instance = TestGetNullableString(false);

        instance.IsNotNull();

        Assert.NotEmpty(instance.ToString());
    }

    [Fact]
    public void Verify_IsNotNull_ThrowsErrorWhenNull()
    {
        string? instance = TestGetNullableString(true);

        Assert.Throws<NullReferenceException>(() =>
        {
            instance.IsNotNull();
        });
    }

    private string? TestGetNullableString(bool returnNull)
    {
        if (returnNull) { return null; }
        return "Hello World";
    }

    private string MockReverseString(string input, bool throwExeption = false)
    {
        if (throwExeption) { throw new Exception("Mocking an unexpected exception"); }
        char[] array = input.ToCharArray();
        Array.Reverse(array);
        return string.Join("", array);
    }

    private Task<string> MockReverseStringAsync(string input, bool throwExeption = false)
    {
        if (throwExeption) { throw new Exception("Mocking an unexpected exception"); }
        char[] array = input.ToCharArray();
        Array.Reverse(array);
        return Task.FromResult(string.Join("", array));
    }
}
