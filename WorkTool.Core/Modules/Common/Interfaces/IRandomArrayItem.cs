namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IRandomArrayItem<TValue>
{
    TValue GetRandom(TValue[] values);
}