using Microsoft.EntityFrameworkCore;

using WorkTool.Console.Interfaces;

namespace WorkTool.Console.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext dbContext;
    private readonly Lazy<ITimerRepository> timerRepository;

    public UnitOfWork(DbContext dbContext, Lazy<ITimerRepository> timerRepository)
    {
        this.dbContext = dbContext;
        this.timerRepository = timerRepository;
    }

    public ITimerRepository TimerRepository => timerRepository.Value;

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}
