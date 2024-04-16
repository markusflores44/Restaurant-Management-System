INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1001', 'Cheese Burger', 6.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1002', 'Beef Burger', 10.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('1003', 'Chicken Burger', 7.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2001', 'Coke', 2.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2002', 'Seven UP', 2.99);
INSERT INTO menu_item (`Item#`, `Item_Name`, `Item_Price`) VALUES ('2003', 'Soda', 2.99);

INSERT INTO `booth` (`Booth#`, `Booth_Name`) VALUES
	(1, 'Table 1'),
	(2, 'Table 2'),
	(3, 'Table 3'),
	(4, 'Table 4'),
	(5, 'Table 5');

INSERT INTO `bill` (`Bill#`) VALUES
	(1),
	(2),
	(3),
	(4),
	(5),
	(6),
	(7),
	(8),
	(9),
	(10),
	(11);

INSERT INTO `bill_items` (`Item#`, `Bill#`, `Quantity`) VALUES
	(1002, 1, 1),
	(1002, 2, 2),
	(1002, 4, 7),
	(1002, 5, 3),
	(1002, 8, 4),
	(1002, 10, 2),
	(1002, 11, 2),
	(1003, 6, 3),
	(1003, 7, 5),
	(1003, 9, 7),
	(2002, 2, 2),
	(2002, 4, 5),
	(2002, 8, 3),
	(2002, 9, 4),
	(2002, 11, 2),
	(2003, 1, 1),
	(2003, 5, 4),
	(2003, 6, 2),
	(2003, 7, 4),
	(2003, 10, 5);

INSERT INTO `booth_schedule` (`Date_Time`, `Booth#`, `Booth_Schedule_ID`) VALUES
	('2024-04-14 09:00:00', 1, 1),
	('2024-04-14 12:00:00', 5, 2),
	('2024-04-14 13:00:00', 3, 3),
	('2024-04-15 11:00:00', 4, 4);

INSERT INTO `customer` (`Customer#`, `Customer_Name`, `Phone_Number`) VALUES
	(1, 'Markus', '3685234537'),
	(2, 'Kyle', '123456789'),
	(3, 'Junction', '123456782'),
	(4, 'Mavros', '123456781');

INSERT INTO `reservation` (`Booking#`, `Booth_Schedule_ID`, `Customer#`, `Bill#`) VALUES
	(1, 1, 1, 10),
	(2, 2, 1, 7),
	(3, 3, 1, NULL),
	(4, 4, 2, 11);
