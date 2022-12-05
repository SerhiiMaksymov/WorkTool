namespace WorkTool.Core.Modules.Common.Extensions;

public static class DataTableExtension
{
    public static string ToCsv(this DataTable dataTable)
    {
        return dataTable.ToCsv(";", Environment.NewLine);
    }

    public static string GetTableString(this DataTable dataTable)
    {
        return dataTable.GetTableString(Environment.NewLine, -2);
    }

    public static Task<string> GetTableStringAsync(this Task<DataTable> task)
    {
        return task.GetTableStringAsync(Environment.NewLine, -2);
    }

    public static string ToCsv(this DataTable dataTable, string separator, string rowSeparator)
    {
        return $"{dataTable.Columns.ToCsv(separator)}{rowSeparator}{dataTable.Rows.ToCsv(dataTable.Columns, separator, rowSeparator)}";
    }

    public static string GetTableString(this DataTable dataTable, string rowSeparator, int padding)
    {
        var columnNames = dataTable.Columns.Cast<DataColumn>().ToArray();
        var valueRows = dataTable.Rows.OfType<DataRow>().ToArray();
        var values = new string[valueRows.Length][];
        var maxLengths = new int[columnNames.Length];

        for (var index = 0; index < columnNames.Length; index++)
        {
            maxLengths[index] = columnNames[index].ColumnName.Length;
        }

        for (var rowIndex = 0; rowIndex < valueRows.Length; rowIndex++)
        {
            var valuesRow = columnNames
                .Select(x => valueRows[rowIndex][x]?.ToString() ?? string.Empty)
                .ToArray();
            values[rowIndex] = new string[columnNames.Length];

            for (var columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                var value = valuesRow[columnIndex];
                values[rowIndex][columnIndex] = value;
                maxLengths[columnIndex] = Math.Max(value.Length, maxLengths[columnIndex]);
            }
        }

        var formatColumnNames = new string[columnNames.Length];

        for (var index = 0; index < formatColumnNames.Length; index++)
        {
            formatColumnNames[index] = string.Format(
                "{0," + (-maxLengths[index] + padding) + "}",
                columnNames[index]
            );
        }

        var formatRows = new string[valueRows.Length];

        for (var rowIndex = 0; rowIndex < valueRows.Length; rowIndex++)
        {
            var formatRow = new List<string>();

            for (var columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                formatRow.Add("{" + columnIndex + "," + (-maxLengths[columnIndex] + padding) + "}");
            }

            formatRows[rowIndex] = string.Format(formatRow.JoinString(""), values[rowIndex]);
        }

        var result =
            $"{formatColumnNames.JoinString("")}{rowSeparator}{formatRows.JoinString(Environment.NewLine)}";

        return result;
    }

    public static async Task<string> GetTableStringAsync(
        this Task<DataTable> task,
        string rowSeparator,
        int padding
    )
    {
        var dataTable = await task;

        return dataTable.GetTableString(rowSeparator, padding);
    }
}
