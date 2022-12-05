namespace WorkTool.Core.Modules.Common.Services;

public class LazyLoad<TObject> where TObject : class
{
    private readonly Func<TObject> func;
    private TObject value;

    public LazyLoad(Func<TObject> func)
    {
        this.func = func;
    }

    public TObject GetValue()
    {
        if (value is null)
        {
            value = func.Invoke();

            return value;
        }

        return value;
    }
}
