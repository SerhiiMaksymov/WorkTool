namespace WorkTool.Core.Modules.ModularSystem.Extensions;

public static class TreeNodeBuilderExtension
{
    public static TreeNodeBuilder<Guid, IModule> Add(
        this TreeNodeBuilder<Guid, IModule> builder,
        IModule module
    )
    {
        return builder.Add(module.Id, module);
    }
}
