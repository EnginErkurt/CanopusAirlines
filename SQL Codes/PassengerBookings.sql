-- 1. Yolcular Tablosu
CREATE TABLE Passengers (
    passenger_id INT PRIMARY KEY IDENTITY(1,1),
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    email NVARCHAR(150),
    phone NVARCHAR(50),
    gender NVARCHAR(10),
    birth_date DATETIME
);

-- 2. Biletler Tablosu (Rezervasyonlar)
CREATE TABLE Tickets (
    ticket_id INT PRIMARY KEY IDENTITY(1,1),
    flight_id INT,           -- Hangi uçuş?
    passenger_id INT,        -- Hangi yolcu?
    seat_number VARCHAR(10), -- Koltuk No (12A vb.)
    pnr_code VARCHAR(10),    -- PNR Kodu
    total_price DECIMAL(10,2), -- Ödenen Tutar
    booking_date DATETIME DEFAULT GETDATE(),
    
    -- İlişkiler
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id),
    FOREIGN KEY (passenger_id) REFERENCES Passengers(passenger_id)
);