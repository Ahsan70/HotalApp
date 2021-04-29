﻿using HotalAppLibrary.Models;
using System;
using System.Collections.Generic;

namespace HotalAppLibrary.Data
{
    public interface IDatabaseData
    {
        void BookGuest(string fristName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId);
        void CheckInGuest(int bookingId);
        List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
        List<BookingFullModel> SearchBookings(string lastName);
    }
}