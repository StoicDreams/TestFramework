namespace SampleProject;

internal class SampleParent : ISampleParent
{
    public SampleParent(ISampleChildA childA, ISampleChildB childB)
    {
        ChildA = childA;
        ChildB = childB;
    }

    public string DoSomething(string input)
    {
        return $"Parent: {ChildA.DoSomething(input)} - {ChildB.DoSomething(input)}";
    }

    public void DoSomethingElse(string input)
    {
        ChildA.DoSomethingElse(input);
        ChildB.DoSomethingElse(input);
        Value = $"Parent: {ChildA.Value} - {ChildB.Value}";
    }

    public string DoSomethingMoreArgs(string input, int number)
    {
        return $"{input}:{number}";
    }

    public string Value { get; private set; } = string.Empty;

    private ISampleChildA ChildA { get; }
    private ISampleChildB ChildB { get; }
}
