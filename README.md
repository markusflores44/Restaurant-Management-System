# Restaurant-Management-System

The FOUR GUYS Restaurant Management systems is an all purpose ordering application that is Capable of adding customers and saving them in a database. Creating menus that will load from a database and save create and display reservations from and to a database.

The general flow of the application will be if a customer calls the restaurant to place a reservation, An employee will ask if they are saved in the system, if the answer is yes the employee will proceed to the reservation page to book the reservation, if not the employee will ask for the customers name and phone number to add into the Customer Database table.

The reservation function works by loading a booth schedule from a database and only showing booths that have not been booked by another customer. When a booth is selected with a customer a new reservation is created and added to the reservation table tying in the customer # with the Booth # and date time. The Bill ID is allowed to be null and will be added later when the customer orders food at the restaurant. This will be tied together by the order now menu page after the order is placed.

The take out menu and order menu have matching functionality except for an extra function in the order menu class that will add the Bill # into Reservation table, tying the order together.
The general function of the takeout/order now is to load a picker wheel from preset menu information inside the database. This table called menu_items has a basic string name for the menu item along with a price and unique ID number. Customers will choose One main item and One drink item and select the quantity of each item to add to their order. Once the order is complete it saves the data into the Bill_items and if the order is tied to a reservation the bill # will be added into the reservation row for that particular booking.
The Order history displays all of the orders by reading the reservations and take out/ in house orders from the database and displaying them in a collection view inside the Maui app.

Error handling is used in several places to stop employees from accidently entering the wrong information or deleting a customer that has a reservation or selecting a booth without choosing a time while making a reservation and also making sure no fields are left blank before submitting.
