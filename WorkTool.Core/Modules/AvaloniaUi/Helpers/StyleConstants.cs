namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class StyleConstants
{
    [Style]
    public static readonly IStyle BooleanPropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<BooleanPropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<BooleanPropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            BooleanPropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new CheckBox()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle UInt64PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<UInt64PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<UInt64PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            UInt64PropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle UInt32PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<UInt32PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<UInt32PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle UInt16PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<UInt16PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<UInt16PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            UInt16PropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            UInt16PropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            UInt16PropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle SBytePropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<SBytePropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<SBytePropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle BytePropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<BytePropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<BytePropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle Int16PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<Int16PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<Int16PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle Int64PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<Int64PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<Int64PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle Int32PropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<Int32PropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<Int32PropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle DoublePropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<DoublePropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<DoublePropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle DecimalPropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<DecimalPropertyInfoTemplatedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<DecimalPropertyInfoTemplatedControl>(
                        (_, name) =>
                            new Grid()
                                .AddColumnDefinitions(GridLength.Star, GridLength.Star)
                                .AddChild(
                                    new TextBlock()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementTextBlockName,
                                            name
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            StringNullablePropertyInfoTemplatedControl.TitleProperty
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new NumericUpDown()
                                        .SetName(
                                            BooleanPropertyInfoTemplatedControl.ElementControlName,
                                            name
                                        )
                                        .SetGridColumn(1)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle PropertyInfoItemsControlStyle = new Style()
        .SetSelector(default(Selector).Is<PropertyInfoReactiveItemsControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<PropertyInfoReactiveItemsControl>(
                        (_, nameScope) =>
                            new Expander()
                                .SetVerticalContentAlignmentStretch()
                                .SetHorizontalContentAlignmentStretch()
                                .SetHeader(
                                    new TextBlock()
                                        .SetName(
                                            PropertyInfoReactiveItemsControl.ElementTextBlock,
                                            nameScope
                                        )
                                        .BindTemplateValue(
                                            TextBlock.TextProperty,
                                            PropertyInfoReactiveItemsControl.TitleProperty
                                        )
                                        .Item
                                )
                                .SetContent(
                                    new ItemsPresenter()
                                        .SetName(
                                            PropertyInfoReactiveItemsControl.ElementItemsPresenter,
                                            nameScope
                                        )
                                        .BindTemplateValue(
                                            ItemsPresenterBase.ItemsProperty,
                                            ItemsControl.ItemsProperty
                                        )
                                        .Item.BindTemplateValue(
                                            ItemsPresenterBase.ItemsPanelProperty,
                                            ItemsControl.ItemsPanelProperty
                                        )
                                        .Item.BindTemplateValue(
                                            ItemsPresenterBase.ItemTemplateProperty,
                                            ItemsControl.ItemTemplateProperty
                                        )
                                        .Item
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle NullablePropertyInfoItemsControlStyle = new Style()
        .SetSelector(default(Selector).Is<NullablePropertyInfoReactiveItemsControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<NullablePropertyInfoReactiveItemsControl>(
                        (templated, name) =>
                            new Expander()
                                .SetVerticalContentAlignmentStretch()
                                .SetHorizontalContentAlignmentStretch()
                                .SetHeader(
                                    templated.CheckBox.SetContent(
                                        new TextBlock()
                                            .SetName(
                                                PropertyInfoReactiveItemsControl.ElementTextBlock
                                            )
                                            .RegisterInNameScope(name)
                                    )
                                )
                                .SetContent(
                                    new ItemsPresenter()
                                        .SetName(
                                            PropertyInfoReactiveItemsControl.ElementItemsPresenter
                                        )
                                        .RegisterInNameScope(name)
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle StringPropertyInfoTemplatedControlStyle = new Style()
        .SetSelector(default(Selector).Is<StringNullablePropertyInfoTemplatedControl>())
        .AddSetter(
            TemplatedControl.TemplateProperty,
            new FuncControlTemplate<StringNullablePropertyInfoTemplatedControl>(
                (_, name) =>
                    new Grid()
                        .AddColumnDefinitions(GridLength.Star, GridLength.Auto, GridLength.Star)
                        .AddChild(
                            new TextBlock()
                                .SetName(
                                    StringNullablePropertyInfoTemplatedControl.ElementTextBlockName,
                                    name
                                )
                                .BindTemplateValue(
                                    TextBlock.TextProperty,
                                    StringNullablePropertyInfoTemplatedControl.TitleProperty
                                )
                                .Item
                        )
                        .AddChild(
                            new CheckBox()
                                .SetGridColumn(1)
                                .BindTemplateValue(
                                    ToggleButton.IsCheckedProperty,
                                    StringNullablePropertyInfoTemplatedControl.IsNullProperty,
                                    BindingMode.TwoWay
                                )
                                .Item
                        )
                        .AddChild(
                            new TextBox()
                                .SetName(
                                    StringNullablePropertyInfoTemplatedControl.ElementControlName,
                                    name
                                )
                                .SetGridColumn(2)
                                .BindTemplateValue(
                                    TextBox.TextProperty,
                                    StringNullablePropertyInfoTemplatedControl.ValueProperty
                                )
                                .Item.BindTemplateValue(
                                    InputElement.IsEnabledProperty,
                                    StringNullablePropertyInfoTemplatedControl.IsNullProperty,
                                    InvertBooleanConverter.Default
                                )
                                .Item
                        )
            )
        );

    [Style]
    public static readonly IStyle CommandsControlStyle = new Style()
        .SetSelector(default(Selector).Is<CommandsControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate(
                        (_, _) =>
                            new Grid()
                                .AddRowDefinition(GridLength.Auto)
                                .AddRowDefinition(GridLength.Star)
                                .AddChild(
                                    new ItemsPresenter()
                                        .BindValue(
                                            ItemsPresenterBase.ItemsProperty,
                                            new TemplateBinding(ItemsControl.ItemsProperty),
                                            null
                                        )
                                        .Item.BindValue(
                                            ItemsPresenterBase.ItemsPanelProperty,
                                            new TemplateBinding(ItemsControl.ItemsPanelProperty),
                                            null
                                        )
                                        .Item.BindValue(
                                            ItemsPresenterBase.ItemTemplateProperty,
                                            new TemplateBinding(ItemsControl.ItemTemplateProperty),
                                            null
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new ContentPresenter()
                                        .SetGridRow(1)
                                        .BindValue(
                                            ContentPresenter.ContentProperty,
                                            new TemplateBinding(CommandsControl.ContentProperty),
                                            null
                                        )
                                        .Item
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle TabbedControlStyle = new Style()
        .SetSelector(default(Selector).Is<TabbedControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<TabbedControl>(
                        (templated, _) =>
                            new Grid()
                                .AddRowDefinition(GridLength.Auto)
                                .AddRowDefinition(GridLength.Star)
                                .AddChild(templated.Menu)
                                .AddChild(templated.Tabs.SetGridRow(1))
                    )
                )
        );

    [Style]
    public static readonly IStyle DialogControlStyle = new Style()
        .SetSelector(default(Selector).Is<DialogControl>())
        .AddSetter(
            new Setter()
                .SetProperty(DialogControl.DialogBackgroundProperty)
                .SetValue(new SolidColorBrush(new Color(124, 0, 0, 0)))
        )
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate(
                        (_, _) =>
                            new Panel()
                                .AddChild(
                                    new ContentPresenter()
                                        .SetZIndex(1)
                                        .BindValue(
                                            ContentPresenter.ContentProperty,
                                            new TemplateBinding(ContentControl.ContentProperty),
                                            null
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new ContentPresenter()
                                        .SetHorizontalContentAlignmentCenter()
                                        .SetVerticalContentAlignmentCenter()
                                        .BindValue(
                                            ContentPresenter.BackgroundProperty,
                                            new TemplateBinding(
                                                DialogControl.DialogBackgroundProperty
                                            ),
                                            null
                                        )
                                        .Item.SetZIndex(2)
                                        .BindValue(
                                            Visual.IsVisibleProperty,
                                            new TemplateBinding(
                                                DialogControl.IsVisibleDialogProperty
                                            ),
                                            null
                                        )
                                        .Item.BindValue(
                                            ContentPresenter.ContentProperty,
                                            new TemplateBinding(DialogControl.DialogProperty),
                                            null
                                        )
                                        .Item
                                )
                    )
                )
        );

    [Style]
    public static readonly IStyle MessageControlStyle = new Style()
        .SetSelector(default(Selector).Is<MessageControl>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate(
                        (_, _) =>
                            new Grid()
                                .AddRowDefinition(GridLength.Auto)
                                .AddRowDefinition(GridLength.Star)
                                .AddRowDefinition(GridLength.Auto)
                                .AddChild(
                                    new ContentPresenter()
                                        .BindValue(
                                            ContentPresenter.ContentProperty,
                                            new TemplateBinding(MessageControl.TitleProperty),
                                            null
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new ContentPresenter()
                                        .SetGridRow(1)
                                        .BindValue(
                                            ContentPresenter.ContentProperty,
                                            new TemplateBinding(MessageControl.ContentProperty),
                                            null
                                        )
                                        .Item
                                )
                                .AddChild(
                                    new ItemsPresenter()
                                        .SetGridRow(2)
                                        .BindValue(
                                            ItemsPresenterBase.ItemsProperty,
                                            new TemplateBinding(ItemsControl.ItemsProperty),
                                            null
                                        )
                                        .Item.BindValue(
                                            ItemsPresenterBase.ItemTemplateProperty,
                                            new TemplateBinding(ItemsControl.ItemTemplateProperty),
                                            null
                                        )
                                        .Item.BindValue(
                                            ItemsPresenterBase.ItemsPanelProperty,
                                            new TemplateBinding(ItemsControl.ItemsPanelProperty),
                                            null
                                        )
                                        .Item
                                )
                    )
                )
        );
}
