/*
 Pipe-ready connection string sample:
 Server=np:\\./pipe/LOCALDB#20734081\\tsql\\query;Database=PE2ECommerce;Trusted_Connection=True;MultipleActiveResultSets=True;
*/
USE master;
IF DB_ID('PE2ECommerce') IS NOT NULL
BEGIN
    ALTER DATABASE PE2ECommerce SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PE2ECommerce;
END;
GO
CREATE DATABASE PE2ECommerce;
GO
USE PE2ECommerce;
GO
CREATE TABLE dbo.Users
(
    UserId         INT           IDENTITY(1,1) PRIMARY KEY,
    UserName       NVARCHAR(50)  NOT NULL UNIQUE,
    Email          NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(200) NOT NULL,
    [Lock]         BIT           NOT NULL DEFAULT(0),
    RoleName       NVARCHAR(30)  NOT NULL DEFAULT('Staff')
);
CREATE TABLE dbo.Agent
(
    AgentId    INT IDENTITY(1,1) PRIMARY KEY,
    AgentName  NVARCHAR(120) NOT NULL,
    [Address]  NVARCHAR(200) NOT NULL,
    Phone      NVARCHAR(30)  NOT NULL,
    Email      NVARCHAR(120) NOT NULL
);
CREATE TABLE dbo.Item
(
    ItemId     INT IDENTITY(1,1) PRIMARY KEY,
    ItemName   NVARCHAR(120) NOT NULL,
    [Size]     NVARCHAR(30)  NOT NULL,
    UnitInStock INT          NOT NULL,
    UnitPrice   DECIMAL(18,2) NOT NULL,
    IsActive    BIT NOT NULL DEFAULT(1)
);
CREATE TABLE dbo.[Order]
(
    OrderId    INT IDENTITY(1000,1) PRIMARY KEY,
    OrderDate  DATETIME2      NOT NULL,
    AgentId    INT            NOT NULL REFERENCES dbo.Agent(AgentId),
    CreatedBy  INT            NOT NULL REFERENCES dbo.Users(UserId),
    Notes      NVARCHAR(200)  NULL
);
CREATE TABLE dbo.OrderDetail
(
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId       INT NOT NULL REFERENCES dbo.[Order](OrderId),
    ItemId        INT NOT NULL REFERENCES dbo.Item(ItemId),
    Quantity      INT NOT NULL CHECK(Quantity > 0),
    UnitAmount    DECIMAL(18,2) NOT NULL,
    CONSTRAINT UQ_OrderDetail UNIQUE(OrderId, ItemId)
);
GO
INSERT INTO dbo.Users(UserName, Email, PasswordHash, [Lock], RoleName) VALUES
('admin', 'admin@pe2ecommerce.local', '21232f297a57a5a743894a0e4a801fc3', 0, 'Admin'),
('manager', 'manager@pe2ecommerce.local', '5e884898da28047151d0e56f8dc62927', 0, 'Manager'),
('auditor', 'auditor@pe2ecommerce.local', '96e79218965eb72c92a549dd5a330112', 0, 'Auditor'),
('agentportal', 'agent.portal@pe2ecommerce.local', '25d55ad283aa400af464c76d713c07ad', 0, 'Agent'),
('readonly', 'readonly@pe2ecommerce.local', 'e99a18c428cb38d5f260853678922e03', 1, 'Viewer');
INSERT INTO dbo.Agent(AgentName, [Address], Phone, Email) VALUES
('Sunrise Retail', '12 Nguyen Trai, District 5, HCMC', '0903111111', 'sunrise@agents.com'),
('Green Valley Foods', '45 Tran Hung Dao, District 1, HCMC', '0903222233', 'greenvalley@agents.com'),
('Lotus Supplies', '77 Hai Ba Trung, District 3, HCMC', '0903444455', 'lotus@agents.com'),
('Delta Traders', '102 Ly Thuong Kiet, District 10, HCMC', '0903666677', 'delta@agents.com'),
('Ocean Breeze', '23 Nguyen Van Cu, District 5, HCMC', '0903888899', 'ocean@agents.com'),
('Golden Mart', '4 Nguyen Dinh Chieu, District 3, HCMC', '0903001122', 'golden@agents.com'),
('Saigon Central', '55 Le Loi, District 1, HCMC', '0903333444', 'central@agents.com'),
('Metroline', '78 Nguyen Hue, District 1, HCMC', '0903555566', 'metro@agents.com'),
('Harmony Wholesale', '14 Ton Duc Thang, District 1, HCMC', '0903777788', 'harmony@agents.com'),
('Everest Retail', '99 Cach Mang Thang 8, District 1, HCMC', '0903999000', 'everest@agents.com'),
('Blue Lagoon', '22 Truong Dinh, District 3, HCMC', '0903111222', 'bluelagoon@agents.com'),
('Urban Fresh', '62 Pasteur, District 1, HCMC', '0903222333', 'urbanfresh@agents.com'),
('Galaxy Distribution', '120 Nguyen Oanh, Go Vap, HCMC', '0903444555', 'galaxy@agents.com'),
('Rivera Partners', '12 Pham Van Dong, Thu Duc, HCMC', '0903666777', 'rivera@agents.com'),
('Nova Retail', '5 Nguyen Thi Minh Khai, District 1, HCMC', '0903888999', 'nova@agents.com');
INSERT INTO dbo.Item(ItemName, [Size], UnitInStock, UnitPrice, IsActive) VALUES
('Arabica Coffee Beans', '1kg bag', 120, 18.50, 1),
('Robusta Coffee Beans', '1kg bag', 160, 14.25, 1),
('Organic Green Tea', '500g box', 140, 12.40, 1),
('Black Tea Blend', '500g box', 150, 11.10, 1),
('Cold Brew Bottle', '250ml', 300, 2.70, 1),
('Nitro Cold Brew', '330ml can', 280, 3.30, 1),
('Chai Syrup', '700ml bottle', 90, 9.90, 1),
('Caramel Syrup', '700ml bottle', 80, 9.40, 1),
('Vanilla Syrup', '700ml bottle', 85, 9.40, 1),
('Chocolate Powder', '2kg tub', 60, 21.80, 1),
('Matcha Powder', '1kg tub', 55, 32.50, 1),
('Dairy Creamer', '1L carton', 200, 3.80, 1),
('Almond Milk', '1L carton', 210, 4.20, 1),
('Paper Cups', '100pcs pack', 400, 6.50, 1),
('Stirrers', '500pcs pack', 500, 4.10, 1);
INSERT INTO dbo.[Order] (OrderDate, AgentId, CreatedBy, Notes) VALUES
('2024-01-04', 1, 1, 'Monthly restock'),
('2024-01-08', 2, 2, NULL),
('2024-01-15', 3, 1, 'Urgent request'),
('2024-01-22', 4, 2, NULL),
('2024-02-02', 5, 1, NULL),
('2024-02-05', 6, 2, 'Promo bundle'),
('2024-02-11', 7, 1, NULL),
('2024-02-14', 8, 1, 'Valentine sale'),
('2024-02-20', 9, 2, NULL),
('2024-02-25', 10, 3, NULL),
('2024-03-01', 11, 2, NULL),
('2024-03-05', 12, 1, 'Reopening stock'),
('2024-03-11', 13, 2, NULL),
('2024-03-15', 14, 3, NULL),
('2024-03-18', 15, 1, NULL),
('2024-03-22', 1, 2, 'Seasonal tasting'),
('2024-03-24', 2, 1, NULL),
('2024-03-27', 3, 2, 'Gift sets');
INSERT INTO dbo.OrderDetail(OrderId, ItemId, Quantity, UnitAmount) VALUES
(1000, 1, 20, 18.50),(1000, 5, 40, 2.70),(1000, 14, 15, 6.50),
(1001, 2, 25, 14.25),(1001, 6, 35, 3.30),(1001, 13, 30, 4.20),
(1002, 3, 18, 12.40),(1002, 7, 12, 9.90),(1002, 10, 6, 21.80),
(1003, 4, 20, 11.10),(1003, 8, 10, 9.40),(1003, 11, 5, 32.50),
(1004, 1, 15, 18.50),(1004, 9, 12, 9.40),(1004, 12, 25, 3.80),
(1005, 2, 18, 14.25),(1005, 13, 26, 4.20),(1005, 15, 30, 4.10),
(1006, 5, 50, 2.70),(1006, 6, 20, 3.30),(1006, 14, 10, 6.50),
(1007, 3, 20, 12.40),(1007, 10, 8, 21.80),(1007, 11, 6, 32.50),
(1008, 1, 12, 18.50),(1008, 7, 10, 9.90),(1008, 12, 40, 3.80),
(1009, 2, 14, 14.25),(1009, 8, 12, 9.40),(1009, 15, 28, 4.10),
(1010, 4, 16, 11.10),(1010, 9, 14, 9.40),(1010, 13, 20, 4.20),
(1011, 5, 40, 2.70),(1011, 6, 30, 3.30),(1011, 14, 20, 6.50),
(1012, 3, 18, 12.40),(1012, 7, 12, 9.90),(1012, 10, 8, 21.80),
(1013, 1, 22, 18.50),(1013, 11, 7, 32.50),(1013, 12, 30, 3.80),
(1014, 2, 20, 14.25),(1014, 8, 15, 9.40),(1014, 15, 35, 4.10),
(1015, 5, 45, 2.70),(1015, 9, 15, 9.40),(1015, 13, 25, 4.20),
(1016, 1, 18, 18.50),(1016, 7, 10, 9.90),(1016, 10, 6, 21.80),
(1017, 3, 16, 12.40),(1017, 12, 35, 3.80),(1017, 14, 12, 6.50),
(1018, 4, 18, 11.10),(1018, 6, 24, 3.30),(1018, 11, 8, 32.50);
GO
