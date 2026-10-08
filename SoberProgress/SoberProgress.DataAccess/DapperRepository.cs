using Dapper;
using Microsoft.Data.Sqlite;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;
using System.Data;

namespace SoberProgress.DataAccess
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString = "Data Source=sober.db";

        public DapperRepository()
        {
            using var db = new SqliteConnection(_connectionString);
            db.Open();

            var sqlCreate = @"
                CREATE TABLE IF NOT EXISTS AlcoUsers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NULL,
                    Surname TEXT NULL,
                    Patronymic TEXT NULL,
                    RegistryDate TEXT NULL,
                    LastDrinkDate TEXT NULL,
                    IsCoded INTEGER NOT NULL
                );";

            db.Execute(sqlCreate);
        }

        private IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public IEnumerable<T> ReadAll()
        {
            using var db = CreateConnection();
            return db.Query<T>("SELECT * FROM AlcoUsers").ToList();
        }

        public T ReadById(int id)
        {
            using var db = CreateConnection();
            return db.QueryFirstOrDefault<T>("SELECT * FROM AlcoUsers WHERE Id = @Id", new { Id = id });
        }

        public void Create(T item)
        {
            if (item is AlcoUser user)
            {
                using var db = CreateConnection();
                var sql = @"INSERT INTO AlcoUsers (Name, Surname, Patronymic, RegistryDate, LastDrinkDate, IsCoded) 
                            VALUES (@Name, @Surname, @Patronymic, @RegistryDate, @LastDrinkDate, @IsCoded);";
                db.Execute(sql, user);
            }
        }

        public void Update(T item)
        {
            if (item is AlcoUser user)
            {
                using var db = CreateConnection();
                var sql = @"UPDATE AlcoUsers 
                            SET Name = @Name, Surname = @Surname, Patronymic = @Patronymic, 
                                RegistryDate = @RegistryDate, LastDrinkDate = @LastDrinkDate, IsCoded = @IsCoded 
                            WHERE Id = @Id;";
                db.Execute(sql, user);
            }
        }

        public void Delete(int id)
        {
            using var db = CreateConnection();
            db.Execute("DELETE FROM AlcoUsers WHERE Id = @Id", new { Id = id });
        }
    }
}
