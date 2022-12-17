namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class AvaloniaListExtension
{
    public static void Update<T>(this AvaloniaList<T> avaloniaList, IEnumerable<T> items)
    {
        avaloniaList.Clear();
        avaloniaList.AddRange(items);
    }

    public static void Update<T>(this AvaloniaList<T> avaloniaList, params T[] items)
    {
        avaloniaList.Clear();
        avaloniaList.AddRange(items);
    }

    public static void Update<T>(this AvaloniaList<T> avaloniaList, T item)
    {
        avaloniaList.Clear();
        avaloniaList.Add(item);
    }
}
