-- MySQL Script generated by MySQL Workbench
-- Sat Mar 30 19:26:41 2024
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `mydb` ;

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Menu_Item`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Menu_Item` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Menu_Item` (
  `Item#` INT NOT NULL AUTO_INCREMENT,
  `Item_Name` VARCHAR(50) NOT NULL,
  `Item_Price` DOUBLE NOT NULL,
  PRIMARY KEY (`Item#`),
  UNIQUE INDEX `Item_Name_UNIQUE` (`Item_Name` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Customer`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Customer` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Customer` (
  `Customer#` INT NOT NULL AUTO_INCREMENT,
  `Customer_Name` VARCHAR(45) NOT NULL,
  `Phone_Number` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`Customer#`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Booth`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Booth` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Booth` (
  `Booth#` INT NOT NULL AUTO_INCREMENT,
  `Booth_Name` VARCHAR(15) NOT NULL,
  PRIMARY KEY (`Booth#`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Booth_Schedule`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Booth_Schedule` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Booth_Schedule` (
  `Date_Time` DATETIME NOT NULL,
  `Booth_Booth#` INT NOT NULL,
  `Booth_Schedule_ID` INT NULL,
  INDEX `fk_Booth_Schedule_Booth_idx` (`Booth_Booth#` ASC) VISIBLE,
  PRIMARY KEY (`Booth_Booth#`, `Date_Time`),
  UNIQUE INDEX `Booth_Schedule_ID_UNIQUE` (`Booth_Schedule_ID` ASC) VISIBLE,
  CONSTRAINT `fk_Booth_Schedule_Booth`
    FOREIGN KEY (`Booth_Booth#`)
    REFERENCES `mydb`.`Booth` (`Booth#`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Bill`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Bill` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Bill` (
  `Bill#` INT NOT NULL AUTO_INCREMENT,
  `Total_Price` DOUBLE,
  PRIMARY KEY (`Bill#`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Bill_Items`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Bill_Items` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Bill_Items` (
  `Item#` INT NOT NULL,
  `Bill#` INT NOT NULL,
  `Quantity` INT NULL,
  INDEX `fk_Bill_Items_Menu_Item1_idx` (`Item#` ASC) VISIBLE,
  INDEX `fk_Bill_Items_Bill1_idx` (`Bill#` ASC) VISIBLE,
  PRIMARY KEY (`Item#`, `Bill#`),
  CONSTRAINT `fk_Bill_Items_Menu_Item1`
    FOREIGN KEY (`Item#`)
    REFERENCES `mydb`.`Menu_Item` (`Item#`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Bill_Items_Bill1`
    FOREIGN KEY (`Bill#`)
    REFERENCES `mydb`.`Bill` (`Bill#`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Reservation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Reservation` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Reservation` (
  `Booking#` INT NOT NULL AUTO_INCREMENT,
  `Booth_Schedule_ID` INT NULL,
  `Customer#` INT NOT NULL,
  `Bill#` INT NOT NULL,
  `Number_of_Customers` INT NULL,
  PRIMARY KEY (`Booking#`),
  INDEX `Booth_Schedule_ID_idx` (`Booth_Schedule_ID` ASC) VISIBLE,
  INDEX `fk_Reservation_Customer1_idx` (`Customer#` ASC) VISIBLE,
  INDEX `fk_Reservation_Bill1_idx` (`Bill#` ASC) VISIBLE,
  CONSTRAINT `Booth_Schedule_ID`
    FOREIGN KEY (`Booth_Schedule_ID`)
    REFERENCES `mydb`.`Booth_Schedule` (`Booth_Schedule_ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Reservation_Customer1`
    FOREIGN KEY (`Customer#`)
    REFERENCES `mydb`.`Customer` (`Customer#`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Reservation_Bill1`
    FOREIGN KEY (`Bill#`)
    REFERENCES `mydb`.`Bill` (`Bill#`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1,001', 'Cheese Burger', 6.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1,002', 'Beef Burger', 10.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1,003', 'Chicken Burger', 7.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2,001', 'Coke', 2.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2,002', 'Seven UP', 2.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2,003', 'Soda', 2.99);


