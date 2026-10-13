using Dapper;
using Microsoft.Data.Sqlite;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;
using System.Data;

namespace SoberProgress.DataAccess
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly string _connectionString = "Data Source=sober.db";
        private readonly IDbConnection _connection;

        private readonly TableMetadata _metadata;

        private bool _disposed = false;

        public DapperRepository()
        {
            _metadata = SqlExpressionBuilder.GetMetadata<T>();

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            _connection.Execute(_metadata.CreateTableSql);
            foreach (var cp in _metadata.ComplexProperties)
            {
                var childMetadata = SqlExpressionBuilder.GetMetadata(cp.PropertyType);
                _connection.Execute(childMetadata.CreateTableSql);
            }
        }

        public IEnumerable<T> ReadAll() => _connection.Query<T>(_metadata.SelectAllSql).ToList();

        public T ReadById(int id)
        {
            return _connection.QueryFirstOrDefault<T>(_metadata.SelectByIdSql, new { Id = id });
        }

        public void Create(T item)
        {
            if (item == null) return;

            var parameters = SqlExpressionBuilder.PrepareParameters(_connection, item, _metadata);

            int id = _connection.ExecuteScalar<int>(_metadata.InsertSql, parameters);

            typeof(T).GetProperty("Id")?.SetValue(item, id);
        }

        public void Update(T item)
        {
            if (item == null) return;
            _connection.Execute(_metadata.UpdateSql, item);
        }

        public void Delete(int id)
        {
            _connection.Execute(_metadata.DeleteSql, new { Id = id });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.State != ConnectionState.Closed)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~DapperRepository()
        {
            Dispose(false);
        }
    }
}
