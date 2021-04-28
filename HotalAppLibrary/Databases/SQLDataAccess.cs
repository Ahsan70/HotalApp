using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HotalAppLibrary.Databases
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _configuration;

        public SQLDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<T> LoadData<T, U>(string sqlStatement,
                                     U parameters,
                                     string connectionStringName,
                                     bool isStoredProcedure = false)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                CommandType commandType = CommandType.Text;

                if (!(isStoredProcedure = true))
                {
                }
                else
                {
                    commandType = CommandType.StoredProcedure;
                }
                List<T> rows = connection.Query<T>(sqlStatement,
                                                   parameters,
                                                   commandType: commandType).ToList();
                return rows;
            }
        }


        public void SaveData<T>(string sqlStatement,
                                    T parameters,
                                    string connectionStringName,
                                    bool isStoredProcedure = false)
        {

            string connectionString = _configuration.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                CommandType commandType = CommandType.Text;


                if (!(isStoredProcedure = true))
                {
                }
                else
                {
                    commandType = CommandType.StoredProcedure;
                }
                connection.Execute(sqlStatement,
                                                    parameters,
                                                    commandType: commandType);

            }
        }
    }
}
