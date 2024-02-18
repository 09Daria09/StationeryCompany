CREATE DATABASE StationeryCompany
USE StationeryCompany

CREATE TABLE ProductTypes (
    TypeID INT PRIMARY KEY IDENTITY,
    TypeName VARCHAR(255)
);

INSERT INTO ProductTypes (TypeName) 
VALUES 
    (N'����������'),              
    (N'�������� ���������'),       
    (N'���������� � ����������'), 
    (N'������� ��������������'),   
    (N'������� � �����'),        
    (N'������ ��� ����������'),  
    (N'������� ������'),
    (N'���� � �����������');

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName VARCHAR(255),
    TypeID INT,
    Quantity INT,
    Cost DECIMAL(10,2),
    FOREIGN KEY (TypeID) REFERENCES ProductTypes(TypeID)
);
INSERT INTO Products (ProductName, TypeID, Quantity, Cost) 
VALUES 
    (N'������� �����', 1, 100, 20.00),
    (N'������� ������ ��� ��������', 2, 50, 30.00),
    (N'����������� ����������', 3, 30, 120.00),
    (N'������ ����������', 4, 20, 300.00),
    (N'������ ��������', 5, 40, 450.00),
    (N'����������� ������', 6, 55, 150.00),
    (N'������� ���� ������������', 7, 15, 2000.00),
    (N'���������� ���� "���������"', 8, 25, 250.00),
    (N'�������������� ��������', 1, 80, 35.00),
    (N'���������� ��� ����������', 2, 40, 120.00),
    (N'������� ��������', 3, 10, 4000.00),
    (N'����� �� �������', 4, 30, 220.00),
    (N'����� ��� ��������', 5, 25, 550.00),
    (N'�������� ��� ���������', 6, 15, 700.00),
    (N'���������-����', 7, 5, 5000.00),
    (N'��������� ���� "UNO"', 8, 50, 60.00),
    (N'������� � ������', 1, 200, 15.00),
    (N'�����-�����������', 2, 45, 85.00),
    (N'������ USB 64Gb', 3, 40, 250.00),
    (N'������� 30 ��', 1, 150, 25.00),
    (N'������� �5', 2, 60, 50.00),
    (N'���� ������������', 3, 35, 320.00),
    (N'����� ��������', 1, 85, 120.00),
    (N'����� ��� �������', 2, 70, 80.00),
    (N'�������� ��� ���������', 6, 20, 2000.00);


CREATE TABLE SalesManagers (
    ManagerID INT PRIMARY KEY IDENTITY,
    ManagerName VARCHAR(255),
	PhoneNumber VARCHAR(20)
);
INSERT INTO SalesManagers (ManagerName, PhoneNumber) 
VALUES 
    (N'���� ������', '+380 (98) 173-45-67'),
    (N'����� �������', '+380 (98) 123-66-67'),
    (N'������� �������', '+380 (44) 123-45-67'),
    (N'����� ���������', '+380 (96) 984-22-86'),
    (N'������� �������', '+380 (44) 654-90-45');
	

CREATE TABLE CustomerCompanies (
    CompanyID INT PRIMARY KEY IDENTITY,
    CompanyName VARCHAR(255),
    PhoneNumber VARCHAR(20),
    City VARCHAR(255)
);

INSERT INTO CustomerCompanies (CompanyName, PhoneNumber, City) 
VALUES 
    (N'��� "������������ �������"', '+380-44-123-4567', '����'),
    (N'��� "������������"', '+380-32-234-5678', '�����'),
    (N'��� "��������� � ����������"', '+380-57-345-6789', '�������'),
    (N'��� "���-�������"', '+380-62-456-7890', '������'),
    (N'��� "����������"', '+380-48-567-8901', '������'),
	 (N'��� "�����������"', '+380-44-678-9012', '����'),
    (N'��� "��������������"', '+380-32-789-0123', '�����'),
    (N'��� "����������"', '+380-61-890-1234', '���������'),
    (N'��� "���������"', '+380-44-901-2345', '����'),
    (N'��� "IT Solutions"', '+380-57-012-3456', '�������'),
    (N'��� "������������"', '+380-62-123-4567', '������'),
    (N'��� "������������"', '+380-48-234-5678', '������');

CREATE TABLE Sales (
    SaleID INT PRIMARY KEY IDENTITY,
    ProductID INT,
    ManagerID INT,
    CompanyID INT,
    QuantitySold INT,
    PricePerUnit DECIMAL(10,2),
    SaleDate DATE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (ManagerID) REFERENCES SalesManagers(ManagerID),
    FOREIGN KEY (CompanyID) REFERENCES CustomerCompanies(CompanyID)
);

INSERT INTO Sales (ProductID, ManagerID, CompanyID, QuantitySold, PricePerUnit, SaleDate) 
VALUES 
    (1, 1, 1, 100, 20.00, '2023-01-10'),
    (2, 2, 2, 150, 15.00, '2023-01-15'),
    (3, 3, 3, 200, 25.00, '2023-02-01'),
    (4, 4, 4, 250, 30.00, '2023-02-05'),
    (5, 5, 5, 300, 35.00, '2023-02-10'),
    (1, 2, 3, 120, 22.00, '2023-02-15'),
    (2, 3, 4, 130, 18.00, '2023-02-20'),
    (3, 4, 5, 110, 21.00, '2023-02-25'),
    (4, 5, 1, 100, 19.00, '2023-03-01'),
    (5, 1, 2, 90, 23.00, '2023-03-05'),
    (1, 3, 4, 80, 24.00, '2023-03-10'),
    (2, 4, 5, 70, 17.00, '2023-03-15'),
    (3, 5, 1, 160, 26.00, '2023-03-20'),
    (4, 1, 2, 170, 27.00, '2023-03-25'),
    (5, 2, 3, 180, 28.00, '2023-04-01'),
    (1, 3, 4, 190, 29.00, '2023-04-05'),
    (2, 4, 5, 200, 30.00, '2023-04-10'),
    (3, 5, 1, 210, 31.00, '2023-04-15'),
    (4, 1, 2, 220, 32.00, '2023-04-20'),
    (5, 2, 3, 230, 33.00, '2023-04-25'),
    (1, 3, 4, 240, 34.00, '2023-05-01'),
    (2, 4, 5, 250, 35.00, '2023-05-05'),
    (3, 5, 1, 260, 36.00, '2023-05-10'),
    (4, 1, 2, 270, 37.00, '2023-05-15'),
    (5, 2, 3, 280, 38.00, '2023-05-20'),
    (24, 1, 12, 100, 20.00, '2023-01-10'),
    (23, 2, 11, 150, 15.00, '2023-01-15'),
    (22, 3, 10, 200, 25.00, '2023-02-01'),
    (21, 4, 9, 250, 30.00, '2023-02-05'),
    (20, 5, 8, 300, 35.00, '2023-02-10'),
    (19, 2, 7, 120, 22.00, '2023-02-15'),
    (18, 3, 6, 130, 18.00, '2023-02-20'),
    (17, 4, 5, 110, 21.00, '2023-02-25'),
    (16, 5, 4, 100, 19.00, '2023-03-01'),
    (15, 1, 3, 90, 23.00, '2023-03-05'),
    (14, 3, 2, 80, 24.00, '2023-03-10'),
    (13, 4, 1, 70, 17.00, '2023-03-15'),
    (12, 5, 12, 160, 26.00, '2023-03-20'),
    (11, 1, 11, 170, 27.00, '2023-03-25'),
    (10, 2, 10, 180, 28.00, '2023-04-01'),
    (9, 3, 10, 190, 29.00, '2023-04-05'),
    (8, 4, 9, 200, 30.00, '2023-04-10'),
    (7, 5, 8, 210, 31.00, '2023-04-15'),
    (6, 1, 7, 220, 32.00, '2023-04-20'),
    (5, 2, 6, 230, 33.00, '2023-04-25'),
    (4, 3, 5, 240, 34.00, '2023-05-01'),
    (3, 4, 4, 250, 35.00, '2023-05-05'),
    (2, 5, 3, 260, 36.00, '2023-05-10'),
    (1, 1, 2, 270, 37.00, '2023-05-15'),
    (5, 2, 1, 280, 38.00, '2023-05-20'),
    (1, 2, 7, 100, 20.00, '2023-01-10'),
    (2, 2, 7, 150, 15.00, '2023-01-15'),
    (3, 2, 7, 200, 25.00, '2023-02-01'),
    (4, 1, 7, 250, 30.00, '2023-02-05'),
    (5, 1, 7, 300, 35.00, '2023-02-10'),
    (1, 1, 8, 120, 22.00, '2023-02-15'),
    (2, 2, 8, 130, 18.00, '2023-02-20'),
    (3, 3, 8, 110, 21.00, '2023-02-25'),
    (4, 3, 8, 100, 19.00, '2023-03-01'),
    (5, 1, 8, 90, 23.00, '2023-03-05'),
    (1, 1, 10, 80, 24.00, '2023-03-10'),
    (2, 1, 11, 70, 17.00, '2023-03-15'),
    (3, 1, 10, 160, 26.00, '2023-03-20'),
    (4, 2, 11, 170, 27.00, '2023-03-25'),
    (5, 2, 4, 180, 28.00, '2023-04-01'),
    (1, 2, 4, 190, 29.00, '2023-04-05'),
    (2, 2, 1, 200, 30.00, '2023-04-10'),
    (3, 3, 1, 210, 31.00, '2023-04-15'),
    (4, 3, 1, 220, 32.00, '2023-04-20'),
    (5, 3, 3, 230, 33.00, '2023-04-25'),
    (1, 3, 4, 240, 34.00, '2023-05-01'),
    (2, 3, 5, 250, 35.00, '2023-05-05');
	GO

CREATE PROCEDURE ShowAllProducts AS
BEGIN
    SELECT 
        Products.ProductID as [ID ��������],
        Products.ProductName as [��������],
        ProductTypes.TypeName as [���],
        Products.Quantity as [����������],
        Products.Cost as [���������]
    FROM Products
    INNER JOIN ProductTypes ON Products.TypeID = ProductTypes.TypeID;
END;
--
CREATE PROCEDURE ShowAllProductTypes AS
BEGIN
    SELECT 
        TypeID as [ID ����],
        TypeName as [�������� ����]
    FROM ProductTypes;
END;
--
CREATE PROCEDURE ShowAllSalesManagers AS
BEGIN
    SELECT 
        ManagerID as [ID ���������],
        ManagerName as [��� ���������],
        PhoneNumber as [���������� �����]
    FROM SalesManagers;
END;

--
CREATE PROCEDURE ShowProductsWithMaxQuantity AS
BEGIN
    SELECT 
        P.ProductID as [ID ��������],
        P.ProductName as [��������],
        PT.TypeName as [���],
        P.Quantity as [����������],
        P.Cost as [���������]
    FROM Products P
    INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    WHERE P.Quantity = (SELECT MAX(Quantity) FROM Products);
END;

--
CREATE PROCEDURE ShowProductsWithMinQuantity AS
BEGIN
    SELECT 
        P.ProductID as [ID ��������],
        P.ProductName as [��������],
        PT.TypeName as [���],
        P.Quantity as [����������],
        P.Cost as [���������]
    FROM Products P
	INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    WHERE Quantity = (SELECT MIN(Quantity) FROM Products);
END;
EXEC ShowProductsWithMinQuantity;
--1
CREATE PROCEDURE ShowProductsWithMinCost AS
BEGIN
    SELECT 
        p.ProductID as [ID],
        p.ProductName as [��������],
        PT.TypeName as [���],
        p.Quantity as [����������],
        p.Cost as [���������]
    FROM Products p 
	INNER JOIN ProductTypes PT ON p.TypeID = PT.TypeID
    WHERE Cost = (SELECT MIN(Cost) FROM Products);
END;
EXEC ShowProductsWithMinCost;
--
CREATE PROCEDURE ShowProductsWithMaxCost AS
BEGIN
    SELECT 
        P.ProductID as [������������� ��������],
        P.ProductName as [�������� ��������],
        PT.TypeID as [ID ����],
        P.Quantity as [����������],
        P.Cost as [���������]
    FROM Products P
	INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    WHERE Cost = (SELECT MAX(Cost) FROM Products);
END;
EXEC ShowProductsWithMaxCost;
--
CREATE PROCEDURE ShowProductsByType
    @TypeName NVARCHAR(255)
AS
BEGIN
    SELECT 
        P.ProductID as [ID ��������],
        P.ProductName as [��������],
        PT.TypeName as [���],
        P.Quantity as [����������],
        P.Cost as [���������]
    FROM Products P
    INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    WHERE PT.TypeName = @TypeName;
END;
EXEC ShowProductsByType @TypeName = N'����������';

CREATE PROCEDURE ShowProductsSoldByManager
    @ManagerName NVARCHAR(255)
AS
BEGIN
    SELECT 
        P.ProductID as [ID ��������],
        P.ProductName as [��������],
        SM.ManagerName as [��� ���������],
        S.QuantitySold as [��������� ����������],
        S.PricePerUnit as [���� �� �������]
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN SalesManagers SM ON S.ManagerID = SM.ManagerID
    WHERE SM.ManagerName = @ManagerName;
END;
EXEC ShowProductsSoldByManager @ManagerName = N'���� ������';




CREATE PROCEDURE ShowProductsBoughtByCompany
    @CompanyName NVARCHAR(255)
AS
BEGIN
    SELECT 
        P.ProductID as [ID ��������],
        P.ProductName as [�������� ��������],
        CC.CompanyName as [�������� ��������],
        S.QuantitySold as [��������� ����������],
        S.PricePerUnit as [���� �� �������]
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN CustomerCompanies CC ON S.CompanyID = CC.CompanyID
    WHERE CC.CompanyName = @CompanyName;
END;

EXEC ShowProductsBoughtByCompany @CompanyName = N'��� "������������"';


CREATE PROCEDURE ShowLatestSale
AS
BEGIN
    SELECT TOP 1
        P.ProductName as [��������],
        SM.ManagerName as [��� ���������],
        CC.CompanyName as [�������� ��������],
        S.QuantitySold as [����������],
        S.PricePerUnit as [���� �� �������],
        S.SaleDate as [���� �������]
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN SalesManagers SM ON S.ManagerID = SM.ManagerID
    INNER JOIN CustomerCompanies CC ON S.CompanyID = CC.CompanyID
    ORDER BY S.SaleDate DESC;
END;

EXEC ShowLatestSale;

CREATE PROCEDURE ShowAverageQuantityByProductType
AS
BEGIN
    SELECT 
        PT.TypeName as [��� ��������],
        AVG(P.Quantity) as [������� ����������]
    FROM Products P
    INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    GROUP BY PT.TypeName;
END;

EXEC ShowAverageQuantityByProductType;





