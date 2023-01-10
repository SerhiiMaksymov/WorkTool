namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IIdentifierGenerator<out TKey>
{
    TKey Generate();
}
