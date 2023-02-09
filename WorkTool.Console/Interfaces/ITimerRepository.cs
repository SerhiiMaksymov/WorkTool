namespace WorkTool.Console.Interfaces;

public interface ITimerRepository
{
    Task AddAsync(TimerItem timer);
    Task DeleteByIdAsync(Guid id);
    Task UpdateAsync(TimerItem timer);
    Task<IEnumerable<TimerItem>> GetItemsAsync();
}
