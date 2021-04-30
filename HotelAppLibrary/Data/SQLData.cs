using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SQLData : IDatabaseData
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
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailables",
                                                          new { startDate, endDate },
                                                          connectionStringName,
                                                          true);
        }


        public void BookGuest(string fristName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                                                                  new { fristName, lastName },
                                                                  connectionStringName,
                                                                  true).FirstOrDefault();
           
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id=@Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName,
                                                                          false).First();
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName,
                                                                              true).ToList();
            _db.SaveData("dbo.spBookings_Insert",
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName,
                         true);



        }


        public List<BookingFullModel> SearchBookings(string lastName)
        {
            return _db.LoadData<BookingFullModel, dynamic>("dbo.spBookings_Search",
                                                          new { lastName, startDate = DateTime.Now.Date },
                                                          connectionStringName,
                                                          true);

        }

        public void CheckInGuest(int bookingId)
        {

            _db.SaveData("dbo.spBookings_CheckIn",
                         new { Id = bookingId },
                         connectionStringName,
                         true);
        }


        public RoomTypeModel GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
                                                        new { id },
                                                        connectionStringName,
                                                        true).FirstOrDefault();
        }
    }
}
