using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace StoicDreams;

public static class Assertions
{
    public static AssertionRef<TItem> Should<TItem>(this TItem? item) => new(item);
    public static ActionAssertion Should(this Action action) => new(action);
    public static AsyncActionAssertion Should(this Func<Task> action) => new(action);
}

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

public class AssertionRef<TItem>
{
    internal TItem? Item;
    private static string ItemDisplay
    {
        get
        {
            Type type = typeof(TItem);
            if (!type.IsGenericType) return type.Name;
            string typeName = type.Name.Split('`')[0];
            string args = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));
            return $"{typeName}<{args}>";
        }
    }

    internal AssertionRef(TItem? item)
    {
        Item = item;
    }

    public void Be(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (EqualityComparer<TItem>.Default.Equals(Item, check)) return;
        Fail(nameof(Be), memberName, filePath, lineNumber);
    }

    public void BeApproximately(double expected, double precision, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            if (Item is IConvertible convertible)
            {
                double val = convertible.ToDouble(null);
                if (Math.Abs(val - expected) <= precision) return;
            }
        }
        catch { }
        Fail(nameof(BeApproximately), memberName, filePath, lineNumber);
    }

    public void BeAssignableTo<TExpected>([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is TExpected) return;
        Fail(nameof(BeAssignableTo), memberName, filePath, lineNumber);
    }

    public void BeEmpty([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is IEnumerable collection)
        {
            if (Item is string s && s.Length == 0) return;
            IEnumerator enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) return;
        }
        Fail(nameof(BeEmpty), memberName, filePath, lineNumber);
    }

    public void BeEquivalentTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false
        };

        string jsonItem = JsonSerializer.Serialize(Item, options);
        string jsonCheck = JsonSerializer.Serialize(check, options);

        if (jsonItem == jsonCheck) return;
        Fail(nameof(BeEquivalentTo), memberName, filePath, lineNumber);
    }

    public void BeFalse([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is bool item && !item) return;
        Fail(nameof(BeFalse), memberName, filePath, lineNumber);
    }

    public void BeGreaterThan(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) > 0) return;
        Fail(nameof(BeGreaterThan), memberName, filePath, lineNumber);
    }

    public void BeGreaterThanOrEqualTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) >= 0) return;
        Fail(nameof(BeGreaterThanOrEqualTo), memberName, filePath, lineNumber);
    }

    public void BeInRange<TComparable>(TComparable min, TComparable max, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TComparable : IComparable<TComparable>
    {
        if (Item is IComparable<TComparable> item && item.CompareTo(min) >= 0 && item.CompareTo(max) <= 0) return;
        Fail(nameof(BeInRange), memberName, filePath, lineNumber);
    }

    public void BeLessThan(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) < 0) return;
        Fail(nameof(BeLessThan), memberName, filePath, lineNumber);
    }

    public void BeLessThanOrEqualTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) <= 0) return;
        Fail(nameof(BeLessThanOrEqualTo), memberName, filePath, lineNumber);
    }

    public void BeNull([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is null) return;
        Fail(nameof(BeNull), memberName, filePath, lineNumber);
    }

    public void BeNullOrEmpty([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is null) return;
        if (Item is string item && string.IsNullOrEmpty(item)) return;
        if (Item is IEnumerable collection)
        {
            var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) return;
        }
        Fail(nameof(BeNullOrEmpty), memberName, filePath, lineNumber);
    }

    public void BeNullOrWhiteSpace([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is null) return;
        if (Item is string item && string.IsNullOrWhiteSpace(item)) return;
        Fail(nameof(BeNullOrWhiteSpace), memberName, filePath, lineNumber);
    }

    public void BeOfType<TExpected>([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is TExpected) return;
        Fail(nameof(BeOfType), memberName, filePath, lineNumber);
    }

    public void BeSameAs(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (ReferenceEquals(Item, check)) return;
        Fail(nameof(BeSameAs), memberName, filePath, lineNumber);
    }

    public void BeTrue([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is bool item && item) return;
        Fail(nameof(BeTrue), memberName, filePath, lineNumber);
    }

    public void Contain<TCheck>(TCheck check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        if (Item is string s && check is string checkStr)
        {
            success = s.Contains(checkStr);
        }
        else if (Item is IEnumerable enumerable)
        {
            foreach (object? element in enumerable)
            {
                if (element?.Equals(check) ?? check is null)
                {
                    success = true;
                    break;
                }
            }
        }
        if (success) return;
        Fail(nameof(Contain), memberName, filePath, lineNumber);
    }

    public void ContainKey<TKey>(TKey key, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (key is not null && Item is IDictionary dictionary && dictionary.Contains(key)) return;
        Fail(nameof(ContainKey), memberName, filePath, lineNumber);
    }

    public void ContainSingle([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is IEnumerable collection)
        {
            int count = 0;
            foreach (object? _ in collection)
            {
                count++;
                if (count > 1) break;
            }
            if (count == 1) return;
        }
        Fail(nameof(ContainSingle), memberName, filePath, lineNumber);
    }

    public void EndWith(string check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string s && s.EndsWith(check)) return;
        Fail(nameof(EndWith), memberName, filePath, lineNumber);
    }

    public void HaveCount(int expectedCount, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is ICollection collection && collection.Count == expectedCount) return;
        if (Item is string s && s.Length == expectedCount) return;
        if (Item is IEnumerable en)
        {
            int count = 0;
            foreach (object? _ in en) count++;
            if (count == expectedCount) return;
        }
        Fail(nameof(HaveCount), memberName, filePath, lineNumber);
    }

    public void HaveElementAt<TExpectation>(int index, TExpectation expected, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is IEnumerable collection)
        {
            int currentIndex = 0;
            foreach (object? element in collection)
            {
                if (currentIndex == index)
                {
                    if (element?.Equals(expected) ?? expected is null) return;
                    break;
                }
                currentIndex++;
            }
        }
        Fail(nameof(HaveElementAt), memberName, filePath, lineNumber);
    }

    public void MatchRegex(string pattern, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string s && Regex.IsMatch(s, pattern)) return;
        Fail(nameof(MatchRegex), memberName, filePath, lineNumber);
    }


    public void NotBeApproximately(double expected, double precision, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            if (Item is IConvertible convertible)
            {
                double val = convertible.ToDouble(null);
                if (Math.Abs(val - expected) > precision) return;
            }
        }
        catch { }
        Fail(nameof(NotBeApproximately), memberName, filePath, lineNumber);
    }


    public void NotBeEmpty([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is IEnumerable collection)
        {
            if (Item is string s && s.Length > 0) return;
            IEnumerator enumerator = collection.GetEnumerator();
            if (enumerator.MoveNext()) return;
        }
        Fail(nameof(NotBeEmpty), memberName, filePath, lineNumber);
    }

    public void NotBeEquivalentTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false
        };

        string jsonItem = JsonSerializer.Serialize(Item, options);
        string jsonCheck = JsonSerializer.Serialize(check, options);

        if (jsonItem != jsonCheck) return;
        Fail(nameof(NotBeEquivalentTo), memberName, filePath, lineNumber);
    }


    public void NotBeInRange<TComparable>(TComparable min, TComparable max, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TComparable : IComparable<TComparable>
    {
        if (Item is IComparable<TComparable> item && (item.CompareTo(min) < 0 || item.CompareTo(max) > 0)) return;
        Fail(nameof(NotBeInRange), memberName, filePath, lineNumber);
    }


    public void NotBeNull([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is not null) return;
        Fail(nameof(NotBeNull), memberName, filePath, lineNumber);
    }

    public void NotBeNullOrEmpty([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string item && !string.IsNullOrEmpty(item)) return;
        if (Item is IEnumerable collection)
        {
            var enumerator = collection.GetEnumerator();
            if (enumerator.MoveNext()) return;
        }
        Fail(nameof(NotBeNullOrEmpty), memberName, filePath, lineNumber);
    }

    public void NotBeNullOrWhiteSpace([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string item && !string.IsNullOrWhiteSpace(item)) return;
        Fail(nameof(NotBeNullOrWhiteSpace), memberName, filePath, lineNumber);
    }

    public void NotBeSameAs(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (!ReferenceEquals(Item, check)) return;
        Fail(nameof(NotBeSameAs), memberName, filePath, lineNumber);
    }

    public void NotContain<TCheck>(TCheck check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        if (Item is string s && check is string checkStr)
        {
            success = s.Contains(checkStr);
        }
        else if (Item is IEnumerable enumerable)
        {
            foreach (object? element in enumerable)
            {
                if (element?.Equals(check) ?? check is null)
                {
                    success = true;
                    break;
                }
            }
        }
        if (!success) return;
        Fail(nameof(NotContain), memberName, filePath, lineNumber);
    }

    public void StartWith(string check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string s && s.StartsWith(check)) return;
        Fail(nameof(StartWith), memberName, filePath, lineNumber);
    }

    private void Fail(string assertionName, string? memberName, string? filePath, int lineNumber)
    {
        string itemValue = Item switch
        {
            null => "null",
            string s => $"\"{s}\"",
            IEnumerable => "[Collection]",
            _ => Item.ToString() ?? "null"
        };
        if (itemValue.Length > 20)
        {
            itemValue = $"{itemValue[0..10]}...{itemValue.Substring(itemValue.Length - 10)}";
        }
        throw new TestFailException($"({ItemDisplay}:{itemValue}).Should().{assertionName} failed with {memberName} in {filePath} at line {lineNumber}.");
    }
}
