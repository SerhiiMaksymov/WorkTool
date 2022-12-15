namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IResolver
{
    object Resolve(Type type);
}
