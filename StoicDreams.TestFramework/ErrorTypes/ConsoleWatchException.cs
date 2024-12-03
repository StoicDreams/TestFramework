namespace StoicDreams;

public class ConsoleWatchException : TestFailException
{
    public ConsoleWatchException() { }
    public ConsoleWatchException(string message) : base(message) { }
    public ConsoleWatchException(string message, Exception innerException) : base(message, innerException) { }
}
