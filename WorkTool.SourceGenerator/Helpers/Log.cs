namespace WorkTool.SourceGenerator.Helpers;

public static class Log
{
    public static readonly ISend<string> Send = new UdpSendString(
        new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 6000),
        Encoding.UTF8
    );
}
