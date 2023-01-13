namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class DelegateExtension
{
    public static Type[] GetParameterTypes(this Delegate del)
    {
        return del.Method.GetParameters().Select(x => x.ParameterType).ToArray();
    }
}
