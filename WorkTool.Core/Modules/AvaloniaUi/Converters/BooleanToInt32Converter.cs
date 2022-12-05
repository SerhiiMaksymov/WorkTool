namespace WorkTool.Core.Modules.AvaloniaUi.Converters;

public class BooleanToInt32Converter : IValueConverter
{
    private readonly int _falseValue;
    private readonly int _trueValue;

    public BooleanToInt32Converter(int trueValue, int falseValue)
    {
        _trueValue = trueValue;
        _falseValue = falseValue;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(int) && value is bool booleanValue)
        {
            return booleanValue ? _trueValue : _falseValue;
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

        if (int32Value == _falseValue)
        {
            return false;
        }

        if (int32Value == _trueValue)
        {
            return true;
        }

        return value;
    }
}
