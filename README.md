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
