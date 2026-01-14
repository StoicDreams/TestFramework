namespace StoicDreams;

public class HelloWorldTests : TestFrameworkBlazor
{
    [Fact]
    public void Verify_Render_With_Builder_Updates()
    {
        ArrangeRenderTest<World>(options =>
        {
            options.SetupComponentParameters<World>(b =>
            {
                b.AddChildContent(MockRender("Added Content"));
            });
        }, services =>
        {
            services.AddSub<ICache>();
            return services;
        })
        .Assert(a =>
        {
            Assert.Contains("Added Content", a.Render.Markup);
        });
    }

    [Fact]
    public void Verify_Render_With_Mocked_Children()
    {
        bool renderIsDisposed = false;
        ArrangeRenderTest<HelloWorld>(options =>
        {
            options.AddStub<Hello>("MockedHello");
            options.AddStub<World>("MockedWorld");
            options.Parameters.Add("OnDispose", () => { renderIsDisposed = true; });
        }, services =>
        {
            services.AddSub<ICache>();
            return services;
        })
        .Act(a => a.Render.Markup)
        .Assert(a =>
        {
            a.Render.Find("div").InnerHtml.Should().Contain("MockedWorld");
            a.Render.Find("div").InnerHtml.Should().NotContain("MockedHello");
            renderIsDisposed.Should().BeFalse();
        })
        .Act(a => a.Render.Render(b => b.Add(e => e.ToggleA, true)))
        .Assert(a =>
        {
            a.Render.Find("div").InnerHtml.Should().NotContain("MockedWorld");
            a.Render.Find("div").InnerHtml.Should().Contain("MockedHello");
            renderIsDisposed.Should().BeFalse();
        })
        .Act(a => a.DetachRender())
        .Assert(a =>
        {
            renderIsDisposed.Should().BeTrue();
            a.GetResult<string>().Should().Contain("The component has been removed from the render tree");
        });
    }

    [Fact]
    public void Verify_Render_Integration_Test()
    {
        bool renderIsDisposed = false;
        ArrangeRenderTest<HelloWorld>(options =>
        {
            options.Parameters.Add("OnDispose", () => { renderIsDisposed = true; });
        }, services => SampleBlazorLib.SampleStartup.Startup(services))
        .Act(a => a.Render.Markup)
        .Assert(a =>
        {
            a.Render.FindComponent<World>();
            a.FindComponent<World>();
            a.Render.Find("h3").InnerHtml.Should().Contain("World");
            a.Render.Find("h3").InnerHtml.Should().NotContain("Hello");
            renderIsDisposed.Should().BeFalse();
        })
        .Act(a => a.Render.Render(b => b.Add(e => e.ToggleA, true)))
        .Assert(a =>
        {
            a.Render.FindComponent<Hello>();
            a.FindComponent<Hello>();
            a.Render.Find("h3").InnerHtml.Should().NotContain("World");
            a.Render.Find("h3").InnerHtml.Should().Contain("Hello");
            renderIsDisposed.Should().BeFalse();
        })
        .Act(a => a.DetachRender())
        .Assert(a =>
        {
            renderIsDisposed.Should().BeTrue();
            a.GetResult<string>().Should().Contain("The component has been removed from the render tree");
        });
    }

    [Fact]
    public void Verify_Render_Integration_Test_AsyncAct_Properly_Throws_Exception()
    {
        try
        {
            ArrangeRenderTest<HelloWorld>(null, SampleBlazorLib.SampleStartup.Startup)
            .Act(async a =>
            {
                await Task.CompletedTask;
                throw new Exception("Something unexpected happened!");
            })
            .Assert(a =>
            {
                throw new Exception("This should not be reached!");
            });
        }
        catch (Exception ex)
        {
            if (ex.Message.StartsWith("Something unexpected happened!")) return;
        }
        throw new Exception("Testing failed to throw expected exception.");
    }

    [Fact]
    public void Verify_Render_Integration_Test_Act_Throws_Exception()
    {
        ArrangeRenderTest<HelloWorld>(null, SampleBlazorLib.SampleStartup.Startup)
        .ActThrowsException(async a =>
        {
            await Task.CompletedTask;
            throw new Exception("Something unexpected happened!");
        })
        .Assert(a =>
        {
            Assert.Equal("Something unexpected happened!", a.GetResult<string>());
        });
    }

    [Fact]
    public void Verify_Render_Integration_Test_Act_Throws_Explicit_Exception()
    {
        ArrangeRenderTest<HelloWorld>(null, SampleBlazorLib.SampleStartup.Startup)
        .ActThrowsException<InvalidCastException>(async a =>
        {
            await Task.CompletedTask;
            throw new InvalidCastException("Something unexpected happened!");
        })
        .Assert(a =>
        {
            Assert.Equal("Something unexpected happened!", a.GetResult<string>());
        });
    }
}