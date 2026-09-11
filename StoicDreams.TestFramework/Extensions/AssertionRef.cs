using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace StoicDreams;

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
        Fail(nameof(Be), check, memberName, filePath, lineNumber);
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
        Fail($"{nameof(BeApproximately)}({expected}, ±{precision})",
            expected,
            Item,
            memberName,
            filePath,
            lineNumber);
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
        Fail(nameof(BeEquivalentTo), check, memberName, filePath, lineNumber);
    }

    public void BeFalse([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is bool item && !item) return;
        Fail(nameof(BeFalse), false, memberName, filePath, lineNumber);
    }

    public void BeGreaterThan(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) > 0) return;
        Fail(nameof(BeGreaterThan), check, memberName, filePath, lineNumber);
    }

    public void BeGreaterThanOrEqualTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) >= 0) return;
        Fail(nameof(BeGreaterThanOrEqualTo), check, memberName, filePath, lineNumber);
    }

    public void BeInRange<TComparable>(TComparable min, TComparable max, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TComparable : IComparable<TComparable>
    {
        if (Item is IComparable<TComparable> item && item.CompareTo(min) >= 0 && item.CompareTo(max) <= 0) return;
        Fail($"{nameof(BeInRange)}({FormatValue(min)} to {FormatValue(max)})", $"between {FormatValue(min)} and {FormatValue(max)}", Item, memberName, filePath, lineNumber);
    }

    public void BeLessThan(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) < 0) return;
        Fail(nameof(BeLessThan), check, memberName, filePath, lineNumber);
    }

    public void BeLessThanOrEqualTo(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Comparer<TItem>.Default.Compare(Item, check) <= 0) return;
        Fail(nameof(BeLessThanOrEqualTo), check, memberName, filePath, lineNumber);
    }

    public void BeNull([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is null) return;
        Fail(nameof(BeNull), null, memberName, filePath, lineNumber);
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
        Fail(nameof(BeOfType), typeof(TExpected).Name, memberName, filePath, lineNumber);
    }

    public void BeSameAs(TItem check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (ReferenceEquals(Item, check)) return;
        Fail(nameof(BeSameAs), check, memberName, filePath, lineNumber);
    }

    public void BeTrue([CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is bool item && item) return;
        Fail(nameof(BeTrue), true, memberName, filePath, lineNumber);
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
        Fail(nameof(Contain), check, memberName, filePath, lineNumber);
    }

    public void ContainKey<TKey>(TKey key, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (key is not null && Item is IDictionary dictionary && dictionary.Contains(key)) return;
        Fail(nameof(ContainKey), key, memberName, filePath, lineNumber);
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
        Fail(nameof(EndWith), check, memberName, filePath, lineNumber);
    }

    public void HaveCount(int expectedCount, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        int? actualCount = Item switch
        {
            string text => text.Length,
            ICollection collection => collection.Count,
            IEnumerable enumerable => GetCount(enumerable),
            _ => null
        };

        if (actualCount == expectedCount) return;
        Fail(
            nameof(HaveCount),
            expectedCount,
            actualCount,
            memberName,
            filePath,
            lineNumber);
    }

    private int? GetCount(IEnumerable enumerable)
    {
        int count = 0;
        foreach (object? _ in enumerable) count++;
        return count;
    }

    public void HaveElementAt<TExpectation>(int index, TExpectation expected, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is IEnumerable enumerable)
        {
            int currentIndex = 0;

            foreach (object? item in enumerable)
            {
                if (currentIndex == index)
                {
                    if (item?.Equals(expected) ?? expected is null)
                    {
                        return;
                    }

                    Fail(
                        $"{nameof(HaveElementAt)}({index})",
                        expected,
                        item,
                        memberName,
                        filePath,
                        lineNumber);

                    return;
                }

                currentIndex++;
            }
        }
        Fail($"{nameof(HaveElementAt)}({index})", expected, "<index does not exist>", memberName, filePath, lineNumber);
    }

    public void MatchRegex(string pattern, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string s && Regex.IsMatch(s, pattern)) return;
        Fail(nameof(MatchRegex), pattern, memberName, filePath, lineNumber);
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
        Fail($"{nameof(NotBeApproximately)}({expected}, ±{precision})", expected, memberName, filePath, lineNumber);
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
        Fail(nameof(NotBeEquivalentTo), check, memberName, filePath, lineNumber);
    }


    public void NotBeInRange<TComparable>(TComparable min, TComparable max, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        where TComparable : IComparable<TComparable>
    {
        if (Item is IComparable<TComparable> item && (item.CompareTo(min) < 0 || item.CompareTo(max) > 0)) return;
        Fail($"{nameof(NotBeInRange)}({min} to {max})", $"between {FormatValue(min)} and {FormatValue(max)}", Item, memberName, filePath, lineNumber);
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
        Fail(nameof(NotBeSameAs), check, memberName, filePath, lineNumber);
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
        Fail(nameof(NotContain), check, memberName, filePath, lineNumber);
    }

    public void StartWith(string check, [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (Item is string s && s.StartsWith(check)) return;
        Fail(nameof(StartWith), check, memberName, filePath, lineNumber);
    }

    private void Fail(
        string assertionName,
        string? memberName,
        string? filePath,
        int lineNumber)
    {
        ThrowFailure(
            assertionName,
            expected: null,
            actual: Item,
            memberName,
            filePath,
            lineNumber,
            includeExpected: false);
    }

    private void Fail(
        string assertionName,
        object? expected,
        string? memberName,
        string? filePath,
        int lineNumber)
    {
        ThrowFailure(
            assertionName,
            expected,
            Item,
            memberName,
            filePath,
            lineNumber,
            includeExpected: true);
    }

    private void Fail(
        string assertionName,
        object? expected,
        object? actual,
        string? memberName,
        string? filePath,
        int lineNumber)
    {
        ThrowFailure(
            assertionName,
            expected,
            actual,
            memberName,
            filePath,
            lineNumber,
            includeExpected: true);
    }

    private void ThrowFailure(
        string assertionName,
        object? expected,
        object? actual,
        string? memberName,
        string? filePath,
        int lineNumber,
        bool includeExpected)
    {
        string actualDisplay = FormatValue(actual);

        StringBuilder message = new();

        message.AppendLine(
            $"({ItemDisplay}:{actualDisplay}).Should().{assertionName} failed.");

        if (includeExpected)
        {
            message.AppendLine($"Expected: {FormatValue(expected)}");
        }

        message.AppendLine($"Received: {actualDisplay}");
        message.Append(
            $"Test: {memberName} in {filePath} at line {lineNumber}.");

        throw new TestFailException(message.ToString());
    }

    private static string FormatValue(object? value, int depth = 0)
    {
        if (value is null)
        {
            return "null";
        }

        if (value is string text)
        {
            return $"\"{text}\"";
        }

        if (value is char character)
        {
            return $"'{character}'";
        }

        if (value is IDictionary dictionary)
        {
            if (depth >= 2)
            {
                return $"[{value.GetType().Name}]";
            }

            List<string> items = [];

            foreach (DictionaryEntry entry in dictionary)
            {
                items.Add(
                    $"{FormatValue(entry.Key, depth + 1)}: {FormatValue(entry.Value, depth + 1)}");

                if (items.Count >= 20)
                {
                    items.Add("...");
                    break;
                }
            }

            return $"{{ {string.Join(", ", items)} }}";
        }

        if (value is IEnumerable enumerable)
        {
            if (depth >= 2)
            {
                return $"[{value.GetType().Name}]";
            }

            List<string> items = [];

            foreach (object? item in enumerable)
            {
                items.Add(FormatValue(item, depth + 1));

                if (items.Count >= 20)
                {
                    items.Add("...");
                    break;
                }
            }

            return $"[{string.Join(", ", items)}]";
        }

        return value.ToString() ?? "null";
    }
}
