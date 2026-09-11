using System.Runtime.CompilerServices;

namespace StoicDreams;

public class ActionAssertion
{
    private readonly Action Subject;

    internal ActionAssertion(Action subject)
    {
        Subject = subject;
    }

    public void Throw<TException>([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TException : Exception
    {
        try
        {
            Subject();
        }
        catch (TException)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new TestFailException($"Expected {typeof(TException).Name} but {ex.GetType().Name} was thrown in {memberName} at {filePath}:{lineNumber}.");
        }

        throw new TestFailException($"Expected {typeof(TException).Name} but no exception was thrown in {memberName} at {filePath}:{lineNumber}.");
    }

    public void NotThrow([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            Subject();
        }
        catch (Exception ex)
        {
            throw new TestFailException($"Expected no exception but {ex.GetType().Name} was thrown: {ex.Message} in {memberName} at {filePath}:{lineNumber}.");
        }
    }
}
