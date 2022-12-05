namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IRandom<out TValue>
{
    TValue GetRandom();
}

public interface IRandom<out TValue, in TOptions>
{
    TValue GetRandom(TOptions options);
}
