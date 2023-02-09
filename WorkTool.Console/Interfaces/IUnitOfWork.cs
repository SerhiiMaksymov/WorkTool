namespace WorkTool.Console.Interfaces;

public interface IUnitOfWork
{
    ITimerRepository TimerRepository { get; }

    Task SaveChangesAsync();
}
