CREATE TABLE [dbo].[Rooms]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [RoomNumber] VARCHAR(50) NULL, 
    [RoomTypeId] INT NULL, 
    CONSTRAINT [FK_Rooms_ToRoomTypes] FOREIGN KEY (RoomTypeId) REFERENCES RoomTypes(Id)
)
