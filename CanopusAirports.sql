USE [master]
GO
/****** Object:  Database [CanopusAirports]    Script Date: 9.12.2025 19:56:41 ******/
CREATE DATABASE [CanopusAirports]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CanopusAirports', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CanopusAirports.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CanopusAirports_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CanopusAirports_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CanopusAirports] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CanopusAirports].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CanopusAirports] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CanopusAirports] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CanopusAirports] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CanopusAirports] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CanopusAirports] SET ARITHABORT OFF 
GO
ALTER DATABASE [CanopusAirports] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CanopusAirports] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CanopusAirports] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CanopusAirports] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CanopusAirports] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CanopusAirports] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CanopusAirports] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CanopusAirports] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CanopusAirports] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CanopusAirports] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CanopusAirports] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CanopusAirports] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CanopusAirports] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CanopusAirports] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CanopusAirports] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CanopusAirports] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CanopusAirports] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CanopusAirports] SET RECOVERY FULL 
GO
ALTER DATABASE [CanopusAirports] SET  MULTI_USER 
GO
ALTER DATABASE [CanopusAirports] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CanopusAirports] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CanopusAirports] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CanopusAirports] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CanopusAirports] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CanopusAirports] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CanopusAirports', N'ON'
GO
ALTER DATABASE [CanopusAirports] SET QUERY_STORE = ON
GO
ALTER DATABASE [CanopusAirports] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CanopusAirports]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetLocationLabel]    Script Date: 9.12.2025 19:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetLocationLabel] (@IataCode VARCHAR(10))
RETURNS VARCHAR(300)
AS
BEGIN
    DECLARE @Result VARCHAR(300);
    
    SELECT @Result = city + ' (' + iata + ')' 
    FROM Airports 
    WHERE iata = @IataCode;

    RETURN @Result;
END;
GO
/****** Object:  Table [dbo].[Airports]    Script Date: 9.12.2025 19:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Airports](
	[airport_id] [int] IDENTITY(1,1) NOT NULL,
	[airport_name] [varchar](255) NOT NULL,
	[city] [varchar](255) NULL,
	[country] [varchar](255) NULL,
	[iata] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[airport_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_AirportCountsByCountry]    Script Date: 9.12.2025 19:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_AirportCountsByCountry] AS
SELECT 
    country, 
    COUNT(*) as TotalAirports
FROM 
    Airports
GROUP BY 
    country;
GO
SET IDENTITY_INSERT [dbo].[Airports] ON 

INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (1, N'Minsk National Airport', N'Minsk', N'Belarus', N'MSQ')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (2, N'Tocumen International Airport', N'Tocumen', N'Panama', N'PTY')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (3, N'Aden International Airport', N'Aden', N'Yemen', N'ADE')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (4, N'La Aurora International Airport', N'Guatemala City', N'Guatemala', N'GUA')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (5, N'Bandaranaike International Airport', N'Colombo', N'Sri Lanka', N'CMB')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (6, N'Malta International Airport', N'Valletta', N'Malta', N'MLA')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (7, N'Monseñor Óscar Arnulfo Romero International Airport', N'San Salvador', N'El Salvador', N'SAL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (8, N'Tribhuvan International Airport', N'Kathmandu', N'Nepal', N'KTM')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (9, N'Jinnah International Airport', N'Karachi', N'Pakistan', N'KHI')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (10, N'Istanbul Airport', N'Istanbul', N'Turkey', N'IST')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (11, N'Sabiha Gokcen International Airport', N'Istanbul', N'Turkey', N'SAW')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (12, N'Ankara Esenboga Airport', N'Ankara', N'Turkey', N'ESB')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (13, N'Antalya Airport', N'Antalya', N'Turkey', N'AYT')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (14, N'Frankfurt Airport', N'Frankfurt', N'Germany', N'FRA')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (15, N'Munich Airport', N'Munich', N'Germany', N'MUC')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (16, N'Berlin Brandenburg Airport', N'Berlin', N'Germany', N'BER')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (17, N'London Heathrow Airport', N'London', N'United Kingdom', N'LHR')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (18, N'London Gatwick Airport', N'London', N'United Kingdom', N'LGW')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (19, N'Manchester Airport', N'Manchester', N'United Kingdom', N'MAN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (20, N'Charles de Gaulle Airport', N'Paris', N'France', N'CDG')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (21, N'Orly Airport', N'Paris', N'France', N'ORY')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (22, N'Nice Côte d''Azur Airport', N'Nice', N'France', N'NCE')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (23, N'Rome Fiumicino Airport', N'Rome', N'Italy', N'FCO')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (24, N'Milan Malpensa Airport', N'Milan', N'Italy', N'MXP')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (25, N'Venice Marco Polo Airport', N'Venice', N'Italy', N'VCE')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (26, N'Madrid Barajas Airport', N'Madrid', N'Spain', N'MAD')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (27, N'Barcelona El Prat Airport', N'Barcelona', N'Spain', N'BCN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (28, N'Amsterdam Schiphol Airport', N'Amsterdam', N'Netherlands', N'AMS')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (29, N'Zurich Airport', N'Zurich', N'Switzerland', N'ZRH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (30, N'Brussels Airport', N'Brussels', N'Belgium', N'BRU')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (31, N'Vienna International Airport', N'Vienna', N'Austria', N'VIE')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (32, N'Warsaw Chopin Airport', N'Warsaw', N'Poland', N'WAW')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (33, N'Prague Václav Havel Airport', N'Prague', N'Czech Republic', N'PRG')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (34, N'Athens International Airport', N'Athens', N'Greece', N'ATH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (35, N'Budapest Airport', N'Budapest', N'Hungary', N'BUD')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (36, N'Lisbon Airport', N'Lisbon', N'Portugal', N'LIS')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (37, N'Dublin Airport', N'Dublin', N'Ireland', N'DUB')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (38, N'Copenhagen Airport', N'Copenhagen', N'Denmark', N'CPH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (39, N'Stockholm Arlanda Airport', N'Stockholm', N'Sweden', N'ARN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (40, N'Oslo Gardermoen Airport', N'Oslo', N'Norway', N'OSL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (41, N'Beijing Capital International Airport', N'Beijing', N'China', N'PEK')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (42, N'Shanghai Pudong Airport', N'Shanghai', N'China', N'PVG')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (43, N'Guangzhou Baiyun Airport', N'Guangzhou', N'China', N'CAN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (44, N'Tokyo Haneda Airport', N'Tokyo', N'Japan', N'HND')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (45, N'Tokyo Narita Airport', N'Tokyo', N'Japan', N'NRT')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (46, N'Osaka Kansai Airport', N'Osaka', N'Japan', N'KIX')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (47, N'Incheon International Airport', N'Seoul', N'South Korea', N'ICN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (48, N'Delhi Airport', N'Delhi', N'India', N'DEL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (49, N'Mumbai Airport', N'Mumbai', N'India', N'BOM')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (50, N'Bangalore Airport', N'Bangalore', N'India', N'BLR')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (51, N'Jakarta Airport', N'Jakarta', N'Indonesia', N'CGK')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (52, N'Bali Ngurah Rai Airport', N'Denpasar', N'Indonesia', N'DPS')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (53, N'Dubai International Airport', N'Dubai', N'UAE', N'DXB')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (54, N'Abu Dhabi International Airport', N'Abu Dhabi', N'UAE', N'AUH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (55, N'Riyadh King Khalid Airport', N'Riyadh', N'Saudi Arabia', N'RUH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (56, N'Jeddah King Abdulaziz Airport', N'Jeddah', N'Saudi Arabia', N'JED')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (57, N'Doha Hamad International Airport', N'Doha', N'Qatar', N'DOH')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (58, N'Singapore Changi Airport', N'Singapore', N'Singapore', N'SIN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (59, N'Bangkok Suvarnabhumi Airport', N'Bangkok', N'Thailand', N'BKK')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (60, N'Kuala Lumpur International Airport', N'Kuala Lumpur', N'Malaysia', N'KUL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (61, N'Manila Ninoy Aquino Airport', N'Manila', N'Philippines', N'MNL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (62, N'Ho Chi Minh City Airport', N'Ho Chi Minh City', N'Vietnam', N'SGN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (63, N'Hanoi Noi Bai Airport', N'Hanoi', N'Vietnam', N'HAN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (64, N'Los Angeles International Airport', N'Los Angeles', N'United States', N'LAX')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (65, N'New York JFK Airport', N'New York', N'United States', N'JFK')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (66, N'Chicago O''Hare Airport', N'Chicago', N'United States', N'ORD')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (67, N'Dallas Fort Worth Airport', N'Dallas', N'United States', N'DFW')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (68, N'Atlanta Hartsfield-Jackson Airport', N'Atlanta', N'United States', N'ATL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (69, N'San Francisco International Airport', N'San Francisco', N'United States', N'SFO')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (70, N'Toronto Pearson Airport', N'Toronto', N'Canada', N'YYZ')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (71, N'Vancouver International Airport', N'Vancouver', N'Canada', N'YVR')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (72, N'Montreal Trudeau Airport', N'Montreal', N'Canada', N'YUL')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (73, N'Mexico City International Airport', N'Mexico City', N'Mexico', N'MEX')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (74, N'Cancun International Airport', N'Cancun', N'Mexico', N'CUN')
INSERT [dbo].[Airports] ([airport_id], [airport_name], [city], [country], [iata]) VALUES (75, N'Guadalajara International Airport', N'Guadalajara', N'Mexico', N'GDL')
SET IDENTITY_INSERT [dbo].[Airports] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Airports__9874D2348B3BF62C]    Script Date: 9.12.2025 19:56:41 ******/
ALTER TABLE [dbo].[Airports] ADD UNIQUE NONCLUSTERED 
(
	[iata] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNewAirport]    Script Date: 9.12.2025 19:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AddNewAirport]
    @Name VARCHAR(255),
    @City VARCHAR(255),
    @Country VARCHAR(255),
    @Iata VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Airports WHERE iata = @Iata)
    BEGIN
        PRINT 'This IATA code is already registered!'
    END
    ELSE
    BEGIN
        INSERT INTO Airports (airport_name, city, country, iata)
        VALUES (@Name, @City, @Country, @Iata);
    END
END;
GO
USE [master]
GO
ALTER DATABASE [CanopusAirports] SET  READ_WRITE 
GO
