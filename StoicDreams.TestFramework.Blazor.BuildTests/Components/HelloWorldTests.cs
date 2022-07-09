namespace StoicDreams;

public class HelloWorldTests : TestFrameworkBlazor
{
	[Fact]
	public void Verify_Render_With_Mocked_Children()
	{
		bool renderIsDisposed = false;
		IRenderActions<HelloWorld> actions = ArrangeRenderTest<HelloWorld>(options =>
		{
			options.AddStub<Hello>("MockedHello");
			options.AddStub<World>("MockedWorld");
			options.Parameters.Add("OnDispose", () => { renderIsDisposed = true; });
		});

		actions.Act(a => a.Render.Markup);

		actions.Assert(a =>
		{
			a.Render.Find("div").ToMarkup().Should().Contain("MockedWorld");
			a.Render.Find("div").ToMarkup().Should().NotContain("MockedHello");
			renderIsDisposed.Should().BeFalse();
		});

		actions.Act(a => a.Render.SetParametersAndRender(b => b.Add(e => e.ToggleA, true)));

		actions.Assert(a =>
		{
			a.Render.Find("div").ToMarkup().Should().NotContain("MockedWorld");
			a.Render.Find("div").ToMarkup().Should().Contain("MockedHello");
			renderIsDisposed.Should().BeFalse();
		});

		actions.Act(a => a.DetachRender());

		actions.Assert(a =>
		{
			renderIsDisposed.Should().BeTrue();
			a.GetResult<string>().Should().Contain("The component has been removed from the render tree");
		});
	}
}