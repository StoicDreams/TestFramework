namespace StoicDreams;

public class AssertException : TestFailException
{
    public AssertException() { }
    public AssertException(string message) : base(message) { }
    public AssertException(string message, Exception innerException) : base(message, innerException) { }
}
