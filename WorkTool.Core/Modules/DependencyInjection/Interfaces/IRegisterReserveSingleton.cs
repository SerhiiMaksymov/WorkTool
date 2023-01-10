namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterReserveSingleton
{
    void RegisterReserveSingleton(ReserveIdentifier identifier, Delegate del);
}
