using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DataLibrary
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _config;
        public string ConnectionStringName { get; set; } = @"Data Source = SmallDB.db";
        public SQLDataAccess()
        {
            
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            //string connectionString = _config.GetConnectionString(ConnectionStringName);

            //using (IDbConnection connection = new SQLiteConnection(connectionString))
            //{
            //    var data = await connection.QueryAsync<T>(sql, parameters);

            //    return data.ToList();
            //}
            SQLiteConnection connection = new SQLiteConnection(ConnectionStringName);
            connection.Open();
            var data = await connection.QueryAsync<T>(sql, parameters);
            return data.ToList();


        }

        public async Task Savedate<T>(string sql, T parameters)
        {
        //    string connectionString = _config.GetConnectionString(ConnectionStringName);

        //    using (IDbConnection connection = new SQLiteConnection(connectionString))
        //    {
        //        var data = await connection.QueryAsync<T>(sql, parameters);

        //        await connection.ExecuteAsync(sql, parameters);
        //    }
            SQLiteConnection connection = new SQLiteConnection(ConnectionStringName);
            connection.Open();
            var data = await connection.QueryAsync<T>(sql, parameters);
            await connection.ExecuteAsync(sql, parameters);

        }
    }
}
