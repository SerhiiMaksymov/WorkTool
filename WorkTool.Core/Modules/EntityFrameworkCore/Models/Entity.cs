namespace WorkTool.Core.Modules.EntityFrameworkCore.Models;

public class Entity<TId> where TId : struct
{
    public TId Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastEdit { get; set; }
}
