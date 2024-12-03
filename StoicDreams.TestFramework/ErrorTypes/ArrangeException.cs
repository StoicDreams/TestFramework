namespace StoicDreams;

public class ArrangeException : TestFailException
{
    public ArrangeException() { }
    public ArrangeException(string message) : base(message) { }
    public ArrangeException(string message, Exception innerException) : base(message, innerException) { }
}
