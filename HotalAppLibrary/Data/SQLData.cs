using HotalAppLibrary.Databases;
using HotalAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotalAppLibrary.Data
{
  public  class SQLData
    {
        private readonly ISQLDataAccess _db;
        private const string connectionStringName = "SQLdb";
        public SQLData(ISQLDataAccess db)
        {
            _db = db;
        }
        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate,
                                                         DateTime endDate)
        {
         return   _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailables",
                                                 new {startDate,endDate },connectionStringName,true);
        }
    }
}
