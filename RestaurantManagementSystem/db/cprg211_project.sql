-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 30, 2024 at 03:14 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cprg211_project`
--

-- --------------------------------------------------------

--
-- Table structure for table `bill`
--

CREATE TABLE `bill` (
  `Bill#` int(11) NOT NULL,
  `Booking#` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `bill_item`
--

CREATE TABLE `bill_item` (
  `Bill#` int(11) NOT NULL,
  `Item#` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `booth`
--

CREATE TABLE `booth` (
  `Booth#` int(11) NOT NULL,
  `Booth_Name` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `booth_schedule`
--

CREATE TABLE `booth_schedule` (
  `Date_Time` datetime NOT NULL,
  `Booth` int(11) NOT NULL,
  `Booth_Schedule_ID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `customer`
--

CREATE TABLE `customer` (
  `Customer#` int(11) NOT NULL,
  `Customer_Name` varchar(50) DEFAULT NULL,
  `Phone` varchar(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `menu_item`
--

CREATE TABLE `menu_item` (
  `Item#` int(11) NOT NULL,
  `Item_Name` varchar(50) DEFAULT NULL,
  `Price` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `reservation`
--

CREATE TABLE `reservation` (
  `Booking#` int(11) NOT NULL,
  `Booth_Schedule_ID` int(11) NOT NULL,
  `Number of Customers` int(11) NOT NULL,
  `Customer#` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bill`
--
ALTER TABLE `bill`
  ADD PRIMARY KEY (`Bill#`),
  ADD KEY `Booking#` (`Booking#`);

--
-- Indexes for table `bill_item`
--
ALTER TABLE `bill_item`
  ADD PRIMARY KEY (`Bill#`,`Item#`),
  ADD KEY `Item#` (`Item#`);

--
-- Indexes for table `booth`
--
ALTER TABLE `booth`
  ADD PRIMARY KEY (`Booth#`);

--
-- Indexes for table `booth_schedule`
--
ALTER TABLE `booth_schedule`
  ADD PRIMARY KEY (`Date_Time`,`Booth`),
  ADD UNIQUE KEY `Booth_Schedule_ID` (`Booth_Schedule_ID`),
  ADD KEY `Booth` (`Booth`);

--
-- Indexes for table `customer`
--
ALTER TABLE `customer`
  ADD PRIMARY KEY (`Customer#`);

--
-- Indexes for table `menu_item`
--
ALTER TABLE `menu_item`
  ADD PRIMARY KEY (`Item#`);

--
-- Indexes for table `reservation`
--
ALTER TABLE `reservation`
  ADD PRIMARY KEY (`Booking#`),
  ADD KEY `Booth_Schedule_ID` (`Booth_Schedule_ID`),
  ADD KEY `Customer#` (`Customer#`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bill`
--
ALTER TABLE `bill`
  MODIFY `Bill#` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `booth`
--
ALTER TABLE `booth`
  MODIFY `Booth#` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `customer`
--
ALTER TABLE `customer`
  MODIFY `Customer#` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `menu_item`
--
ALTER TABLE `menu_item`
  MODIFY `Item#` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reservation`
--
ALTER TABLE `reservation`
  MODIFY `Booking#` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bill`
--
ALTER TABLE `bill`
  ADD CONSTRAINT `bill_ibfk_1` FOREIGN KEY (`Booking#`) REFERENCES `reservation` (`Booking#`);

--
-- Constraints for table `bill_item`
--
ALTER TABLE `bill_item`
  ADD CONSTRAINT `bill_item_ibfk_1` FOREIGN KEY (`Bill#`) REFERENCES `bill` (`Bill#`),
  ADD CONSTRAINT `bill_item_ibfk_2` FOREIGN KEY (`Item#`) REFERENCES `menu_item` (`Item#`);

--
-- Constraints for table `booth_schedule`
--
ALTER TABLE `booth_schedule`
  ADD CONSTRAINT `booth_schedule_ibfk_1` FOREIGN KEY (`Booth`) REFERENCES `booth` (`Booth#`);

--
-- Constraints for table `reservation`
--
ALTER TABLE `reservation`
  ADD CONSTRAINT `reservation_ibfk_1` FOREIGN KEY (`Booth_Schedule_ID`) REFERENCES `booth_schedule` (`Booth_Schedule_ID`),
  ADD CONSTRAINT `reservation_ibfk_2` FOREIGN KEY (`Customer#`) REFERENCES `customer` (`Customer#`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
