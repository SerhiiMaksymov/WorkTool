namespace WorkTool.Core.Modules.Common.Extensions;

public static class DataColumnCollectionExtension
{
    public static string ToCsv(this DataColumnCollection collection, string separator)
    {
        return collection.OfType<DataColumn>().Select(x => x.ColumnName).JoinString(separator);
    }
}