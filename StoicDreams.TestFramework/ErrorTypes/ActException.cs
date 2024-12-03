namespace StoicDreams;

public class ActException : TestFailException
{
    public ActException() { }
    public ActException(string message) : base(message) { }
    public ActException(string message, Exception innerException) : base(message, innerException) { }
}
