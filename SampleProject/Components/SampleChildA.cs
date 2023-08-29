namespace SampleProject;

internal class SampleChildA : ISampleChildA
{
    public string DoSomething(string input)
    {
        return $"Something A: {input}";
    }

    public void DoSomethingElse(string input)
    {
        Value = $"Something Else A: {input}";
    }

    public string Value { get; private set; } = string.Empty;

    private string PrivateMethod(string input) => input;

    private static string PrivateStaticMethod(string input) => input;
}
