namespace WorkTool.Core.Modules.CommandLine.Models;

public class IpAddressCommandLineArgumentMeta : CommandLineArgumentMeta<IPAddress>
{
    public IpAddressCommandLineArgumentMeta(string key, IPAddress address) : base(key, address)
    {
    }

    public override IPAddress Parse(string value)
    {
        return IPAddress.Parse(value);
    }
}