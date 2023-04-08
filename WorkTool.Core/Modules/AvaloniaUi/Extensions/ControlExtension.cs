namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ControlExtension
{
    public static T FindControlThrowIfNotFound<T>(this Control control, string name)
        where T : Control
    {
        var child = control.FindControl<T>(name);

        if (child is null)
        {
            throw new NotFoundNamedControlException(name, typeof(T));
        }

        return child;
    }

    public static TControl SetAttachedFlyout<TControl>(this TControl control, FlyoutBase flyoutBase)
        where TControl : Control
    {
        FlyoutBase.SetAttachedFlyout(control, flyoutBase);

        return control;
    }

    public static FlyoutBase? GetAttachedFlyout<TControl>(this TControl control)
        where TControl : Control
    {
        return FlyoutBase.GetAttachedFlyout(control);
    }

    public static TControl SetGridRow<TControl>(this TControl control, int value)
        where TControl : Control
    {
        Grid.SetRow(control, value);

        return control;
    }

    public static TControl SetDock<TControl>(this TControl control, Dock value)
        where TControl : Control
    {
        DockPanel.SetDock(control, value);

        return control;
    }

    public static TControl SetGridColumn<TControl>(this TControl control, int value)
        where TControl : Control
    {
        Grid.SetColumn(control, value);

        return control;
    }

    public static TControl SetHorizontalScrollBarVisibility<TControl>(
        this TControl control,
        ScrollBarVisibility value
    ) where TControl : Control
    {
        control.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, value);

        return control;
    }

    public static TControl SetAllowAutoHide<TControl>(this TControl control, bool value)
        where TControl : Control
    {
        control.SetValue(ScrollViewer.AllowAutoHideProperty, value);

        return control;
    }

    public static TControl SetVerticalScrollBarVisibility<TControl>(
        this TControl control,
        ScrollBarVisibility value
    ) where TControl : Control
    {
        control.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, value);

        return control;
    }

    public static TParent? FindParent<TParent>(this Control control)
    {
        if (control.Parent is null)
        {
            return default;
        }

        if (control.Parent is TParent parent)
        {
            return parent;
        }

        return ((Control)control.Parent).FindParent<TParent>();
    }
}
