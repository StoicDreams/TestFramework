namespace StoicDreams;

internal class ConsoleHelper
{
    internal void WatchConsole(Action proc)
    {
        SetupConsole();
        proc.Invoke();
        CheckConsoleLogs();
    }

    internal async Task<object?> WatchConsole(Func<Task<object?>> proc)
    {
        SetupConsole();
        object? result = await proc.Invoke();
        CheckConsoleLogs();
        return result;
    }

    private void SetupConsole()
    {
        Logs.Close();
        Logs.Dispose();
        Logs = new();
        Console.SetOut(Logs);
    }

    private void CheckConsoleLogs()
    {
        Console.SetOut(ConsoleOriginalOut);
        Console.Write(Logs);
        if (ConsoleWatch.Length == 0) return;
        if (Logs.ToString() is not string logs || string.IsNullOrWhiteSpace(logs)) return;
        foreach (string message in ConsoleWatch)
        {
            if (logs.Contains(message, StringComparison.OrdinalIgnoreCase))
            {
                throw new ConsoleWatchException($"Test Failed: Console contained `{message}`");
            }
        }
    }


    internal string[] ConsoleWatch { get; set; } = [];
    private StringWriter Logs { get; set; } = new();
    private TextWriter ConsoleOriginalOut { get; } = Console.Out;
}
