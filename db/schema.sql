IF DB_ID('StarCorpTravel') IS NULL
    CREATE DATABASE StarCorpTravel;
GO

USE StarCorpTravel;
GO

IF OBJECT_ID('dbo.Payments', 'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID('dbo.Passengers', 'U') IS NOT NULL DROP TABLE dbo.Passengers;
IF OBJECT_ID('dbo.Bookings', 'U') IS NOT NULL DROP TABLE dbo.Bookings;
IF OBJECT_ID('dbo.Flights', 'U') IS NOT NULL DROP TABLE dbo.Flights;
IF OBJECT_ID('dbo.Airlines', 'U') IS NOT NULL DROP TABLE dbo.Airlines;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

CREATE TABLE dbo.Customers
(
    Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    Email           NVARCHAR(200) NOT NULL,
    PhoneNumber     NVARCHAR(50) NOT NULL,
    Address         NVARCHAR(400) NOT NULL,
    DocumentNumber  NVARCHAR(20) NOT NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL,
    UpdatedAt       DATETIME2 NOT NULL
);
GO

CREATE TABLE dbo.Airlines
(
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Airlines PRIMARY KEY,
    Name        NVARCHAR(200) NOT NULL,
    Code        NVARCHAR(10) NOT NULL,
    Cnpj        NVARCHAR(20) NOT NULL,
    IsActive    BIT NOT NULL CONSTRAINT DF_Airlines_IsActive DEFAULT (1),
    CreatedAt   DATETIME2 NOT NULL,
    UpdatedAt   DATETIME2 NOT NULL
);
GO

CREATE TABLE dbo.Flights
(
    Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Flights PRIMARY KEY,
    Origin          NVARCHAR(100) NOT NULL,
    Destination     NVARCHAR(100) NOT NULL,
    DepartureTime   DATETIME2 NOT NULL,
    ArrivalTime     DATETIME2 NOT NULL,
    BasePrice       DECIMAL(18, 2) NOT NULL,
    EconomySeats    INT NOT NULL,
    BusinessSeats   INT NOT NULL,
    AirlineId       UNIQUEIDENTIFIER NOT NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Flights_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL,
    UpdatedAt       DATETIME2 NOT NULL,
    CONSTRAINT FK_Flights_Airlines FOREIGN KEY (AirlineId) REFERENCES dbo.Airlines (Id)
);
GO

CREATE INDEX IX_Flights_Search ON dbo.Flights (Origin, Destination, DepartureTime);
GO

CREATE TABLE dbo.Bookings
(
    Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Bookings PRIMARY KEY,
    CustomerId      UNIQUEIDENTIFIER NOT NULL,
    FlightId        UNIQUEIDENTIFIER NOT NULL,
    BookingClass    INT NOT NULL,
    Status          INT NOT NULL,
    TotalAmount     DECIMAL(18, 2) NOT NULL,
    CreatedAt       DATETIME2 NOT NULL,
    UpdatedAt       DATETIME2 NOT NULL,
    CONSTRAINT FK_Bookings_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (Id),
    CONSTRAINT FK_Bookings_Flights FOREIGN KEY (FlightId) REFERENCES dbo.Flights (Id)
);
GO

CREATE TABLE dbo.Passengers
(
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Passengers PRIMARY KEY,
    BookingId   UNIQUEIDENTIFIER NOT NULL,
    Name        NVARCHAR(200) NOT NULL,
    Document    NVARCHAR(20) NOT NULL,
    CreatedAt   DATETIME2 NOT NULL,
    UpdatedAt   DATETIME2 NOT NULL,
    CONSTRAINT FK_Passengers_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings (Id)
);
GO

CREATE TABLE dbo.Payments
(
    Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Payments PRIMARY KEY,
    BookingId       UNIQUEIDENTIFIER NOT NULL,
    CustomerId      UNIQUEIDENTIFIER NOT NULL,
    Amount          DECIMAL(18, 2) NOT NULL,
    PaymentMethod   INT NOT NULL,
    Status          INT NOT NULL,
    PaidAt          DATETIME2 NULL,
    CreatedAt       DATETIME2 NOT NULL,
    UpdatedAt       DATETIME2 NOT NULL,
    CONSTRAINT FK_Payments_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings (Id),
    CONSTRAINT FK_Payments_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (Id)
);
GO
