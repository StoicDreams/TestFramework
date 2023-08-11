namespace StoicDreams.Tests;

public class SampleMoqTransition : TestFramework
{
    [Fact]
    public void VerifyReturnsWithObject()
    {
        Mock<ISampleParent> mockParent = new();
        mockParent.Setup(m => m.DoSomething(IsAny<string>()))
            .Returns("World");

        string something = mockParent.Object.DoSomething("Hello");
        Assert.Equal("World", something);
    }

    [Fact]
    public void VerifyReturnsOnMethodWithOneArgument()
    {
        string result = "Missing";
        Mock<ISampleParent> mockParent = new();
        mockParent.Setup(m => m.DoSomething(IsAny<string>()))
            .Returns<string>(input => result = input);

        string something = mockParent.Object.DoSomething("Hello");
        Assert.Equal("Hello", result);
        Assert.Equal("Hello", something);
    }

    [Fact]
    public void VerifyReturnsOnMethodWithTwoArguments()
    {
        string resultA = "Missing";
        int resultB = 0;
        Mock<ISampleParent> mockParent = new();
        mockParent.Setup(m => m.DoSomethingMoreArgs(IsAny<string>(), IsAny<int>()))
            .Returns<string, int>((a, b) =>
            {
                resultA = a;
                resultB = b;
                return $"{b}-{a}";
            });

        string something = mockParent.Object.DoSomethingMoreArgs("Hello", 3);
        Assert.Equal("Hello", resultA); ;
        Assert.Equal(3, resultB);
        Assert.Equal("3-Hello", something);
    }

    [Fact]
    public void VerifyCallback()
    {
        string result = "Missing";
        Mock<ISampleParent> mockParent = new();
        mockParent.Setup(m => m.DoSomething(IsAny<string>()))
            .Callback((string input) => result = input);

        string something = mockParent.Object.DoSomething("Hello");
        Assert.Equal("Hello", result);
        Assert.Equal("Hello", something);
    }

    [Fact]
    public void VerifyParentInterfaceUsingMock()
    {
        Mock<ISampleParent> mockParent = new();
        mockParent.Setup(m => m.DoSomethingElse(IsAny<string>()));
    }

    [Fact]
    public void VerifyChildAInterfaceUsingMock()
    {
        Mock<ISampleChildA> mockParent = new();
        mockParent.Setup(m => m.DoSomethingElse(IsAny<string>()));
    }

    [Fact]
    public void VerifyChildBInterfaceUsingMock()
    {
        Mock<ISampleChildB> mockParent = new();
        mockParent.Setup(m => m.DoSomethingElse(IsAny<string>()));
    }
}
