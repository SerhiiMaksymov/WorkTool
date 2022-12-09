namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsSenderOptions
{
    public readonly static TimeSpan DefaultWait = 1.ToTimeSpanSeconds();
    public const ushort DefaultCount = 9;
    public static readonly SmsSenderOptions Default =
        new() { Count = DefaultCount, Wait = DefaultWait };

    public TimeSpan Wait { get; set; }
    public ushort Count { get; set; }
}
