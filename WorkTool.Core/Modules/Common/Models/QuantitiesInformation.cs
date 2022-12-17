namespace WorkTool.Core.Modules.Common.Models;

public readonly struct QuantitiesInformation : IComparable
{
    public const ulong EiBSize = 1152921504606847000ul;
    public const ulong PiBSize = 1125899906842624ul;
    public const ulong TiBSize = 1099511627776ul;
    public const ulong GiBSize = 1073741824ul;
    public const ulong MiBSize = 1048576ul;
    public const ulong KiBSize = 1024ul;

    private readonly ulong size;

    public QuantitiesInformation(ulong size)
    {
        this.size = size;
    }

    public override string ToString()
    {
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

    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;
        if (!(obj is QuantitiesInformation num))
            throw new ArgumentException();
        if (this < num)
            return -1;
        return this > num ? 1 : 0;
    }

    public static bool operator >(QuantitiesInformation x, QuantitiesInformation y)
    {
        return x.size > y.size;
    }

    public static bool operator <(QuantitiesInformation x, QuantitiesInformation y)
    {
        return x.size < y.size;
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
}
