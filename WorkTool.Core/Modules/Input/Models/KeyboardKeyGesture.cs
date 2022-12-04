namespace WorkTool.Core.Modules.Input.Models;

public sealed class KeyboardKeyGesture : IEquatable<KeyboardKeyGesture>
{
    private static readonly Dictionary<string, KeyboardKey> SKeySynonyms = new ()
    {
        {
            "+", KeyboardKey.OemPlus
        },
        {
            "-", KeyboardKey.OemMinus
        },
        {
            ".", KeyboardKey.OemPeriod
        },
        {
            ",", KeyboardKey.OemComma
        }
    };

    public KeyboardKey Key { get; }

    public KeyboardKeyModifiers KeyModifiers { get; }

    public KeyboardKeyGesture(KeyboardKey key, KeyboardKeyModifiers modifiers = KeyboardKeyModifiers.None)
    {
        Key          = key;
        KeyModifiers = modifiers;
    }

    public bool Equals(KeyboardKeyGesture? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Key == other.Key && KeyModifiers == other.KeyModifiers;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is KeyboardKeyGesture gesture && Equals(gesture);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)Key * 397) ^ (int)KeyModifiers;
        }
    }

    public static bool operator ==(KeyboardKeyGesture left, KeyboardKeyGesture right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(KeyboardKeyGesture left, KeyboardKeyGesture right)
    {
        return !Equals(left, right);
    }

    public static KeyboardKeyGesture Parse(string gesture)
    {
        var key          = KeyboardKey.None;
        var keyModifiers = KeyboardKeyModifiers.None;
        var cstart       = 0;

        for (var c = 0; c <= gesture.Length; c++)
        {
            var ch     = c == gesture.Length ? '\0' : gesture[c];
            var isLast = c == gesture.Length;

            if (!isLast && (ch != '+' || cstart == c))
            {
                continue;
            }

            var partSpan = gesture.AsSpan(cstart, c - cstart).Trim();

            if (isLast)
            {
                key = ParseKey(partSpan.ToString());
            }
            else
            {
                keyModifiers |= ParseModifier(partSpan);
            }

            cstart = c + 1;
        }

        return new KeyboardKeyGesture(key, keyModifiers);
    }

    public override string ToString()
    {
        var s = new StringBuilder();

        static void Plus(StringBuilder s)
        {
            if (s.Length > 0)
            {
                s.Append("+");
            }
        }

        if (KeyModifiers.WorkToolHasAllFlags(KeyboardKeyModifiers.Control))
        {
            s.Append("Ctrl");
        }

        if (KeyModifiers.WorkToolHasAllFlags(KeyboardKeyModifiers.Shift))
        {
            Plus(s);
            s.Append("Shift");
        }

        if (KeyModifiers.WorkToolHasAllFlags(KeyboardKeyModifiers.Alt))
        {
            Plus(s);
            s.Append("Alt");
        }

        if (KeyModifiers.WorkToolHasAllFlags(KeyboardKeyModifiers.Meta))
        {
            Plus(s);
            s.Append("Cmd");
        }

        Plus(s);
        s.Append(Key);

        return s.ToString();
    }

    private static KeyboardKey ParseKey(string key)
    {
        if (SKeySynonyms.TryGetValue(key.ToLower(), out var rv))
        {
            return rv;
        }

        return Enum.Parse<KeyboardKey>(key, true);
    }

    private static KeyboardKeyModifiers ParseModifier(ReadOnlySpan<char> modifier)
    {
        if (modifier.Equals("ctrl".AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return KeyboardKeyModifiers.Control;
        }

        if (modifier.Equals("cmd".AsSpan(), StringComparison.OrdinalIgnoreCase)
        || modifier.Equals("win".AsSpan(), StringComparison.OrdinalIgnoreCase)
        || modifier.Equals("⌘".AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return KeyboardKeyModifiers.Meta;
        }

        return Enum.Parse<KeyboardKeyModifiers>(modifier.ToString(), true);
    }
}