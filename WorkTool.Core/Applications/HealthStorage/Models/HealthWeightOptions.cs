namespace WorkTool.Core.Applications.HealthStorage.Models;

public class HealthWeightOptions<TId> : Entity<TId>
{
    [PartProperty("CreateHealthWeightModel")]
    public float Weight { get; set; }
}
