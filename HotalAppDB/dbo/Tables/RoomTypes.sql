CREATE TABLE [dbo].[RoomTypes]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [Title] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(2000) NOT NULL, 
    [Price] MONEY NOT NULL
)
