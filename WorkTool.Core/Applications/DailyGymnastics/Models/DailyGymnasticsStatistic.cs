namespace WorkTool.Core.Applications.DailyGymnastics.Models;

public class DailyGymnasticsStatistic
{
    public ulong  Count                 { get; }
    public double CalculationsPerSecond { get; }

    public DailyGymnasticsStatistic(ulong count, double calculationsPerSecond)
    {
        Count                 = count;
        CalculationsPerSecond = calculationsPerSecond;
    }
}