namespace WorkTool.Core.Applications.DailyGymnastics.Views;

public class DailyGymnasticsView : ReactiveUserControl<DailyGymnasticsViewModel>
{
    public DailyGymnasticsView(DailyGymnasticsViewModel viewModel)
    {
        ViewModel = viewModel;

        KeyBindings.Add(
            new KeyBinding { Command = viewModel.CheckCommand, Gesture = new KeyGesture(Key.Enter) }
        );
    }
}
