namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IIdentifier<out TKey>
{
    public TKey Key { get; }
}