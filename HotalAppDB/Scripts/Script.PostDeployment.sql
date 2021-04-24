/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select 1 from dbo.RoomTypes)
begin
insert into dbo.RoomTypes(Title,Description,Price) values
('King Size Bed','A room with a king size bed and window',100),
('Two Queen Bed','A room with a two queen bed and window',400),
('Excuetive Suite','Two rooms,with a king size bed and window',1000)
end


if not exists (select 1 from dbo.Rooms)
begin
declare @roomIdOne int ;
declare @roomIdTwo int ;
declare @roomIdThree int;
select @roomIdOne=Id from dbo.RoomTypes where Title='King Size Bed';
select @roomIdTwo=Id from dbo.RoomTypes where Title='Two Queen Bed';
select @roomIdThree=Id from dbo.RoomTypes where Title='Excuetive Suite';
insert into dbo.Rooms(RoomNumber,RoomTypeId) values
('101',@roomIdOne),
('102',@roomIdOne),
('103',@roomIdOne),
('201',@roomIdTwo),
('202',@roomIdTwo),
('204',@roomIdThree)
end