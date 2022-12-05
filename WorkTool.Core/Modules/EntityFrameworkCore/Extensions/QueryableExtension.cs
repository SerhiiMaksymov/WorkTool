namespace WorkTool.Core.Modules.EntityFrameworkCore.Extensions;

public static class QueryableExtension
{
    public static async Task<DataTable> GetDataTableAsync<T>(this IQueryable<T> queryable)
    {
        var data = await queryable.ToArrayAsync();

        return data.ToDataTable();
    }
}
