namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyStatusGetter
{
    DependencyStatus GetStatus(TypeInformation type);
}
