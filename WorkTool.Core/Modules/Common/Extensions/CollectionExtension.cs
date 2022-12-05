namespace WorkTool.Core.Modules.Common.Extensions;

public static class CollectionExtension
{
    public static ICollection<TItem> AddItem<TItem>(this ICollection<TItem> collection, TItem item)
    {
        collection.Add(item);

        return collection;
    }
}
