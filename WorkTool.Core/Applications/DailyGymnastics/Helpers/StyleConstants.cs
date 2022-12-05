namespace WorkTool.Core.Applications.DailyGymnastics.Helpers;

public static class StyleConstants
{
    [Style]
    public static readonly IStyle DailyGymnasticsViewStyle = new Style()
        .SetSelector(default(Selector).Is<DailyGymnasticsView>())
        .AddSetter(
            new Setter()
                .SetProperty(TemplatedControl.TemplateProperty)
                .SetValue(
                    new FuncControlTemplate<DailyGymnasticsView>(
                        (_, _) =>
                            new TabControl()
                                .AddStyle(
                                    new Style()
                                        .SetSelector(
                                            default(Selector)
                                                .Is<TabControl>()
                                                .Child()
                                                .OfType<Border>()
                                                .Child()
                                                .OfType<DockPanel>()
                                                .Child()
                                                .OfType<ItemsPresenter>()
                                        )
                                        .AddSetter(Visual.IsVisibleProperty, false)
                                )
                                .BindBindingValue(
                                    SelectingItemsControl.SelectedIndexProperty,
                                    (DailyGymnasticsViewModel vm) => vm.CurrentTab,
                                    BindingMode.TwoWay
                                )
                                .Item.AddItem(
                                    new TabItem().SetContent(
                                        new Button()
                                            .SetVerticalAlignmentStretch()
                                            .SetHorizontalAlignmentStretch()
                                            .SetHorizontalContentAlignmentCenter()
                                            .SetVerticalContentAlignmentCenter()
                                            .SetContent(
                                                new TextBlock().SetText("Start").SetFontSize(60)
                                            )
                                            .BindBindingValue(
                                                Button.CommandProperty,
                                                (DailyGymnasticsViewModel vm) => vm.StartCommand
                                            )
                                            .Item
                                    )
                                )
                                .AddItem(
                                    new TabItem().SetContent(
                                        new Grid()
                                            .AddRowDefinitions(
                                                GridLength.Auto,
                                                GridLength.Star,
                                                GridLength.Auto
                                            )
                                            .AddChild(
                                                new TextBlock()
                                                    .SetFontSize(60)
                                                    .SetHorizontalAlignmentCenter()
                                                    .SetVerticalAlignmentCenter()
                                                    .BindBindingValue(
                                                        TextBlock.TextProperty,
                                                        (DailyGymnasticsViewModel vm) => vm.Time,
                                                        "{0:hh\\:mm\\:ss}"
                                                    )
                                                    .Item
                                            )
                                            .AddChild(
                                                new Grid()
                                                    .SetGridRow(1)
                                                    .AddColumnDefinitions(
                                                        GridLength.Star,
                                                        GridLength.Star
                                                    )
                                                    .AddChild(
                                                        new StackPanel()
                                                            .SetOrientation(Orientation.Horizontal)
                                                            .AddChild(
                                                                new TextBlock()
                                                                    .SetFontSize(60)
                                                                    .SetHorizontalAlignmentCenter()
                                                                    .SetVerticalAlignmentCenter()
                                                                    .BindBindingValue(
                                                                        TextBlock.TextProperty,
                                                                        (
                                                                            DailyGymnasticsViewModel vm
                                                                        ) => vm.Sample
                                                                    )
                                                                    .Item
                                                            )
                                                            .AddChild(
                                                                new TextBox()
                                                                    .SetFontSize(60)
                                                                    .SetHorizontalAlignmentCenter()
                                                                    .SetVerticalAlignmentCenter()
                                                                    .SetWidth(400)
                                                                    .BindBindingValue(
                                                                        TextBlock.TextProperty,
                                                                        (
                                                                            DailyGymnasticsViewModel vm
                                                                        ) => vm.Result,
                                                                        binding =>
                                                                            binding
                                                                                .SetMode(
                                                                                    BindingMode.TwoWay
                                                                                )
                                                                                .SetConverter(
                                                                                    new MinusToNumberConverter()
                                                                                )
                                                                    )
                                                                    .Item
                                                            )
                                                    )
                                                    .AddChild(
                                                        new TextBlock()
                                                            .SetGridColumn(1)
                                                            .SetFontSize(60)
                                                            .SetHorizontalAlignmentCenter()
                                                            .SetVerticalAlignmentCenter()
                                                            .BindBindingValue(
                                                                TextBlock.TextProperty,
                                                                (DailyGymnasticsViewModel vm) =>
                                                                    vm.Count,
                                                                BindingMode.TwoWay
                                                            )
                                                            .Item
                                                    )
                                            )
                                            .AddChild(
                                                new StackPanel()
                                                    .SetGridRow(2)
                                                    .SetOrientation(Orientation.Horizontal)
                                                    .AddChild(
                                                        new Button()
                                                            .SetHorizontalContentAlignmentCenter()
                                                            .SetVerticalContentAlignmentCenter()
                                                            .SetFontSize(60)
                                                            .SetMargin(5)
                                                            .SetContent("Check")
                                                            .BindBindingValue(
                                                                Button.CommandProperty,
                                                                (DailyGymnasticsViewModel vm) =>
                                                                    vm.CheckCommand
                                                            )
                                                            .Item
                                                    )
                                                    .AddChild(
                                                        new Button()
                                                            .SetHorizontalContentAlignmentCenter()
                                                            .SetVerticalContentAlignmentCenter()
                                                            .SetFontSize(60)
                                                            .SetMargin(5)
                                                            .SetContent("Stop")
                                                            .BindBindingValue(
                                                                Button.CommandProperty,
                                                                (DailyGymnasticsViewModel vm) =>
                                                                    vm.StopCommand
                                                            )
                                                            .Item
                                                    )
                                            )
                                    )
                                )
                                .AddItem(
                                    new TabItem().SetContent(
                                        new Grid()
                                            .AddRowDefinitions(GridLength.Star, GridLength.Auto)
                                            .AddChild(
                                                new StackPanel()
                                                    .SetHorizontalAlignmentCenter()
                                                    .SetVerticalAlignmentCenter()
                                                    .SetOrientationVertical()
                                                    .AddChild(
                                                        new TextBlock()
                                                            .SetHorizontalAlignmentCenter()
                                                            .SetVerticalAlignmentCenter()
                                                            .SetFontSize(60)
                                                            .BindBindingValue(
                                                                TextBlock.TextProperty,
                                                                (DailyGymnasticsViewModel vm) =>
                                                                    vm.Statistic.Count,
                                                                "Count: {0}"
                                                            )
                                                            .Item
                                                    )
                                                    .AddChild(
                                                        new TextBlock()
                                                            .SetHorizontalAlignmentCenter()
                                                            .SetVerticalAlignmentCenter()
                                                            .SetFontSize(60)
                                                            .BindBindingValue(
                                                                TextBlock.TextProperty,
                                                                (DailyGymnasticsViewModel vm) =>
                                                                    vm.Statistic.CalculationsPerSecond,
                                                                "CalculationsPerSecond: {0}"
                                                            )
                                                            .Item
                                                    )
                                            )
                                            .AddChild(
                                                new Button()
                                                    .SetGridRow(1)
                                                    .SetMargin(5)
                                                    .SetHorizontalAlignmentCenter()
                                                    .SetVerticalAlignmentCenter()
                                                    .SetHorizontalContentAlignmentCenter()
                                                    .SetVerticalContentAlignmentCenter()
                                                    .SetContent(
                                                        new TextBlock()
                                                            .SetFontSize(60)
                                                            .SetText("Start")
                                                    )
                                                    .BindBindingValue(
                                                        Button.CommandProperty,
                                                        (DailyGymnasticsViewModel vm) =>
                                                            vm.StartCommand
                                                    )
                                                    .Item
                                            )
                                    )
                                )
                    )
                )
        );
}
