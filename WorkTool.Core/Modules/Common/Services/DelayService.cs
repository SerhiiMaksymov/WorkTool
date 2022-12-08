namespace WorkTool.Core.Modules.Common.Services;

public readonly struct DelayService : IDelay
{
    public Task DelayAsync(TimeSpan delay)
    {
        return delay.Delay();
    }
}
