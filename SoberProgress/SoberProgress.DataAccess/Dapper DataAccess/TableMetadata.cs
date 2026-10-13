using System.Reflection;

/// <summary>
/// Класс хранения готовых SQL запросов для сущности
/// </summary>
public class TableMetadata
{
    public string InsertSql { get; }
    public string UpdateSql { get; }
    public string SelectAllSql { get; }
    public string SelectByIdSql { get; }
    public string DeleteSql { get; }
    public string CreateTableSql { get; }
    public PropertyInfo[] ComplexProperties { get; }

    public TableMetadata(string insertSql, string updateSql, string selectAllSql, string selectByIdSql, string deleteSql, string createTableSql, PropertyInfo[] complexProperties)
    {
        InsertSql = insertSql;
        UpdateSql = updateSql;
        SelectAllSql = selectAllSql;
        SelectByIdSql = selectByIdSql;
        DeleteSql = deleteSql;
        CreateTableSql = createTableSql;
        ComplexProperties = complexProperties;
    }
}