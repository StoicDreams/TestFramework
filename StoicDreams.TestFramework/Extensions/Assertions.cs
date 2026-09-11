namespace StoicDreams;

public static class Assertions
{
    public static AssertionRef<TItem> Should<TItem>(this TItem? item) => new(item);
    public static ActionAssertion Should(this Action action) => new(action);
    public static AsyncActionAssertion Should(this Func<Task> action) => new(action);
}
