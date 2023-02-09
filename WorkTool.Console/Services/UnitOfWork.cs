using Microsoft.EntityFrameworkCore;

using WorkTool.Console.Interfaces;

namespace WorkTool.Console.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext dbContext;
    private readonly Lazy<ITimerRepository> timerRepository;

    public UnitOfWork(DbContext dbContext)
    {
        this.dbContext = dbContext;
        
        timerRepository = new Lazy<ITimerRepository>(
            () => new SqliteTimerRepository(this.dbContext),
            true
        );
    }

    public ITimerRepository TimerRepository => timerRepository.Value;

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}
