namespace WorkTool.Core.Modules.Common.Models;

public class Ref<T> where T : struct
{
    public T Value { get; set; }

    public Ref(T value)
    {
        Value = value;
    }

    public static implicit operator T(Ref<T> @ref)
    {
        return @ref.Value;
    }

    public static implicit operator Ref<T>(T @ref)
    {
        return new Ref<T>(@ref);
    }
}