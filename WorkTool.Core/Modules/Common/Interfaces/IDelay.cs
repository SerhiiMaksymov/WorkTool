namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IDelay
{
    Task DelayAsync(TimeSpan delay);
}
