
ALTER TABLE Flights ADD status NVARCHAR(50) DEFAULT 'On Time';
ALTER TABLE Flights ADD capacity INT DEFAULT 150;
GO

UPDATE Flights SET status = 'On Time', capacity = 150 WHERE status IS NULL;