# Info-System
# CONFIGUTING THE DATABASE
sudo mysql_secure_installation

mysql -u root -p
SELECT user, host, plugin FROM mysql.user WHERE user = 'root';

CREATE DATABASE InformationDatabase;

CREATE USER 'Information' @ 'localhost' IDENTIFIED BY 'Info@123##';

GRANT ALL PRIVILEGES ON InformationDatabase.* TO 'Information'@'localhost';

FLUSH PRIVILEGES;

mysql -u Information -p

USE InformationDatabase;

Create the stored procedure
-- Change the delimiter to //
DELIMITER //

CREATE PROCEDURE AddProduct (
    IN p_ProductName VARCHAR(255),
    IN p_Category VARCHAR(255),
    IN p_Price DECIMAL(18, 2),
    IN p_StockQuantity INT,
    IN p_Supplier VARCHAR(255),
    IN p_Description TEXT
)
BEGIN
    INSERT INTO Products (ProductName, Category, Price, StockQuantity, Supplier, Description)
    VALUES (p_ProductName, p_Category, p_Price, p_StockQuantity, p_Supplier, p_Description);
END //

DELIMITER ;

Create the Tables;
CREATE TABLE Products (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Category VARCHAR(255),
    Price DECIMAL(18, 2),
    StockQuantity INT,
    Supplier VARCHAR(255),
    Description TEXT
);
do installations
dotnet add package System.Data.SqlClient
# This procedure will retrieve products:
DELIMITER //

CREATE PROCEDURE GetProducts()
BEGIN
    SELECT * FROM Products;
END //

DELIMITER ;
# retrieves the product by their id
DELIMITER //

CREATE PROCEDURE GetProductById(IN p_Id INT)
BEGIN
    SELECT 
        Id,
        ProductName,
        Category,
        Price,
        StockQuantity,
        Supplier,
        Description
    FROM 
        Products
    WHERE 
        Id = p_Id;
END //

DELIMITER ;
# add product stored procedure
DELIMITER //

-- Drop the existing AddProduct procedure if it exists
DROP PROCEDURE IF EXISTS AddProduct;

-- Create the PostProduct procedure
CREATE PROCEDURE PostProduct(
    IN p_ProductName VARCHAR(255),
    IN p_Category VARCHAR(255),
    IN p_Price DECIMAL(10,2),
    IN p_StockQuantity INT,
    IN p_Supplier VARCHAR(255),
    IN p_Description TEXT
)
BEGIN
    INSERT INTO Products (ProductName, Category, Price, StockQuantity, Supplier, Description)
    VALUES (p_ProductName, p_Category, p_Price, p_StockQuantity, p_Supplier, p_Description);
END //

DELIMITER ;
# detele procedure
DELIMITER //

-- Drop the existing DeleteProduct procedure if it exists
DROP PROCEDURE IF EXISTS DeleteProduct;

-- Create the DeleteProduct procedure
CREATE PROCEDURE DeleteProduct(
    IN p_Id INT
)
BEGIN
    DELETE FROM Products
    WHERE Id = p_Id;
END //

DELIMITER ;
# PARTIALLY UPDATE SP
DELIMITER //

CREATE PROCEDURE UpdateProduct(
    IN p_Id INT,
    IN p_ProductName VARCHAR(255),
    IN p_Category VARCHAR(255),
    IN p_Price DECIMAL(10,2),
    IN p_StockQuantity INT,
    IN p_Supplier VARCHAR(255),
    IN p_Description TEXT
)
BEGIN
    UPDATE Products
    SET 
        ProductName = COALESCE(p_ProductName, ProductName),
        Category = COALESCE(p_Category, Category),
        Price = p_Price,
        StockQuantity = p_StockQuantity,
        Supplier = COALESCE(p_Supplier, Supplier),
        Description = COALESCE(p_Description, Description)
    WHERE Id = p_Id;
END //

DELIMITER ;
# The Math.ceil function rounds up to the nearest integer.