using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SoberProgress.DataAccess
{
    /// <summary>
    /// Класс для генерации SQL запросов через метаанализ свойств объекта
    /// </summary>
    public static class SqlExpressionBuilder
    {
        private static readonly ConcurrentDictionary<Type, TableMetadata> _cache = new ConcurrentDictionary<Type, TableMetadata>();

        public static TableMetadata GetMetadata<T>() => GetMetadata(typeof(T));
        
        /// <summary>
        /// Формирование запросов для sql на основе типа данных
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <returns>Crud запросы</returns>
        public static TableMetadata GetMetadata(Type type)
        {
            return _cache.GetOrAdd(type, t =>
            {
                string tableName = t.Name;
                var allProperties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var simpleProperties = new List<PropertyInfo>();
                var complexProperties = new List<PropertyInfo>();

                foreach (var p in allProperties)
                {
                    if (p.Name == "Id") continue;

                    if (p.PropertyType.IsClass && p.PropertyType != typeof(string) && p.PropertyType != typeof(DateTime) && p.PropertyType != typeof(DateTime?))
                    {
                        complexProperties.Add(p);
                    }
                    else
                    {
                        simpleProperties.Add(p);
                    }
                }

                var columnDefinitions = new List<string>();

                foreach (var p in simpleProperties)
                {
                    string sqliteType = GetSqliteType(p.PropertyType);
                    columnDefinitions.Add($"{p.Name} {sqliteType} NULL");
                }

                foreach (var cp in complexProperties)
                {
                    columnDefinitions.Add($"{cp.Name}Id INTEGER NULL");
                    GetMetadata(cp.PropertyType);
                }

                string createTableSql = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {string.Join(", ", columnDefinitions)}
                                         );";

                var insertColumns = simpleProperties.Select(p => p.Name).ToList();
                insertColumns.AddRange(complexProperties.Select(p => p.Name + "Id"));

                var insertParams = simpleProperties.Select(p => "@" + p.Name).ToList();
                insertParams.AddRange(complexProperties.Select(p => "@" + p.Name + "Id"));

                string insertSql = $"INSERT INTO {tableName} ({string.Join(", ", insertColumns)}) VALUES ({string.Join(", ", insertParams)}); SELECT last_insert_rowid();";

                var setClauses = simpleProperties.Select(p => $"{p.Name} = @{p.Name}").ToList();
                setClauses.AddRange(complexProperties.Select(p => $"{p.Name}Id = @{p.Name}Id"));
                string updateSql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE Id = @Id;";

                return new TableMetadata(insertSql, updateSql, $"SELECT * FROM {tableName}", $"SELECT * FROM {tableName} WHERE Id = @Id", $"DELETE FROM {tableName} WHERE Id = @Id", createTableSql, complexProperties.ToArray());
            });
        }

        /// <summary>
        /// Сохранение вложенных объектов и сборка параметров
        /// </summary>
        public static DynamicParameters PrepareParameters<T>(IDbConnection connection, T item, TableMetadata metadata) where T : class
        {
            var parameters = new DynamicParameters();

            foreach (var p in typeof(T).GetProperties().Where(p => p.Name != "Id"))
            {
                if (!p.PropertyType.IsClass || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                {
                    parameters.Add(p.Name, p.GetValue(item));
                }
            }
            foreach (var cp in metadata.ComplexProperties)
            {
                var childValue = cp.GetValue(item);
                if (childValue != null)
                {
                    var childMetadata = GetMetadata(cp.PropertyType);

                    int childId = connection.ExecuteScalar<int>(childMetadata.InsertSql, childValue);

                    parameters.Add(cp.Name + "Id", childId);
                }
                else
                {
                    parameters.Add(cp.Name + "Id", null);
                }
            }

            return parameters;
        }

        private static string GetSqliteType(Type type)
        {
            if (type == typeof(int) || type == typeof(bool) || type == typeof(long) ||
                type == typeof(int?) || type == typeof(bool?) || type == typeof(long?) ||
                type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "INTEGER";
            }
            if (type == typeof(double) || type == typeof(float) || type == typeof(double?) || type == typeof(float?))
            {
                return "REAL";
            }
            return "TEXT";
        }
    }
}