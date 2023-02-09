using Microsoft.EntityFrameworkCore;

using WorkTool.Console.Interfaces;

namespace WorkTool.Console.Services;

public class SqliteTimerRepository : ITimerRepository
{
    private readonly DbContext context;

    public SqliteTimerRepository(DbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(TimerItem timer)
    {
        await context.AddAsync(timer);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var item = await context.FindAsync<TimerItem>(id);
        context.Remove(item);
    }

    public async Task UpdateAsync(TimerItem timer)
    {
        var item = (await context.Set<TimerItem>().FindAsync(timer.Id)).ThrowIfNull();
        item.Name = timer.Name;
        item.Time = timer.Time;
    }

    public async Task<IEnumerable<TimerItem>> GetItemsAsync()
    {
        return await context.Set<TimerItem>().AsNoTracking().ToArrayAsync();
    }
}
