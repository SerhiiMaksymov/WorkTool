namespace WorkTool.Core.Modules.DependencyInjector.Interfaces;

public interface IResolver
{
    object Resolve(Type type);
}