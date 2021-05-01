using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using System.Linq;

namespace HotelAppLibrary.Databases
{
    public class SQLiteDataAccess : ISQLiteDataAccess
    {
        private readonly IConfiguration _configuration;

        public SQLiteDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<T> LoadData<T, U>(string sqlStatement,
                                     U parameters,
                                     string connectionStringName
                                    )
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {

                List<T> rows = connection.Query<T>(sqlStatement,
                                                   parameters).ToList();
                return rows;
            }
        }


        public void SaveData<T>(string sqlStatement,
                                    T parameters,
                                    string connectionStringName
                                    )
        {

            string connectionString = _configuration.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {



                connection.Execute(sqlStatement, parameters);

            }
        }

    }
}
