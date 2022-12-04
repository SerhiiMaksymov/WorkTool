namespace WorkTool.Core.Modules.AvaloniaUi.Converters;

public class InvertBooleanConverter : IValueConverter
{
    public static readonly InvertBooleanConverter Default = new ();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            return value;
        }

        if (value is not bool result)
        {
            return value;
        }

        return !result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            return value;
        }

        if (value is not bool result)
        {
            return value;
        }

        return !result;
    }
}