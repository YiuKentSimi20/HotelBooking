-- Verificăm dacă baza există, altfel o creăm
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Hotel_Booking_Db')
BEGIN
    CREATE DATABASE Hotel_Booking_Db;
END
GO

USE Hotel_Booking_Db;
GO

-- Ștergem tabela dacă există deja (pentru a putea recrea structura corectă)
IF OBJECT_ID('dbo.Bookings', 'U') IS NOT NULL
DROP TABLE dbo.Bookings;
GO

CREATE TABLE Bookings (
                          BookingId INT IDENTITY(1,1) PRIMARY KEY, -- ID INT cu auto-incrementare
                          CustomerName NVARCHAR(200) NOT NULL,     -- Adaugat conform cerintei
                          CustomerEmail NVARCHAR(200) NOT NULL,
                          RoomType NVARCHAR(50) NOT NULL,
                          CheckInDate DATE NOT NULL,
                          CheckOutDate DATE NOT NULL,
                          TotalAmount DECIMAL(18, 2) NOT NULL,
                          PaymentTransactionId NVARCHAR(100) NOT NULL,
                          CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    -- Currency a fost eliminat
);
GO

-- Index pentru căutări rapide
CREATE INDEX IX_Bookings_CustomerEmail ON Bookings(CustomerEmail);
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Hotel_Invoicing_Db')
BEGIN
    CREATE DATABASE Hotel_Invoicing_Db;
END
GO

USE Hotel_Invoicing_Db;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
DROP TABLE dbo.Invoices;
GO

CREATE TABLE Invoices (
                          InvoiceId INT IDENTITY(1,1) PRIMARY KEY, -- ID INT cu auto-incrementare
                          BookingId INT NOT NULL,                  -- Referință INT către BookingId din prima bază
                          InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
                          CustomerName NVARCHAR(200),
                          CustomerEmail NVARCHAR(200),             -- Modificat din Address in Email conform cerintei
                          NetValue DECIMAL(18, 2) NOT NULL,
                          VatValue DECIMAL(18, 2) NOT NULL,
                          TotalValue DECIMAL(18, 2) NOT NULL,
                          IssuedDate DATETIME2 NOT NULL DEFAULT GETDATE()
    -- Currency a fost eliminat
);
GO

CREATE UNIQUE INDEX IX_Invoices_BookingId ON Invoices(BookingId);
GO
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Hotel_Accommodation_Db')
BEGIN
    CREATE DATABASE Hotel_Accommodation_Db;
END
GO

USE Hotel_Accommodation_Db;
GO

-- Ordinea ștergerii contează din cauza Foreign Keys
IF OBJECT_ID('dbo.RoomAssignments', 'U') IS NOT NULL
DROP TABLE dbo.RoomAssignments;
GO

IF OBJECT_ID('dbo.Rooms', 'U') IS NOT NULL
DROP TABLE dbo.Rooms;
GO

-- 1. Tabela Rooms (Inventarul)
CREATE TABLE Rooms (
                       RoomId INT IDENTITY(1,1) PRIMARY KEY,    -- ID INT cu auto-incrementare
                       RoomNumber NVARCHAR(10) NOT NULL UNIQUE,
                       RoomType NVARCHAR(50) NOT NULL,
                       IsClean BIT NOT NULL DEFAULT 1,          -- 1 = True, 0 = False
                       IsOutOfService BIT NOT NULL DEFAULT 0
);
GO

-- 2. Tabela RoomAssignments (Logica de Check-In)
CREATE TABLE RoomAssignments (
                                 AssignmentId INT IDENTITY(1,1) PRIMARY KEY, -- ID INT cu auto-incrementare
                                 BookingId INT NOT NULL,                     -- Referință INT către BookingId
                                 RoomId INT NOT NULL,
                                 CheckInTime DATETIME2 NOT NULL DEFAULT GETDATE(),
                                 CheckOutTime DATETIME2 NULL,
                                 AccessCode NVARCHAR(20) NOT NULL,

                                 CONSTRAINT FK_Assignments_Rooms FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);
GO

CREATE INDEX IX_RoomAssignments_RoomId ON RoomAssignments(RoomId);
GO

-- 3. Populare cu date de start (Seeding)
-- Verificăm dacă e gol înainte să inserăm, pentru a nu duplica la rulări multiple
IF NOT EXISTS (SELECT 1 FROM Rooms)
BEGIN
INSERT INTO Rooms (RoomNumber, RoomType, IsClean) VALUES
                                                      ('101', 'Single', 1),
                                                      ('102', 'Single', 1),
                                                      ('201', 'Double', 1),
                                                      ('202', 'Double', 1),
                                                      ('203', 'Double', 1),
                                                      ('301', 'Suite', 1);
END
GO