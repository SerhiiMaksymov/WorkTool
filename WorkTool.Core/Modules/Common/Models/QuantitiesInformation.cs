namespace WorkTool.Core.Modules.Common.Models;

public readonly struct QuantitiesInformation : IEquatable<ulong>, INumber<QuantitiesInformation>
{
    public const ulong EiBSize = 1152921504606847000ul;
    public const ulong PiBSize = 1125899906842624ul;
    public const ulong TiBSize = 1099511627776ul;
    public const ulong GiBSize = 1073741824ul;
    public const ulong MiBSize = 1048576ul;
    public const ulong KiBSize = 1024ul;

    public static QuantitiesInformation AdditiveIdentity => 0ul;
    public static QuantitiesInformation MultiplicativeIdentity => 1ul;
    public static QuantitiesInformation One => 1ul;
    public static int Radix => 2;
    public static QuantitiesInformation Zero => 0ul;

    private readonly ulong size;

    public QuantitiesInformation(ulong size)
    {
        this.size = size;
    }

    public static QuantitiesInformation FromEiB(ulong value)
    {
        return EiBSize * value;
    }

    public static QuantitiesInformation FromPiB(ulong value)
    {
        return PiBSize * value;
    }

    public static QuantitiesInformation FromTiB(ulong value)
    {
        return TiBSize * value;
    }

    public static QuantitiesInformation FromKiB(ulong value)
    {
        return KiBSize * value;
    }

    public static QuantitiesInformation FromMiB(ulong value)
    {
        return MiBSize * value;
    }

    public static QuantitiesInformation FromGiB(ulong value)
    {
        return GiBSize * value;
    }

    public bool Equals(QuantitiesInformation other)
    {
        return size == other.size;
    }

    public bool Equals(ulong value)
    {
        return size == value;
    }

    public int CompareTo(QuantitiesInformation other)
    {
        return size.CompareTo(other.size);
    }

    public static QuantitiesInformation operator %(
        QuantitiesInformation left,
        QuantitiesInformation right
    )
    {
        return left.size % right.size;
    }

    public static QuantitiesInformation operator +(QuantitiesInformation value)
    {
        return value;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            QuantitiesInformation info => Equals(info),
            ulong ul => Equals(ul),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return size.GetHashCode();
    }

    public override string ToString()
    {
        if (size == ulong.MinValue)
        {
            return "0";
        }

        var stringBuilder = new StringBuilder();
        var restSize = size;
        restSize = SetEiB(restSize, stringBuilder);
        restSize = SetPiB(restSize, stringBuilder);
        restSize = SetTiB(restSize, stringBuilder);
        restSize = SetGiB(restSize, stringBuilder);
        restSize = SetMiB(restSize, stringBuilder);
        restSize = SetKiB(restSize, stringBuilder);
        SetB(restSize, stringBuilder);

        return stringBuilder.ToString();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return size.ToString(format, formatProvider);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider
    )
    {
        return size.TryFormat(destination, out charsWritten, format, provider);
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;
        if (!(obj is QuantitiesInformation num))
            throw new ArgumentException();

        return CompareTo(num);
    }

    public static implicit operator ulong(QuantitiesInformation value)
    {
        return value.size;
    }

    public static bool operator ==(QuantitiesInformation x, ulong y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(QuantitiesInformation x, ulong y)
    {
        return !x.Equals(y);
    }

    public static bool operator >(QuantitiesInformation x, QuantitiesInformation y)
    {
        return x.size > y.size;
    }

    public static bool operator >=(QuantitiesInformation left, QuantitiesInformation right)
    {
        return left.size >= right.size;
    }

    public static bool operator <(QuantitiesInformation x, QuantitiesInformation y)
    {
        return x.size < y.size;
    }

    public static bool operator <=(QuantitiesInformation left, QuantitiesInformation right)
    {
        return left.size <= right.size;
    }

    public static QuantitiesInformation operator +(QuantitiesInformation x, QuantitiesInformation y)
    {
        return new QuantitiesInformation(x.size + y.size);
    }

    public static implicit operator QuantitiesInformation(ulong value)
    {
        return new(value);
    }

    public static implicit operator QuantitiesInformation(long value)
    {
        return new((ulong)value);
    }

    private void SetB(ulong restSize, StringBuilder stringBuilder)
    {
        if (restSize == ulong.MinValue)
        {
            return;
        }

        var str = stringBuilder.Length == 0 ? $"{restSize} B" : $" {restSize} B";
        stringBuilder.Append(str);
    }

    private ulong SetKiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / KiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} KiB" : $" {count} KiB";
        stringBuilder.Append(str);

        return restSize % KiBSize;
    }

    private ulong SetMiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / MiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} MiB" : $" {count} MiB";
        stringBuilder.Append(str);

        return restSize % MiBSize;
    }

    private ulong SetGiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / GiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} GiB" : $" {count} GiB";
        stringBuilder.Append(str);

        return restSize % GiBSize;
    }

    private ulong SetTiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / TiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} TiB" : $" {count} TiB";
        stringBuilder.Append(str);

        return restSize % TiBSize;
    }

    private ulong SetPiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / PiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} PiB" : $" {count} PiB";
        stringBuilder.Append(str);

        return restSize % PiBSize;
    }

    private ulong SetEiB(ulong restSize, StringBuilder stringBuilder)
    {
        var count = restSize / EiBSize;

        if (count == ulong.MinValue)
        {
            return restSize;
        }

        var str = stringBuilder.Length == 0 ? $"{count} EiB" : $" {count} EiB";
        stringBuilder.Append(str);

        return restSize % EiBSize;
    }

    public static QuantitiesInformation Parse(string s, IFormatProvider? provider)
    {
        return ulong.Parse(s, provider);
    }

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out QuantitiesInformation result
    )
    {
        var value = ulong.TryParse(s, provider, out var ul);
        result = ul;

        return value;
    }

    public static QuantitiesInformation Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return ulong.Parse(s, provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out QuantitiesInformation result
    )
    {
        var value = ulong.TryParse(s, provider, out var ul);
        result = ul;

        return value;
    }

    public static bool operator ==(QuantitiesInformation left, QuantitiesInformation right)
    {
        return left.size == right.size;
    }

    public static bool operator !=(QuantitiesInformation left, QuantitiesInformation right)
    {
        return left.size != right.size;
    }

    public static QuantitiesInformation operator --(QuantitiesInformation value)
    {
        return value.size - 1;
    }

    public static QuantitiesInformation operator /(
        QuantitiesInformation left,
        QuantitiesInformation right
    )
    {
        return left.size / right.size;
    }

    public static QuantitiesInformation operator ++(QuantitiesInformation value)
    {
        return value.size + 1;
    }

    public static QuantitiesInformation operator *(
        QuantitiesInformation left,
        QuantitiesInformation right
    )
    {
        return left.size * right.size;
    }

    public static QuantitiesInformation operator -(
        QuantitiesInformation left,
        QuantitiesInformation right
    )
    {
        return left.size - right.size;
    }

    public static QuantitiesInformation operator -(QuantitiesInformation value)
    {
        return 0 - value.size;
    }

    public static QuantitiesInformation Abs(QuantitiesInformation value)
    {
        return value;
    }

    public static bool IsCanonical(QuantitiesInformation value)
    {
        return true;
    }

    public static bool IsComplexNumber(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsEvenInteger(QuantitiesInformation value)
    {
        return ((long)value.size & 1L) == 0L;
    }

    public static bool IsFinite(QuantitiesInformation value)
    {
        return true;
    }

    public static bool IsImaginaryNumber(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsInfinity(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsInteger(QuantitiesInformation value)
    {
        return true;
    }

    public static bool IsNaN(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsNegative(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsNegativeInfinity(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsNormal(QuantitiesInformation value)
    {
        return value > 0ul;
    }

    public static bool IsOddInteger(QuantitiesInformation value)
    {
        return (value.size & 1UL) > 0UL;
        ;
    }

    public static bool IsPositive(QuantitiesInformation value)
    {
        return true;
    }

    public static bool IsPositiveInfinity(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsRealNumber(QuantitiesInformation value)
    {
        return true;
    }

    public static bool IsSubnormal(QuantitiesInformation value)
    {
        return false;
    }

    public static bool IsZero(QuantitiesInformation value)
    {
        return value == 0ul;
    }

    public static QuantitiesInformation MaxMagnitude(
        QuantitiesInformation x,
        QuantitiesInformation y
    )
    {
        return ulong.Max(x.size, y.size);
    }

    public static QuantitiesInformation MaxMagnitudeNumber(
        QuantitiesInformation x,
        QuantitiesInformation y
    )
    {
        return ulong.Max(x.size, y.size);
    }

    public static QuantitiesInformation MinMagnitude(
        QuantitiesInformation x,
        QuantitiesInformation y
    )
    {
        return ulong.Min(x.size, y.size);
    }

    public static QuantitiesInformation MinMagnitudeNumber(
        QuantitiesInformation x,
        QuantitiesInformation y
    )
    {
        return ulong.Min(x.size, y.size);
    }

    public static QuantitiesInformation Parse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider
    )
    {
        return ulong.Parse(s, style, provider);
    }

    public static QuantitiesInformation Parse(
        string s,
        NumberStyles style,
        IFormatProvider? provider
    )
    {
        return ulong.Parse(s, style, provider);
    }

    public static bool TryConvertFromChecked<TOther>(TOther value, out QuantitiesInformation result)
        where TOther : INumberBase<TOther>
    {
        if (typeof(TOther) == typeof(byte))
        {
            var num = (byte)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(char))
        {
            var ch = (char)(object)value;
            result = (ulong)ch;

            return true;
        }
        if (typeof(TOther) == typeof(decimal))
        {
            var num = (decimal)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(ushort))
        {
            var num = (ushort)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(uint))
        {
            var num = (uint)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(UInt128))
        {
            var uint128 = (UInt128)(object)value;
            result = checked((ulong)uint128);

            return true;
        }
        if (typeof(TOther) == typeof(nuint))
        {
            var num = (nuint)(object)value;
            result = num;

            return true;
        }
        result = 0UL;

        return false;
    }

    public static bool TryConvertFromSaturating<TOther>(
        TOther value,
        out QuantitiesInformation result
    ) where TOther : INumberBase<TOther>
    {
        if (typeof(TOther) == typeof(byte))
        {
            var num = (byte)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(char))
        {
            var ch = (char)(object)value;
            result = (ulong)ch;

            return true;
        }
        if (typeof(TOther) == typeof(decimal))
        {
            var num = (decimal)(object)value;
            result = num >= 18446744073709551615M ? ulong.MaxValue : (num <= 0M ? 0UL : (ulong)num);

            return true;
        }
        if (typeof(TOther) == typeof(ushort))
        {
            var num = (ushort)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(uint))
        {
            var num = (uint)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(UInt128))
        {
            var uint128 = (UInt128)(object)value;
            result = uint128 >= (UInt128)ulong.MaxValue ? ulong.MaxValue : (ulong)uint128;

            return true;
        }
        if (typeof(TOther) == typeof(nuint))
        {
            var num = (nuint)(object)value;
            result = num;

            return true;
        }

        result = 0UL;

        return false;
    }

    public static bool TryConvertFromTruncating<TOther>(
        TOther value,
        out QuantitiesInformation result
    ) where TOther : INumberBase<TOther>
    {
        if (typeof(TOther) == typeof(byte))
        {
            var num = (byte)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(char))
        {
            var ch = (char)(object)value;
            result = (ulong)ch;

            return true;
        }
        if (typeof(TOther) == typeof(decimal))
        {
            var num = (decimal)(object)value;
            result = num >= 18446744073709551615M ? ulong.MaxValue : (num <= 0M ? 0UL : (ulong)num);

            return true;
        }
        if (typeof(TOther) == typeof(ushort))
        {
            var num = (ushort)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(uint))
        {
            var num = (uint)(object)value;
            result = (ulong)num;

            return true;
        }
        if (typeof(TOther) == typeof(UInt128))
        {
            var uint128 = (UInt128)(object)value;
            result = (ulong)uint128;

            return true;
        }
        if (typeof(TOther) == typeof(nuint))
        {
            var num = (nuint)(object)value;
            result = num;

            return true;
        }

        result = 0UL;

        return false;
    }

    public static bool TryConvertToChecked<TOther>(QuantitiesInformation value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    public static bool TryConvertToSaturating<TOther>(
        QuantitiesInformation value,
        out TOther result
    ) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    public static bool TryConvertToTruncating<TOther>(
        QuantitiesInformation value,
        out TOther result
    ) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        out QuantitiesInformation result
    )
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(
        string? s,
        NumberStyles style,
        IFormatProvider? provider,
        out QuantitiesInformation result
    )
    {
        throw new NotImplementedException();
    }
}
