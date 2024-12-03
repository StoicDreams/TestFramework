namespace StoicDreams;

public class TestFailException : Exception
{
    public TestFailException() { }
    public TestFailException(string message) : base(message) { }
    public TestFailException(string message, Exception innerException) : base(message, innerException) { }
}
