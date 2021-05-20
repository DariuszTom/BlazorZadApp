using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _config;
        public string ConnectionStringName { get; set; } = @"Data Source = .\SmallDB.db;Version=3";

        public SQLDataAccess()
        {
        }

        public SQLDataAccess(string diffConString)
        {
            ConnectionStringName = diffConString;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(ConnectionStringName))
            {
                connection.Open();

                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public async Task Savedate<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(ConnectionStringName))
            {
                connection.Open();
                var data = await connection.QueryAsync<T>(sql, parameters);
                //await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}