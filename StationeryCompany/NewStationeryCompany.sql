CREATE DATABASE StationeryCompany
GO

USE StationeryCompany
GO

CREATE TABLE ProductTypes (
    TypeID INT PRIMARY KEY IDENTITY,
    TypeName VARCHAR(255)
);
GO

INSERT INTO ProductTypes (TypeName) 
VALUES 
    (N'Канцелярия'),              
    (N'Бумажная продукция'),       
    (N'Оргтехника и расходники'), 
    (N'Учебные принадлежности'),   
    (N'Рюкзаки и сумки'),        
    (N'Товары для творчества'),  
    (N'Офисная мебель'),
    (N'Игры и развлечения');

	GO

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName VARCHAR(255),
    TypeID INT,
    Quantity INT,
    Cost DECIMAL(10,2),
    FOREIGN KEY (TypeID) REFERENCES ProductTypes(TypeID)
);
GO

INSERT INTO Products (ProductName, TypeID, Quantity, Cost) 
VALUES 
    (N'Гелевая ручка', 1, 100, 20.00),
    (N'Цветная бумага для принтера', 2, 50, 30.00),
    (N'Калькулятор настольный', 3, 30, 120.00),
    (N'Глобус физический', 4, 20, 300.00),
    (N'Рюкзак школьный', 5, 40, 450.00),
    (N'Акварельные краски', 6, 55, 150.00),
    (N'Офисный стул эргономичный', 7, 15, 2000.00),
    (N'Настольная игра "Монополия"', 8, 25, 250.00),
    (N'Корректирующая жидкость', 1, 80, 35.00),
    (N'Органайзер для документов', 2, 40, 120.00),
    (N'Принтер лазерный', 3, 10, 4000.00),
    (N'Атлас по истории', 4, 30, 220.00),
    (N'Сумка для ноутбука', 5, 25, 550.00),
    (N'Мольберт для рисования', 6, 15, 700.00),
    (N'Конференц-стол', 7, 5, 5000.00),
    (N'Карточная игра "UNO"', 8, 50, 60.00),
    (N'Тетрадь в клетку', 1, 200, 15.00),
    (N'Папка-регистратор', 2, 45, 85.00),
    (N'Флешка USB 64Gb', 3, 40, 250.00),
    (N'Линейка 30 см', 1, 150, 25.00),
    (N'Блокнот А5', 2, 60, 50.00),
    (N'Мышь беспроводная', 3, 35, 320.00),
    (N'Пенал школьный', 1, 85, 120.00),
    (N'Книга для заметок', 2, 70, 80.00),
    (N'Таблетка для рисования', 6, 20, 2000.00);
	GO


CREATE TABLE SalesManagers (
    ManagerID INT PRIMARY KEY IDENTITY,
    ManagerName VARCHAR(255),
	PhoneNumber VARCHAR(20)
);
GO
--SELECT ManagerName FROM SalesManagers WHERE ManagerID = 1;
INSERT INTO SalesManagers (ManagerName, PhoneNumber) 
VALUES 
    (N'Иван Иванов', '+380 (98) 173-45-67'),
    (N'Мария Петрова', '+380 (98) 123-66-67'),
    (N'Алексей Сидоров', '+380 (44) 123-45-67'),
    (N'Елена Васильева', '+380 (96) 984-22-86'),
    (N'Николай Морозов', '+380 (44) 654-90-45');
	GO
CREATE TABLE CustomerCompanies (
    CompanyID INT PRIMARY KEY IDENTITY,
    CompanyName VARCHAR(255),
    PhoneNumber VARCHAR(20),
    City VARCHAR(255)
);
GO

INSERT INTO CustomerCompanies (CompanyName, PhoneNumber, City) 
VALUES 
    (N'ООО "Промышленные Решения"', '+380-44-123-4567', 'Киев'),
    (N'ООО "АгроКомплект"', '+380-32-234-5678', 'Львов'),
    (N'ООО "Инновации и Разработки"', '+380-57-345-6789', 'Харьков'),
    (N'ООО "Эко-Энергия"', '+380-62-456-7890', 'Донецк'),
    (N'ООО "МедТехника"', '+380-48-567-8901', 'Одесса'),
	 (N'ООО "БиоСельПрод"', '+380-44-678-9012', 'Киев'),
    (N'ООО "СтройМатериалы"', '+380-32-789-0123', 'Львов'),
    (N'ООО "ТекстильЮг"', '+380-61-890-1234', 'Запорожье'),
    (N'ООО "ФармаКиев"', '+380-44-901-2345', 'Киев'),
    (N'ООО "IT Solutions"', '+380-57-012-3456', 'Харьков'),
    (N'ООО "ЭлектроСвязь"', '+380-62-123-4567', 'Донецк'),
    (N'ООО "КреативСтрой"', '+380-48-234-5678', 'Одесса');
	GO

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
GO

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
        Products.ProductID as [ProductId],
        Products.ProductName as [ProductName],
        Products.TypeID as [TypeId],
        Products.Quantity as [Quantity],
        Products.Cost as [Cost]
    FROM Products
    INNER JOIN ProductTypes ON Products.TypeID = ProductTypes.TypeID;
END;
GO
--
CREATE PROCEDURE ShowAllProductTypes AS
BEGIN
    SELECT 
        TypeID as [ID типа],
        TypeName as [Название типа]
    FROM ProductTypes;
END;
GO
--
CREATE PROCEDURE ShowAllSalesManagers AS
BEGIN
    SELECT 
        ManagerID as [ID менеджера],
        ManagerName as [Имя менеджера],
        PhoneNumber as [Телефонный номер]
    FROM SalesManagers;
END;
GO
--
CREATE PROCEDURE ShowProductsWithMaxQuantity AS
BEGIN
    SELECT 
    P.ProductID as ProductId,
    P.ProductName as ProductName,
    P.TypeID as TypeId,
    P.Quantity as Quantity,
    P.Cost as Cost
    FROM Products p 
    WHERE P.Quantity = (SELECT MAX(Quantity) FROM Products);
END;
GO

--DROP PROCEDURE ShowProductsWithMaxQuantity
--
CREATE PROCEDURE ShowProductsWithMinQuantity AS
BEGIN
    SELECT 
    P.ProductID as ProductId,
    P.ProductName as ProductName,
    P.TypeID as TypeId,
    P.Quantity as Quantity,
    P.Cost as Cost
    FROM Products p 
    WHERE Quantity = (SELECT MIN(Quantity) FROM Products);
END;
GO
--1
CREATE PROCEDURE ShowProductsWithMinCost AS
BEGIN
    SELECT 
    P.ProductID as ProductId,
    P.ProductName as ProductName,
    P.TypeID as TypeId,
    P.Quantity as Quantity,
    P.Cost as Cost
    FROM Products p 
    WHERE Cost = (SELECT MIN(Cost) FROM Products);
END;
GO
--
CREATE PROCEDURE ShowProductsWithMaxCost AS
BEGIN
    SELECT 
    P.ProductID as ProductId,
    P.ProductName as ProductName,
    P.TypeID as TypeId,
    P.Quantity as Quantity,
    P.Cost as Cost
FROM Products P
WHERE P.Cost = (SELECT MAX(Cost) FROM Products);
END;
GO

--EXEC ShowProductsWithMaxCost
--
CREATE PROCEDURE ShowProductsByType
    @TypeName NVARCHAR(255)
AS
BEGIN
    SELECT 
    P.ProductID as ProductId,
    P.ProductName as ProductName,
    P.TypeID as TypeId,
    P.Quantity as Quantity,
    P.Cost as Cost
FROM Products P
    INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    WHERE PT.TypeName = @TypeName;
END;
GO

CREATE PROCEDURE ShowProductsSoldByManager
    @ManagerName NVARCHAR(255)
AS
BEGIN
    SELECT 
        P.ProductID as ProductId,
        P.ProductName as ProductName,
        SM.ManagerName as ManagerName,
        S.QuantitySold as QuantitySold,
        S.PricePerUnit as PricePerUnit
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN SalesManagers SM ON S.ManagerID = SM.ManagerID
    WHERE SM.ManagerName = @ManagerName;
END;
GO

CREATE PROCEDURE ShowProductsBoughtByCompany
    @CompanyName NVARCHAR(255)
AS
BEGIN
    SELECT 
        P.ProductID as ProductId,
        P.ProductName as ProductName,
        CC.CompanyName as CompanyName,
        S.QuantitySold as QuantitySold,
        S.PricePerUnit as PricePerUnit
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN CustomerCompanies CC ON S.CompanyID = CC.CompanyID
    WHERE CC.CompanyName = @CompanyName;
END;
GO


CREATE PROCEDURE ShowLatestSale
AS
BEGIN
    SELECT TOP 1
        S.ProductID,
        P.ProductName,
        SM.ManagerID,
        CC.CompanyID,
        S.QuantitySold,
        S.PricePerUnit,
        S.SaleDate
    FROM Sales S
    INNER JOIN Products P ON S.ProductID = P.ProductID
    INNER JOIN SalesManagers SM ON S.ManagerID = SM.ManagerID
    INNER JOIN CustomerCompanies CC ON S.CompanyID = CC.CompanyID
    ORDER BY S.SaleDate DESC;
END;
GO

--DROP PROCEDURE ShowLatestSale

CREATE PROCEDURE ShowAverageQuantityByProductType
AS
BEGIN
    SELECT 
        PT.TypeName as [Тип продукта],
        AVG(P.Quantity) as [Среднее количество]
    FROM Products P
    INNER JOIN ProductTypes PT ON P.TypeID = PT.TypeID
    GROUP BY PT.TypeName;
END;
GO

--Часть 2 

CREATE PROCEDURE ShowTopManager AS
BEGIN
    SELECT TOP 1
        SM.ManagerName as [Имя менеджера],
        SUM(S.QuantitySold) as [Общее количество продаж]
    FROM SalesManagers SM
    JOIN Sales S ON SM.ManagerID = S.ManagerID
    GROUP BY SM.ManagerName
    ORDER BY SUM(S.QuantitySold) DESC;
END;
GO

CREATE PROCEDURE ShowTopByProfit AS
BEGIN
    SELECT TOP 1
        SM.ManagerName as [Имя менеджера],
        SUM(S.PricePerUnit * S.QuantitySold) as [Общая прибыль]
    FROM SalesManagers SM
    JOIN Sales S ON SM.ManagerID = S.ManagerID
    GROUP BY SM.ManagerName
    ORDER BY SUM(S.PricePerUnit * S.QuantitySold) DESC;
END;
GO

CREATE PROCEDURE ShowTopCustomer AS
BEGIN
    SELECT 
        CC.CompanyName as [Название компании],
        SUM(S.PricePerUnit * S.QuantitySold) as [Общая сумма покупок]
    FROM CustomerCompanies CC
    JOIN Sales S ON CC.CompanyID = S.CompanyID
    GROUP BY CC.CompanyName
    ORDER BY SUM(S.PricePerUnit * S.QuantitySold) DESC;
END;

GO
--

CREATE PROCEDURE ShowTopProductType AS
BEGIN
    SELECT 
        PT.TypeName as [Тип продукта],
        SUM(S.QuantitySold) as [Общее количество продаж]
    FROM ProductTypes PT
    JOIN Products P ON PT.TypeID = P.TypeID
    JOIN Sales S ON P.ProductID = S.ProductID
    GROUP BY PT.TypeName
    ORDER BY SUM(S.QuantitySold) DESC;
END;
GO

CREATE PROCEDURE ShowTopProfitableProductType AS
BEGIN
    SELECT 
        PT.TypeName as [Тип продукта],
        SUM(S.QuantitySold * S.PricePerUnit) as [Общая прибыль]
    FROM ProductTypes PT
    JOIN Products P ON PT.TypeID = P.TypeID
    JOIN Sales S ON P.ProductID = S.ProductID
    GROUP BY PT.TypeName
    ORDER BY SUM(S.QuantitySold * S.PricePerUnit) DESC;
END;
GO

CREATE PROCEDURE ShowMostPopularProducts AS
BEGIN
    SELECT 
        P.ProductName as [Название продукта],
        SUM(S.QuantitySold) as [Количество проданных единиц]
    FROM Products P
    JOIN Sales S ON P.ProductID = S.ProductID
    GROUP BY P.ProductName
    ORDER BY SUM(S.QuantitySold) DESC;
END;
GO

CREATE PROCEDURE ShowUnsoldProducts
    @DaysNotSold INT
AS
BEGIN
    SELECT 
        P.ProductName as [Название продукта],
        ISNULL(DATEDIFF(DAY, MAX(S.SaleDate), GETDATE()), @DaysNotSold) AS [Дней не продано]
    FROM Products P
    LEFT JOIN Sales S ON S.ProductID = P.ProductID
    GROUP BY P.ProductID, P.ProductName
    HAVING MAX(S.SaleDate) IS NULL OR MAX(S.SaleDate) < DATEADD(DAY, -@DaysNotSold, GETDATE())
    ORDER BY P.ProductName;
END;
GO


CREATE PROCEDURE ShowTopSellingManagerByProfitInPeriod
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT TOP 1
        SM.ManagerName AS [Имя менеджера],
        SUM(S.QuantitySold * S.PricePerUnit) AS [Общая выручка]
    FROM SalesManagers SM
    JOIN Sales S ON SM.ManagerID = S.ManagerID
    WHERE S.SaleDate BETWEEN @StartDate AND @EndDate
    GROUP BY SM.ManagerName
    ORDER BY [Общая выручка] DESC;
END;
GO

CREATE PROCEDURE ShowAllCompaniesWithOrderCount
AS
BEGIN
    SELECT 
	    CC.CompanyID AS [ID сомпании],
        CC.CompanyName AS [Название компании],
        CC.PhoneNumber AS [Номер телефона],
		CC.City AS [Город],
        COUNT(S.SaleID) AS [Количество заказов]
    FROM CustomerCompanies CC
    LEFT JOIN Sales S ON CC.CompanyID = S.CompanyID
    GROUP BY CC.CompanyID, CC.CompanyName, CC.PhoneNumber, CC.City
    ORDER BY CC.CompanyName;
END;
GO

CREATE PROCEDURE DeleteProduct
   @ProductID INT
AS
BEGIN
    -- Нужно для отката транзакции в случае ошибки
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM Sales WHERE ProductID = @ProductID;
        DELETE FROM Products WHERE ProductID = @ProductID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO


CREATE PROCEDURE DeleteProductTypeAndRelatedProducts
    @TypeID INT
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM Sales
        WHERE ProductID IN (SELECT ProductID FROM Products WHERE TypeID = @TypeID);

        DELETE FROM Products WHERE TypeID = @TypeID;

        DELETE FROM ProductTypes WHERE TypeID = @TypeID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE DeleteSalesManagerAndRelatedSales
    @ManagerID INT
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
   BEGIN TRY
        DELETE FROM Sales WHERE ManagerID = @ManagerID;
        DELETE FROM SalesManagers WHERE ManagerID = @ManagerID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO
CREATE PROCEDURE DeleteSalesCompaniesAndRelatedSales
    @CompaniesID INT
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
   BEGIN TRY
        DELETE FROM Sales WHERE CompanyID = @CompaniesID;
        DELETE FROM CustomerCompanies WHERE CompanyID = @CompaniesID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO

CREATE PROCEDURE GetAllSalesDetails
AS
BEGIN
    SELECT
        s.SaleID AS [ID продажи],
        p.ProductName AS [Название продукта],
        sm.ManagerName AS [Имя менеджера],
        cc.CompanyName AS [Название компании],
        s.QuantitySold AS [Количество продано],
        s.PricePerUnit AS [Цена за единицу],
        s.SaleDate AS [Дата продажи]
    FROM Sales s
    JOIN Products p ON s.ProductID = p.ProductID
    JOIN SalesManagers sm ON s.ManagerID = sm.ManagerID
    JOIN CustomerCompanies cc ON s.CompanyID = cc.CompanyID
    ORDER BY s.SaleDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE UpdateProductTypeName
    @TypeID INT,
    @NewTypeName VARCHAR(255)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM ProductTypes WHERE TypeID = @TypeID)
    BEGIN
        UPDATE ProductTypes
        SET TypeName = @NewTypeName
        WHERE TypeID = @TypeID;
    END
END;
GO

CREATE PROCEDURE GetProductTypeById
    @TypeID INT
AS
BEGIN
    SELECT TypeName
    FROM ProductTypes
    WHERE TypeID = @TypeID;
END;
GO

CREATE PROCEDURE GetManagerById
    @ManagerID  INT
AS
BEGIN
    SELECT ManagerName, PhoneNumber 
    FROM SalesManagers 
    WHERE ManagerID  = @ManagerID ;
END;
GO

CREATE PROCEDURE UpdateSalesManager
    @ManagerID INT,
    @ManagerName VARCHAR(255),
    @PhoneNumber VARCHAR(20)
AS
BEGIN
    UPDATE SalesManagers
    SET ManagerName = @ManagerName, PhoneNumber = @PhoneNumber
    WHERE ManagerID = @ManagerID;
END;
GO

CREATE PROCEDURE UpdateCustomerCompany
    @CompanyID INT,
    @CompanyName VARCHAR(255),
    @PhoneNumber VARCHAR(20),
    @City VARCHAR(255)
AS
BEGIN
    UPDATE CustomerCompanies
    SET CompanyName = @CompanyName,
        PhoneNumber = @PhoneNumber,
        City = @City
    WHERE CompanyID = @CompanyID;
END;
GO

CREATE PROCEDURE GetCompaniesById
    @CompanyID   INT
AS
BEGIN
    SELECT CompanyName, PhoneNumber, City 
	FROM CustomerCompanies  
    WHERE CompanyID  = @CompanyID  ;
END;
GO

CREATE PROCEDURE GetProductById
    @ProductID   INT
AS
BEGIN
    SELECT ProductName, TypeID, Quantity, Cost 
	FROM Products   
    WHERE ProductID  = @ProductID  ;
END;
GO

CREATE PROCEDURE UpdateProduct
    @ProductID INT,
    @ProductName VARCHAR(255),
    @TypeID INT,
    @Quantity INT,
    @Cost DECIMAL(10,2)
AS
BEGIN
    UPDATE Products
    SET ProductName = @ProductName,
        TypeID = @TypeID,
        Quantity = @Quantity,
        Cost = @Cost
    WHERE ProductID = @ProductID;
END;
GO

CREATE PROCEDURE AddSalesManager
    @ManagerName VARCHAR(255),
    @PhoneNumber VARCHAR(20)
AS
BEGIN
    INSERT INTO SalesManagers (ManagerName, PhoneNumber)
    VALUES (@ManagerName, @PhoneNumber);
END;
GO

CREATE PROCEDURE AddCustomerCompany
    @CompanyName VARCHAR(255),
    @PhoneNumber VARCHAR(20),
    @City VARCHAR(255)
AS
BEGIN
    INSERT INTO CustomerCompanies (CompanyName, PhoneNumber, City)
    VALUES (@CompanyName, @PhoneNumber, @City);
END;
GO

CREATE PROCEDURE AddProductType
    @TypeName VARCHAR(255)
AS
BEGIN
    INSERT INTO ProductTypes (TypeName)
    VALUES (@TypeName);
END;
GO

CREATE PROCEDURE AddProduct
    @ProductName VARCHAR(255),
    @TypeID INT,
    @Quantity INT,
    @Cost DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Products (ProductName, TypeID, Quantity, Cost)
    VALUES (@ProductName, @TypeID, @Quantity, @Cost);
END;
GO
