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

    public static void UpdateIfNeed<T>(this AvaloniaList<T> avaloniaList, T item) where T : notnull
    {
        if (avaloniaList.Count == 1 && avaloniaList[0].Equals(item))
        {
            return;
        }

        avaloniaList.Update(item);
    }

    public static void AddIfNotContains<T>(this AvaloniaList<T> avaloniaList, T item)
        where T : notnull
    {
        if (avaloniaList.Contains(item))
        {
            return;
        }

        avaloniaList.Add(item);
    }
}
