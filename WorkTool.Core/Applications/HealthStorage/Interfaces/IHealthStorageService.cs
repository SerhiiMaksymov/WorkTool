namespace WorkTool.Core.Applications.HealthStorage.Interfaces;

public interface IHealthStorageService<TId>
{
    Task                                        AddHealthWeighAsync(HealthWeightOptions<TId> weight);
    Task<IEnumerable<HealthWeightOptions<TId>>> GetHealthWeighsAsync();
}