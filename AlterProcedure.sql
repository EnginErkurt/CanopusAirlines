ALTER PROCEDURE sp_SearchFlights
    @DepartureID INT,
    @ArrivalID INT,
    @Date DATETIME,
    @ClassType VARCHAR(20) 
AS
BEGIN
    SELECT 
        f.flight_id,
        f.flight_number,
        f.flight_date,
        CASE 
            WHEN @ClassType = 'Business' THEN f.price_business 
            ELSE f.price 
        END AS price, 
        
        dep.city AS FromCity, 
        arr.city AS ToCity     
    FROM 
        Flights f
    INNER JOIN Airports dep ON f.departure_id = dep.airport_id 
    INNER JOIN Airports arr ON f.arrival_id = arr.airport_id   
    WHERE 
        f.departure_id = @DepartureID 
        AND f.arrival_id = @ArrivalID
        AND CAST(f.flight_date AS DATE) >= CAST(@Date AS DATE)
        AND (@ClassType = 'Economy' OR (@ClassType = 'Business' AND f.price_business IS NOT NULL));
END;