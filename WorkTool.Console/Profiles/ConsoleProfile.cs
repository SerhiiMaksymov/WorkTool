using AutoMapper;

namespace WorkTool.Console.Profiles;

public class ConsoleProfile : Profile
{
    public ConsoleProfile()
    {
        CreateMap<CreateTimerViewModel, TimerItemViewModel>()
            .ForMember(
                timerItemViewModel => timerItemViewModel.Time,
                opt =>
                    opt.MapFrom(
                        createTimerViewModel =>
                            createTimerViewModel.Time.Add(
                                TimeSpan.FromSeconds(createTimerViewModel.Seconds)
                            )
                    )
            );

        CreateMap<TimerItemViewModel, TimerItem>();
        CreateMap<TimerItem, TimerItemViewModel>();
    }
}
