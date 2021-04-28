CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@Id int 
AS
begin
update dbo.Bookings
set CheckedIn=1
where Id=@Id
end