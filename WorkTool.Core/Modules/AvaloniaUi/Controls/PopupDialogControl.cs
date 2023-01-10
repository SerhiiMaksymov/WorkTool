namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class PopupDialogControl : ContentControl, IDialogView
{
    public static readonly StyledProperty<bool> IsVisibleDialogProperty = AvaloniaProperty.Register<
        PopupDialogControl,
        bool
    >(nameof(IsVisibleDialog));

    private TaskCompletionSource<bool> taskCompletionSource;

    public PopupDialogControl()
    {
        taskCompletionSource = new TaskCompletionSource<bool>();
    }

    public bool IsVisibleDialog
    {
        get => GetValue(IsVisibleDialogProperty);
        set
        {
            SetValue(IsVisibleDialogProperty, value);

            if (value)
            {
                taskCompletionSource = new TaskCompletionSource<bool>();
            }
            else
            {
                taskCompletionSource.TrySetResult(value);
            }
        }
    }

    public Task CloseAsync()
    {
        IsVisibleDialog = false;

        return Task.CompletedTask;
    }

    public Task ShowAsync()
    {
        if (IsVisibleDialog)
        {
            return taskCompletionSource.Task;
        }

        IsVisibleDialog = true;

        return taskCompletionSource.Task;
    }
}
