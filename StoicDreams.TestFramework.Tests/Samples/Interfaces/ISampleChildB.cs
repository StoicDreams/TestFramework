namespace StoicDreams.Tests.Samples;

internal interface ISampleChildB
{
	string DoSomething(string input);
	void DoSomethingElse(string input);
	string Value { get; }
}
