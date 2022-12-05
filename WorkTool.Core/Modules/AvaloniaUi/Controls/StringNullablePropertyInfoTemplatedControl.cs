namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class StringNullablePropertyInfoTemplatedControl
    : NullablePropertyInfoTemplatedControl<string, TextBox>
{
    static StringNullablePropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<StringNullablePropertyInfoTemplatedControl>(
            x => x.Object
        );
    }

    public StringNullablePropertyInfoTemplatedControl(
        Action<
            PropertyInfoTemplatedControl<string, TextBox>,
            TemplateAppliedEventArgs,
            TextBox,
            TextBlock
        >? onApplyTemplate
    ) : base(onApplyTemplate) { }
}
