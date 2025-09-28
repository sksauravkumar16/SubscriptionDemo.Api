-- Create database and tables for SubscriptionDemo
CREATE DATABASE SubscriptionDemo;
GO
USE SubscriptionDemo;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    ResetToken NVARCHAR(255) NULL,
    ResetTokenExpiry DATETIME NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE CustomerSubscriptions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId NVARCHAR(100) NOT NULL,
    CustomerName NVARCHAR(200) NOT NULL,
    SubscriptionName NVARCHAR(200) NOT NULL,
    SubscriptionCount INT NOT NULL DEFAULT 0,
    StartDate DATE NULL,
    EndDate DATE NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

-- Sample data
INSERT INTO CustomerSubscriptions (CustomerId, CustomerName, SubscriptionName, SubscriptionCount, StartDate, EndDate, IsActive)
VALUES ('CUST-001', 'Acme Corp', 'Basic Plan', 5, '2025-01-01', '2025-12-31', 1);
GO
