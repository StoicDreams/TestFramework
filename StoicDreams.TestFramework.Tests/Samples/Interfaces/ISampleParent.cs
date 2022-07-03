namespace StoicDreams.Tests.Samples;

internal interface ISampleParent
{
	string DoSomething(string input);
	void DoSomethingElse(string input);
	string Value { get; }
}
