using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace 导入英汉词典到数据库并显示进度;

public class ImportExecutor
{
    private readonly IOptions<ConnStrOptions> _optionsConnStr;
    private readonly IHubContext<ImportDictHub> _hubContext;
    private readonly ILogger<ImportExecutor> _logger;
    public ImportExecutor(IOptions<ConnStrOptions> optionsConnStr,
        IHubContext<ImportDictHub> hubContext, ILogger<ImportExecutor> logger)
    {
        _optionsConnStr = optionsConnStr;
        _hubContext = hubContext;
        _logger = logger;
    }
    public async Task ExecuteAsync(string connectionId)
    {
        try
        {
            await DoExecuteAsync(connectionId);
        }
        catch (Exception ex)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("Failed");
            _logger.LogError(ex, "ImportExecutor出现异常");
        }
    }
    public async Task DoExecuteAsync(string connectionId)
    {
        string[] lines = await File.ReadAllLinesAsync("stardict.csv");//读取文件

        var client = _hubContext.Clients.Client(connectionId);
        await client.SendAsync("Started");

        string connStr = _optionsConnStr.Value.Default;             //读取连接字符串
        using SqlConnection conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        //用于批量导入数据的类
        using SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
        bulkCopy.DestinationTableName = "T_WordItems";
        bulkCopy.ColumnMappings.Add("Word1", "Word");
        bulkCopy.ColumnMappings.Add("Phonetic1", "Phonetic");
        bulkCopy.ColumnMappings.Add("Definition1", "Definition");
        bulkCopy.ColumnMappings.Add("Tranalation1", "Tranalation");

        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Word1");
        dataTable.Columns.Add("Phonetic1");
        dataTable.Columns.Add("Definition1");
        dataTable.Columns.Add("Tranalation1");

        int count = lines.Length;
        for (int i = 1; i < count; i++)//跳过表头
        {
            string line = lines[i];
            string[] segments = line.Split(',');

            string word = segments[0];
            string? phonetic = segments[1];
            string? definition = segments[2];
            string? translation = segments[3];

            var dataRow = dataTable.NewRow();
            dataRow["Word1"] = word;
            dataRow["Phonetic1"] = phonetic;
            dataRow["Definition1"] = definition;
            dataRow["Tranalation1"] = translation;
            dataTable.Rows.Add(dataRow);

            if (dataTable.Rows.Count == 1000)
            {
                await bulkCopy.WriteToServerAsync(dataTable);
                dataTable.Clear();
                await client.SendAsync("ImportProgress", i, count);
            }
        }
        await client.SendAsync("ImportProgress", count, count);
        await bulkCopy.WriteToServerAsync(dataTable);              //处理剩余的一组
        await client.SendAsync("Completed");
    }
}