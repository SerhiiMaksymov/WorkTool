namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class MenuFlyoutExtension
{
    public static TMenuFlyout AddItems<TMenuFlyout>(
        this TMenuFlyout menuFlyout,
        IEnumerable<object> items
    ) where TMenuFlyout : MenuFlyout
    {
        if (menuFlyout.Items is ICollection<object> collection)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return menuFlyout;
        }

        var result = new AvaloniaList<object>(menuFlyout.Items.OfType<object>());
        result.AddRange(items);
        menuFlyout.Items = result;

        return menuFlyout;
    }

    public static TMenuFlyout AddItems<TMenuFlyout>(
        this TMenuFlyout menuFlyout,
        params object[] items
    ) where TMenuFlyout : MenuFlyout
    {
        if (menuFlyout.Items is ICollection<object> collection)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return menuFlyout;
        }

        var result = new AvaloniaList<object>(menuFlyout.Items.OfType<object>());
        result.AddRange(items);
        menuFlyout.Items = result;

        return menuFlyout;
    }

    public static TMenuFlyout AddItem<TMenuFlyout>(this TMenuFlyout menuFlyout, object item)
        where TMenuFlyout : MenuFlyout
    {
        if (menuFlyout.Items is ICollection<object> collection)
        {
            collection.Add(item);

            return menuFlyout;
        }

        var items = new AvaloniaList<object>(menuFlyout.Items.OfType<object>()) { item };

        menuFlyout.Items = items;

        return menuFlyout;
    }

    public static TMenuFlyout RemoveItem<TMenuFlyout>(this TMenuFlyout menuFlyout, object item)
        where TMenuFlyout : MenuFlyout
    {
        if (menuFlyout.Items is ICollection<object> collection)
        {
            collection.Remove(item);

            return menuFlyout;
        }

        var items = new AvaloniaList<object>(menuFlyout.Items.OfType<object>());
        items.Remove(item);
        menuFlyout.Items = items;

        return menuFlyout;
    }
}
