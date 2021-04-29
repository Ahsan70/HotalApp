CREATE PROCEDURE [dbo].[spGuests_Insert]
	@fristName nvarchar(50),
	@lastName nvarchar(50)

as
begin
set nocount on;
if not exists (select 1 from dbo.Guests where FirstName=@fristName and LastName=@lastName)
begin
insert into dbo.Guests (FirstName,LastName) values (@fristName,@lastName) 
end
select top 1 [Id], [FirstName], [LastName]
from dbo.Guests where FirstName=@fristName and LastName=@lastName
end
