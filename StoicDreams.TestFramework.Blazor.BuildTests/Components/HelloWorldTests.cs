namespace StoicDreams;

public class HelloWorldTests : TestFrameworkBlazor
{
	[Fact]
	public void Verify_Render_With_Mocked_Children()
	{
		IRenderActions<HelloWorld> actions = ArrangeRenderTest<HelloWorld>(options =>
		{
			options.AddStub<Hello>("MockedHello");
			options.AddStub<World>("MockedWorld");
		});

		actions.Act(a => a.Render.Markup);

		actions.Assert(a =>
		{
			a.Render.Find("div").ToMarkup().Should().Contain("MockedWorld");
			a.Render.Find("div").ToMarkup().Should().NotContain("MockedHello");
		});

		actions.Act(a => a.Render.SetParametersAndRender(b => b.Add(e => e.ToggleA, true)));

		actions.Assert(a =>
		{
			a.Render.Find("div").ToMarkup().Should().NotContain("MockedWorld");
			a.Render.Find("div").ToMarkup().Should().Contain("MockedHello");
		});
	}
}