namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ItemsControlExtension
{
    public static TItemsControl AddItems<TItemsControl>(
        this TItemsControl itemsControl,
        IEnumerable<object> items
    ) where TItemsControl : ItemsControl
    {
        if (itemsControl.Items is ICollection<object> collection)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return itemsControl;
        }

        var result = new AvaloniaList<object>(
            itemsControl.Items?.OfType<object>() ?? Enumerable.Empty<object>()
        );
        result.AddRange(items);
        itemsControl.Items = result;

        return itemsControl;
    }

    public static TItemsControl AddItems<TItemsControl>(
        this TItemsControl itemsControl,
        params object[] items
    ) where TItemsControl : ItemsControl
    {
        if (itemsControl.Items is ICollection<object> collection)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return itemsControl;
        }

        var result = new AvaloniaList<object>(
            itemsControl.Items?.OfType<object>() ?? Enumerable.Empty<object>()
        );
        result.AddRange(items);
        itemsControl.Items = result;

        return itemsControl;
    }

    public static TItemsControl AddItem<TItemsControl>(this TItemsControl itemsControl, object item)
        where TItemsControl : ItemsControl
    {
        if (itemsControl.Items is ICollection<object> collection)
        {
            collection.Add(item);

            return itemsControl;
        }

        var items = new AvaloniaList<object>(
            itemsControl.Items?.OfType<object>() ?? Enumerable.Empty<object>()
        )
        {
            item
        };

        itemsControl.Items = items;

        return itemsControl;
    }

    public static TItemsControl RemoveItem<TItemsControl>(
        this TItemsControl itemsControl,
        object item
    ) where TItemsControl : ItemsControl
    {
        if (itemsControl.Items is ICollection<object> collection)
        {
            collection.Remove(item);

            return itemsControl;
        }

        var items = new AvaloniaList<object>(
            itemsControl.Items?.OfType<object>() ?? Enumerable.Empty<object>()
        );
        items.Remove(item);
        itemsControl.Items = items;

        return itemsControl;
    }
}
