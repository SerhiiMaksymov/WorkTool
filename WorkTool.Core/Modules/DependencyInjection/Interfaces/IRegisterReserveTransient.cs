namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterReserveTransient
{
    void RegisterReserveTransient(ReserveIdentifier identifier, Delegate del);
}
