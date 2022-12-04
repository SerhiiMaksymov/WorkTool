namespace WorkTool.Core.Modules.Common.Services;

public class UdpSendString : UdpSend<string>
{
    public UdpSendString(IPEndPoint ipEndPoint, Encoding encoding) : base(ipEndPoint, s => encoding.GetBytes(s))
    {
        encoding.ThrowIfNull(nameof(encoding));
    }
}