namespace StoicDreams.Tests;

public class ExceptionTests : TestFrameworkBlazor
{
    [Fact]
    public void Verify_Console_Throws_Exception()
    {
        ArrangeRenderTest<World>(arrange =>
        {
            arrange.WatchConsole("throw me!");
        })
        .ActThrowsException<ConsoleWatchException>(act =>
        {
            Console.WriteLine("throw me!");
        })
        .Assert(a =>
        {
        });
    }
}
