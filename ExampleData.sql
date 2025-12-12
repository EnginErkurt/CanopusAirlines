CREATE TABLE Flights (
    flight_id INT PRIMARY KEY IDENTITY(1,1),
    departure_id INT, 
    arrival_id INT,   
    flight_date DATETIME,
    price DECIMAL(10,2),
    flight_number VARCHAR(50)
);

INSERT INTO Flights (departure_id, arrival_id, flight_date, price, flight_number)
VALUES 
(10, 12, '2025-12-25 10:00:00', 1500.00, 'CNP-101'), -- İstanbul -> Ankara
(10, 13, '2025-12-26 14:00:00', 1200.00, 'CNP-102'); -- İstanbul -> Antalya