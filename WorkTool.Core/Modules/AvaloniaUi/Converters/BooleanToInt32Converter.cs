namespace WorkTool.Core.Modules.AvaloniaUi.Converters;

public class BooleanToInt32Converter : IValueConverter
{
    private readonly int falseValue;
    private readonly int trueValue;

    public BooleanToInt32Converter(int trueValue, int falseValue)
    {
        this.trueValue = trueValue;
        this.falseValue = falseValue;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(int) && value is bool booleanValue)
        {
            return booleanValue ? trueValue : falseValue;
        }

        return value;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (targetType != typeof(bool) || value is not int int32Value)
        {
            return value;
        }

        if (int32Value == falseValue)
        {
            return false;
        }

        if (int32Value == trueValue)
        {
            return true;
        }

        return value;
    }
}
