namespace SampleProject;

internal class SampleChildB
{
	public SampleChildB(ISampleChildA childA)
	{
		ChildA = childA;
	}

	public string DoSomething(string input)
	{
		return $"Something B: {ChildA.DoSomething(input)}";
	}

	public void DoSomethingElse(string input)
	{
		ChildA.DoSomethingElse(input);
		Value = $"Something Else B: {ChildA.Value}";
	}

	public string Value { get; private set; } = string.Empty;

	private ISampleChildA ChildA { get; }
}
