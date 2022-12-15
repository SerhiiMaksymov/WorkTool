namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterReserveTransient
{
    void RegisterReserveTransient(ReserveIdentifier identifier, Delegate @delegate);
}

public interface IRegisterReserveSingleton
{
    void RegisterReserveSingleton(ReserveIdentifier identifier, Delegate @delegate);
}
