﻿using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SQLiteData : IDatabaseData
    {
        private const string connectionStringName = "SQLitedb";
        private readonly ISQLiteDataAccess _db;

        public SQLiteData(ISQLiteDataAccess db)
        {
            _db = db;
        }

        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
           

           string sql = "select 1 from Guests where FirstName=@firstName and LastName=@lastName";
            int  results = _db.LoadData<dynamic, dynamic>(sql, new { firstName,lastName },connectionStringName).Count();
          if(results==0)
            {
                sql = @"insert into Guests (FirstName,LastName) values (@firstName,@lastName)";
                _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
            }
            sql = @" select  [Id], [FirstName], [LastName]
                            from Guests where FirstName=@firstName and LastName=@lastName LIMIT 1";

            GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
                                                                   new { firstName, lastName },
                                                                   connectionStringName).FirstOrDefault();

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id=@Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName).First();
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);
            sql = @"select r.*
                        from 
                        Rooms r
                        inner join RoomTypes t on t.Id=r.RoomTypeId
                        and
                        r.Id not in(

                        select b.RoomId from
                       Bookings b
                        where @startDate<b.StartDate and @endDate>b.EndDate
                        or (b.StartDate <=@endDate and @endDate< b.EndDate)
                        or(b.StartDate<=@startDate and @startDate< b.EndDate)
                        )";
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
                                                                              new { startDate, endDate},
                                                                              connectionStringName).ToList();

            sql = @"insert into Bookings (RoomId,GuestId,StartDate,EndDate,TotalCost) 
values(@roomId,@guestId,@startDate,@endDate,@totalCost)";
            
            _db.SaveData(sql,
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = timeStaying.Days * roomType.Price 
                         },
                         connectionStringName);
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = @"update Bookings
                            set CheckedIn=1
                            where Id=@Id";

            _db.SaveData(sql,
                         new { Id = bookingId },
                         connectionStringName
                         );
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql = @"select t.Id,t.Title,t.Description,t.Price
                            from 
                            Rooms r
                            inner join RoomTypes t on t.Id=r.RoomTypeId
                            where r.Id not in(

                            select b.RoomId from
                            Bookings b
                            where @startDate<b.StartDate and @endDate>b.EndDate
                            or (b.StartDate <=@endDate and @endDate< b.EndDate)
                            or(b.StartDate<=@startDate and @startDate< b.EndDate)
                            )
                            group by t.Id,t.Title,t.Description,t.Price";

            return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                         new { startDate, endDate },
                                                         connectionStringName);
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            string sql = @"select [Id], [Title], [Description], [Price] 
                              from RoomTypes
                              where Id =@id;";
            return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                      new { id },
                                                      connectionStringName).FirstOrDefault();
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            string sql = @"select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], 
		                    [b].[EndDate], [b].[CheckedIn], [b].[TotalCost],
		                     [g].[FirstName], [g].[LastName],
		                     [r].[RoomNumber], [r].[RoomTypeId],
		                     [rt].[Title], [rt].[Description], [rt].[Price]
                                from Bookings b 
                                inner join Guests g on b.GuestId=g.Id
                                inner join Rooms r on b.RoomId=r.Id
                                inner join RoomTypes rt on r.RoomTypeId=rt.Id
                                where b.StartDate=@startDate and g.LastName=@lastName";
            return _db.LoadData<BookingFullModel, dynamic>(sql,
                                                         new { lastName, startDate = DateTime.Now.Date },
                                                         connectionStringName);
        }
    }
}
