namespace StoicDreams.Tests.Samples;

internal interface ISampleChildA
{
	string DoSomething(string input);
	void DoSomethingElse(string input);
	string Value { get; }
}
