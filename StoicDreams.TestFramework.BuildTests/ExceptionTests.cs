namespace StoicDreams.Tests;

public class ExceptionTests : TestFramework
{
    [Fact]
    public void VerifyConsoleLogThrowsErrorInUnitTest()
    {
        ArrangeUnitTest<SampleChildA>(arrange =>
        {
            arrange.WatchConsole("throw me!");
        })
        .ActThrowsException<ConsoleWatchException>(act =>
        {
            Console.WriteLine("throw me!");
            throw new ConsoleWatchException("Thrown!");
        })
        .Assert(assert =>
        {

        });
    }

    [Fact]
    public void VerifyConsoleLogThrowsErrorInTest()
    {
        ArrangeTest(arrange =>
        {
            arrange.WatchConsole("throw me!");
            ISampleChildA childA = new SampleChildA();
            return childA;
        })
        .ActThrowsException<ConsoleWatchException>(act =>
        {
            Console.WriteLine("throw me!");
            throw new ConsoleWatchException("Thrown!");
        })
        .Assert(assert =>
        {

        });
    }
}
