namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class DbDataReaderExtension
{
    public static string GetCsvColumnsStringName<TDbDataReader>(
        this TDbDataReader reader,
        string separator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));

        return reader.GetColumnsNames().JoinString(separator);
    }

    public static IEnumerable<string> GetColumnsNames<TDbDataReader>(this TDbDataReader reader)
        where TDbDataReader : DbDataReader
    {
        for (var index = 0; index < reader.FieldCount; index++)
        {
            yield return reader.GetName(index);
        }
    }

    public static IEnumerable<string> GetRowStringValues<TDbDataReader>(this TDbDataReader reader)
        where TDbDataReader : DbDataReader
    {
        var result = new List<string>();

        for (var index = 0; index < reader.FieldCount; index++)
        {
            result.Add(reader.GetValue(index).ToString() ?? string.Empty);
        }

        return result;
    }

    public static async Task<
        IEnumerable<IEnumerable<string>>
    > GetRowsStringValueAsync<TDbDataReader>(this TDbDataReader reader)
        where TDbDataReader : DbDataReader
    {
        var result = new List<IEnumerable<string>>();

        while (await reader.ReadAsync())
        {
            result.Add(reader.GetRowStringValues());
        }

        return result;
    }

    public static string GetCvsRowStringValues<TDbDataReader>(
        this TDbDataReader reader,
        string separator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));

        return reader.GetRowStringValues().JoinString(separator);
    }

    public static string GetCvsRowsStringValue<TDbDataReader>(
        this TDbDataReader reader,
        string separator,
        string rowSeparator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));
        rowSeparator.ThrowIfNull(nameof(rowSeparator));
        var rows = new List<string>();

        while (reader.Read())
        {
            rows.Add(reader.GetCvsRowStringValues(separator));
        }

        return rows.JoinString(rowSeparator);
    }

    public static async Task<string> GetCvsRowsStringValueAsync<TDbDataReader>(
        this TDbDataReader reader,
        string separator,
        string rowSeparator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));
        rowSeparator.ThrowIfNull(nameof(rowSeparator));
        var rows = new List<string>();

        while (await reader.ReadAsync())
        {
            rows.Add(reader.GetCvsRowStringValues(separator));
        }

        return rows.JoinString(rowSeparator);
    }

    public static string GetCvsString<TDbDataReader>(
        this TDbDataReader reader,
        string separator,
        string rowSeparator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));
        rowSeparator.ThrowIfNull(nameof(rowSeparator));
        var columnsStringName = reader.GetCsvColumnsStringName(separator);
        var rowsStringValue = reader.GetCvsRowsStringValue(separator, rowSeparator);

        return $"{columnsStringName}{rowSeparator}{rowsStringValue}";
    }

    public static async Task<string> GetCsvStringAsync<TDbDataReader>(
        this TDbDataReader reader,
        string separator,
        string rowSeparator
    ) where TDbDataReader : DbDataReader
    {
        separator.ThrowIfNull(nameof(separator));
        rowSeparator.ThrowIfNull(nameof(rowSeparator));
        var columnsStringName = reader.GetCsvColumnsStringName(separator);
        var rowsStringValue = await reader.GetCvsRowsStringValueAsync(separator, rowSeparator);

        return $"{columnsStringName}{rowSeparator}{rowsStringValue}";
    }

    public static async Task<string> GetTableStringAsync<TDbDataReader>(
        this TDbDataReader reader,
        string rowSeparator,
        int padding
    ) where TDbDataReader : DbDataReader
    {
        var columnNames = reader.GetColumnsNames().ToArray();
        var valueRows = (await reader.GetRowsStringValueAsync()).ToArray();
        var values = new string[valueRows.Length][];
        var maxLengths = new int[columnNames.Length];

        for (var index = 0; index < columnNames.Length; index++)
        {
            maxLengths[index] = columnNames[index].Length;
        }

        for (var rowIndex = 0; rowIndex < valueRows.Length; rowIndex++)
        {
            var valuesRow = valueRows[rowIndex].ToArray();
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
            var template = $"{{0,{-maxLengths[index] + padding}}}";
            formatColumnNames[index] = string.Format(template, columnNames[index]);
        }

        var formatRows = new string[valueRows.Length];

        for (var rowIndex = 0; rowIndex < valueRows.Length; rowIndex++)
        {
            var formatRow = new List<string>();

            for (var columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                formatRow.Add("{" + columnIndex + "," + (-maxLengths[columnIndex] + padding) + "}");
            }

            formatRows[rowIndex] = string.Format(
                formatRow.JoinString(""),
                values[rowIndex].Cast<object>()
            );
        }

        return $"{formatColumnNames.JoinString("")}{rowSeparator}{formatRows.JoinString(Environment.NewLine)}";
    }
}
