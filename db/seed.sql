USE StarCorpTravel;
GO

DECLARE @now DATETIME2 = SYSUTCDATETIME();

INSERT INTO dbo.Airlines (Id, Name, Code, Cnpj, IsActive, CreatedAt, UpdatedAt) VALUES
('a0000000-0000-0000-0000-000000000001', N'LATAM Airlines',     N'LA', N'11.222.333/0001-81', 1, @now, @now),
('a0000000-0000-0000-0000-000000000002', N'GOL Linhas Aéreas',  N'G3', N'33.444.555/0001-81', 1, @now, @now),
('a0000000-0000-0000-0000-000000000003', N'Azul Linhas Aéreas', N'AD', N'44.555.666/0001-81', 1, @now, @now),
('a0000000-0000-0000-0000-000000000004', N'Voepass',            N'2Z', N'55.666.777/0001-81', 1, @now, @now);

INSERT INTO dbo.Customers (Id, Name, Email, PhoneNumber, Address, DocumentNumber, IsActive, CreatedAt, UpdatedAt) VALUES
('c0000000-0000-0000-0000-000000000001', N'Ana Souza',    N'ana@example.com',    N'+5511999990001', N'Rua A, 100 - São Paulo/SP',      N'472.993.038-09', 1, @now, @now),
('c0000000-0000-0000-0000-000000000002', N'Bruno Lima',   N'bruno@example.com',  N'+5521999990002', N'Rua B, 200 - Rio de Janeiro/RJ', N'529.982.247-25', 1, @now, @now),
('c0000000-0000-0000-0000-000000000003', N'Carla Dias',   N'carla@example.com',  N'+5561999990003', N'Rua C, 300 - Brasília/DF',       N'234.567.890-92', 1, @now, @now),
('c0000000-0000-0000-0000-000000000004', N'Diego Alves',  N'diego@example.com',  N'+5571999990004', N'Rua D, 400 - Salvador/BA',       N'345.678.901-75', 1, @now, @now),
('c0000000-0000-0000-0000-000000000005', N'Elaine Costa', N'elaine@example.com', N'+5551999990005', N'Rua E, 500 - Porto Alegre/RS',   N'456.789.012-49', 1, @now, @now),
('c0000000-0000-0000-0000-000000000006', N'Felipe Rocha', N'felipe@example.com', N'+5511999990006', N'Rua F, 600 - São Paulo/SP',      N'567.890.123-03', 0, @now, @now);

INSERT INTO dbo.Flights (Id, Origin, Destination, DepartureTime, ArrivalTime, BasePrice, EconomySeats, BusinessSeats, AirlineId, IsActive, CreatedAt, UpdatedAt) VALUES
('f0000000-0000-0000-0000-000000000001', N'São Paulo',      N'Rio de Janeiro', DATEADD(DAY, 15, @now), DATEADD(HOUR, 1, DATEADD(DAY, 15, @now)), 500.00, 120, 20, 'a0000000-0000-0000-0000-000000000001', 1, @now, @now),
('f0000000-0000-0000-0000-000000000002', N'São Paulo',      N'Rio de Janeiro', DATEADD(DAY, 16, @now), DATEADD(HOUR, 1, DATEADD(DAY, 16, @now)), 460.00, 100, 16, 'a0000000-0000-0000-0000-000000000002', 1, @now, @now),
('f0000000-0000-0000-0000-000000000003', N'São Paulo',      N'Rio de Janeiro', DATEADD(DAY, 18, @now), DATEADD(HOUR, 1, DATEADD(DAY, 18, @now)), 540.00,  90, 12, 'a0000000-0000-0000-0000-000000000003', 1, @now, @now),
('f0000000-0000-0000-0000-000000000004', N'Rio de Janeiro', N'São Paulo',      DATEADD(DAY, 20, @now), DATEADD(HOUR, 1, DATEADD(DAY, 20, @now)), 480.00, 120, 20, 'a0000000-0000-0000-0000-000000000002', 1, @now, @now),
('f0000000-0000-0000-0000-000000000005', N'Rio de Janeiro', N'São Paulo',      DATEADD(DAY, 22, @now), DATEADD(HOUR, 1, DATEADD(DAY, 22, @now)), 520.00, 110, 18, 'a0000000-0000-0000-0000-000000000001', 1, @now, @now),
('f0000000-0000-0000-0000-000000000006', N'Brasília',       N'Salvador',       DATEADD(DAY, 30, @now), DATEADD(HOUR, 2, DATEADD(DAY, 30, @now)), 750.00, 150, 30, 'a0000000-0000-0000-0000-000000000001', 1, @now, @now),
('f0000000-0000-0000-0000-000000000007', N'Salvador',       N'Brasília',       DATEADD(DAY, 32, @now), DATEADD(HOUR, 2, DATEADD(DAY, 32, @now)), 720.00, 140, 28, 'a0000000-0000-0000-0000-000000000003', 1, @now, @now),
('f0000000-0000-0000-0000-000000000008', N'São Paulo',      N'Porto Alegre',   DATEADD(DAY, 25, @now), DATEADD(HOUR, 2, DATEADD(DAY, 25, @now)), 640.00,  80, 10, 'a0000000-0000-0000-0000-000000000004', 1, @now, @now),
('f0000000-0000-0000-0000-000000000009', N'Porto Alegre',   N'São Paulo',      DATEADD(DAY, 27, @now), DATEADD(HOUR, 2, DATEADD(DAY, 27, @now)), 610.00,  80, 10, 'a0000000-0000-0000-0000-000000000002', 1, @now, @now),
('f0000000-0000-0000-0000-000000000010', N'São Paulo',      N'Rio de Janeiro', DATEADD(DAY, 40, @now), DATEADD(HOUR, 1, DATEADD(DAY, 40, @now)), 700.00,   4,  2, 'a0000000-0000-0000-0000-000000000001', 1, @now, @now),
('f0000000-0000-0000-0000-000000000011', N'São Paulo',      N'Miami',          DATEADD(DAY, 60, @now), DATEADD(HOUR, 9, DATEADD(DAY, 60, @now)), 3200.00, 200, 40, 'a0000000-0000-0000-0000-000000000001', 0, @now, @now);

INSERT INTO dbo.Bookings (Id, CustomerId, FlightId, BookingClass, Status, TotalAmount, CreatedAt, UpdatedAt) VALUES
('b0000000-0000-0000-0000-000000000001', 'c0000000-0000-0000-0000-000000000001', 'f0000000-0000-0000-0000-000000000001', 0, 1, 1167.07, @now, @now),
('b0000000-0000-0000-0000-000000000002', 'c0000000-0000-0000-0000-000000000002', 'f0000000-0000-0000-0000-000000000006', 1, 0, 2173.50, @now, @now),
('b0000000-0000-0000-0000-000000000003', 'c0000000-0000-0000-0000-000000000003', 'f0000000-0000-0000-0000-000000000004', 0, 2,  591.57, @now, @now);

INSERT INTO dbo.Passengers (Id, BookingId, Name, Document, CreatedAt, UpdatedAt) VALUES
('d0000000-0000-0000-0000-000000000001', 'b0000000-0000-0000-0000-000000000001', N'Ana Souza',   N'472.993.038-09', @now, @now),
('d0000000-0000-0000-0000-000000000002', 'b0000000-0000-0000-0000-000000000001', N'João Souza',  N'234.567.890-92', @now, @now),
('d0000000-0000-0000-0000-000000000003', 'b0000000-0000-0000-0000-000000000002', N'Bruno Lima',  N'529.982.247-25', @now, @now),
('d0000000-0000-0000-0000-000000000004', 'b0000000-0000-0000-0000-000000000003', N'Carla Dias',  N'345.678.901-75', @now, @now);

INSERT INTO dbo.Payments (Id, BookingId, CustomerId, Amount, PaymentMethod, Status, PaidAt, CreatedAt, UpdatedAt) VALUES
('e0000000-0000-0000-0000-000000000001', 'b0000000-0000-0000-0000-000000000001', 'c0000000-0000-0000-0000-000000000001', 1167.07, 1, 1, DATEADD(HOUR, -2, @now), @now, @now);

UPDATE dbo.Flights SET EconomySeats = EconomySeats - 2 WHERE Id = 'f0000000-0000-0000-0000-000000000001';
UPDATE dbo.Flights SET BusinessSeats = BusinessSeats - 1 WHERE Id = 'f0000000-0000-0000-0000-000000000006';
GO
