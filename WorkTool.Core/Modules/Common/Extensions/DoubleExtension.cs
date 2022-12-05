namespace WorkTool.Core.Modules.Common.Extensions;

public static class DoubleExtension
{
    public static bool IsZero(this double value)
    {
        return value == 0;
    }

    public static bool IsNegative(this double value)
    {
        return value < 0;
    }

    public static double ThrowIfZero(
        this double value,
        [CallerArgumentExpression("value")] string paramName = ""
    )
    {
        if (value.IsZero())
        {
            throw new ZeroException(paramName);
        }

        return value;
    }

    public static double ThrowIfNegative(
        this double value,
        [CallerArgumentExpression("value")] string paramName = ""
    )
    {
        if (value.IsNegative())
        {
            throw new NegativeException<double>(paramName, value);
        }

        return value;
    }

    public static double ThrowIfZeroOrNegative(
        this double value,
        [CallerArgumentExpression("value")] string paramName = ""
    )
    {
        return value.ThrowIfZero(paramName).ThrowIfNegative(paramName);
    }
}
