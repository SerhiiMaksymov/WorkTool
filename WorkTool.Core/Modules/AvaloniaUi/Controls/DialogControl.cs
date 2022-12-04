namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class DialogControl : ContentControl, IDialogView
{
    public static readonly StyledProperty<object> DialogProperty =
        AvaloniaProperty.Register<DialogControl, object>(nameof(Dialog));

    public static readonly StyledProperty<IBrush> DialogBackgroundProperty =
        AvaloniaProperty.Register<DialogControl, IBrush>(nameof(DialogBackground));

    public static readonly StyledProperty<bool> IsVisibleDialogProperty =
        AvaloniaProperty.Register<DialogControl, bool>(nameof(IsVisibleDialog));
    private TaskCompletionSource<bool> taskCompletionSource;

    public object Dialog
    {
        get => GetValue(DialogProperty);
        set => SetValue(DialogProperty, value);
    }

    public IBrush DialogBackground
    {
        get => GetValue(DialogBackgroundProperty);
        set => SetValue(DialogBackgroundProperty, value);
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
                taskCompletionSource?.TrySetResult(value);
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