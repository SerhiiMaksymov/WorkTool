namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class DialogControl : ContentControl, IDialogView
{
    private static readonly Dictionary<Window, DialogControl> windows;
    private static          DialogControl?                    lastDialogControl;
    
    public static IReadOnlyDictionary<Window, DialogControl> Windows           => windows;
    public static DialogControl?                             LastDialogControl => lastDialogControl;
    
    public const    string PartDialogContentPresenter = "PART_DialogContentPresenter";
    
    public static readonly StyledProperty<bool> IsVisibleDialogProperty = AvaloniaProperty.Register<
        DialogControl,
        bool
    >(nameof(IsVisibleDialog));
    
    public static readonly StyledProperty<object?> DialogProperty = AvaloniaProperty.Register<
        DialogControl,
        object?
    >(nameof(Dialog));

    public static readonly StyledProperty<IBrush?> BackgroundDialogProperty =
        AvaloniaProperty.Register<DialogControl, IBrush?>(nameof(BackgroundDialog));
    
    public static readonly StyledProperty<IBrush?> BorderBrushDialogProperty =
        Border.BorderBrushProperty.AddOwner<DialogControl>();
    
    public static readonly StyledProperty<Thickness> BorderThicknessDialogProperty =
        Border.BorderThicknessProperty.AddOwner<DialogControl>();
    
    public static readonly StyledProperty<CornerRadius> CornerRadiusDialogProperty =
        Border.CornerRadiusProperty.AddOwner<DialogControl>();
    
    public static readonly StyledProperty<IDataTemplate?> ContentTemplateDialogProperty =
        AvaloniaProperty.Register<DialogControl, IDataTemplate?>(nameof(ContentTemplateDialog));

    public static readonly StyledProperty<Thickness> PaddingDialogProperty =
        Decorator.PaddingProperty.AddOwner<DialogControl>();
    
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentDialogProperty =
        AvaloniaProperty.Register<DialogControl, VerticalAlignment>(nameof(VerticalContentAlignmentDialog));
  
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentDialogProperty =
        AvaloniaProperty.Register<DialogControl, HorizontalAlignment>(nameof(HorizontalContentAlignmentDialog));

    private TaskCompletionSource<bool> taskCompletionSource;

    static DialogControl()
    {
        windows = new Dictionary<Window, DialogControl>();
    }

    public DialogControl()
    {
        taskCompletionSource = new TaskCompletionSource<bool>();
    }
    
    public HorizontalAlignment HorizontalContentAlignmentDialog
    {
        get => GetValue(HorizontalContentAlignmentDialogProperty);
        set => SetValue(HorizontalContentAlignmentDialogProperty, value);
    }
    
    public VerticalAlignment VerticalContentAlignmentDialog
    {
        get => GetValue(VerticalContentAlignmentDialogProperty);
        set => SetValue(VerticalContentAlignmentDialogProperty, value);
    }
    
    public Thickness PaddingDialog
    {
        get { return GetValue(PaddingDialogProperty); }
        set { SetValue(PaddingDialogProperty, value); }
    }
    
    public IDataTemplate? ContentTemplateDialog
    {
        get => GetValue(ContentTemplateDialogProperty);
        set => SetValue(ContentTemplateDialogProperty, value);
    }
    
    public CornerRadius CornerRadiusDialog
    {
        get => GetValue(CornerRadiusDialogProperty);
        set => SetValue(CornerRadiusDialogProperty, value);
    }
    
    public Thickness BorderThicknessDialog
    {
        get => GetValue(BorderThicknessDialogProperty);
        set => SetValue(BorderThicknessDialogProperty, value);
    }
    
    public IBrush? BorderBrushDialog
    {
        get => GetValue(BorderBrushDialogProperty);
        set => SetValue(BorderBrushDialogProperty, value);
    }

    public IBrush? BackgroundDialog
    {
        get => GetValue(BackgroundDialogProperty);
        set => SetValue(BackgroundDialogProperty, value);
    }

    public object? Dialog
    {
        get => GetValue(DialogProperty);
        set => SetValue(DialogProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        lastDialogControl = this;
        base.OnApplyTemplate(e);
        var window = this.FindParent<Window>();

        if (window is null)
        {
            return;
        }

        windows[window] = this;
    }
}
