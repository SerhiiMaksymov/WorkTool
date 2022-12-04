namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class PropertyInfoTemplatedControlContext
{
    private readonly Func<object, PropertyInfo, IObjectValue>                   defaultTypeMatch;
    private readonly Dictionary<Type, Func<object, PropertyInfo, IObjectValue>> typeMatchs;

    public PropertyInfoTemplatedControlContext(
    IReadOnlyDictionary<Type, Func<object, PropertyInfo, IObjectValue>> typeMatchs,
    Func<object, PropertyInfo, IObjectValue>                            defaultTypeMatch)
    {
        this.defaultTypeMatch = defaultTypeMatch.ThrowIfNull();
        this.typeMatchs       = new Dictionary<Type, Func<object, PropertyInfo, IObjectValue>>(typeMatchs);
    }

    public Func<object, PropertyInfo, IObjectValue> GetView(Type key)
    {
        if (typeMatchs.TryGetValue(key, out var value))
        {
            return value;
        }

        return defaultTypeMatch;
    }

    public Func<object, PropertyInfo, IObjectValue> GetView<TType>()
    {
        return GetView(typeof(TType));
    }
}