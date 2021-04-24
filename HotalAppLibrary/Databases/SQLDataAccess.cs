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
   public class SQLDataAccess
    {
        private readonly IConfiguration _configuration;

        public SQLDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<T> LoadData<T,U>(string sqlStatement,
                                     U parameters,
                                     string connectionStringName)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);
            using (IDbConnection connection =new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement,parameters).ToList();
                return rows;
            }
        }
    }
}
