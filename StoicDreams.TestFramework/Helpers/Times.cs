namespace StoicDreams;

public class Times
{
    private int _start = 0;
    private int _end = 0;

    public Times(int times)
    {
        _start = times;
        _end = times;
    }

    public Times(int start, int end)
    {
        _start = start;
        _end = end;
    }

    public static Times Never => new(0);

    public static Times Once => new(1);

    public static Times Range(int start, int end) => new(start, end);

    public void Deconstruct(out int from, out int to)
    {
        from = _start;
        to = _end;
    }

    public static implicit operator int(Times times) => times._start;
    public static implicit operator (int from, int to)(Times times) => (times._start, times._end);
}

