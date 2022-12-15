namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class AvaloniaListExtension
{
    public static void Update<T>(this AvaloniaList<T> avaloniaList, params T[] items)
    {
        avaloniaList.Clear();
        avaloniaList.AddRange(items);
    }
}
