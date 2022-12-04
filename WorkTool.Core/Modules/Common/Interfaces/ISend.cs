namespace WorkTool.Core.Modules.Common.Interfaces;

public interface ISend<in TMessage>
{
    void Send(TMessage      message);
    Task SendAsync(TMessage message, CancellationToken token);
}