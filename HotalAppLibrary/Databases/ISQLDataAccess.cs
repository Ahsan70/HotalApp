using System.Collections.Generic;

namespace HotalAppLibrary.Databases
{
    public interface ISQLDataAccess
    {
        List<T> LoadData<T, U>(string sqlStatement,
                               U parameters,
                               string connectionStringName,
                               dynamic options = null);
        void SaveData<T>(string sqlStatement,
                         T parameters,
                         string connectionStringName,
                         dynamic options = null);
    }
}