namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsSenderOptions
{
    public const ushort DefaultCount = 9;

    public readonly static TimeSpan DefaultWait = 1.ToTimeSpanSeconds();
    public static readonly SmsSenderOptions Default = new();

    public TimeSpan Wait { get; set; } = DefaultWait;
    public ushort Count { get; set; } = DefaultCount;
}
