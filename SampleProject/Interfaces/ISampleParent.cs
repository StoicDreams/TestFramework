namespace SampleProject;

internal interface ISampleParent
{
    string DoSomething(string input);
    void DoSomethingElse(string input);
    string Value { get; }
    string DoSomethingMoreArgs(string input, int number);
}
