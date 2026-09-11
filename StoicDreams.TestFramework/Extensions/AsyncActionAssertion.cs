using System.Runtime.CompilerServices;

namespace StoicDreams;

public class AsyncActionAssertion
{
    private readonly Func<Task> Subject;

    internal AsyncActionAssertion(Func<Task> subject)
    {
        Subject = subject;
    }

    public async Task ThrowAsync<TException>([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TException : Exception
    {
        try
        {
            await Subject();
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

    public async Task NotThrowAsync([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            await Subject();
        }
        catch (Exception ex)
        {
            throw new TestFailException($"Expected no exception but {ex.GetType().Name} was thrown: {ex.Message} in {memberName} at {filePath}:{lineNumber}.");
        }
    }
}
