using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    // this class holds all methods for retrieving and deleting Database information
    public class DatabaseAccess
    {
        public MySqlConnectionStringBuilder BuilderString { get; set; } //Blank string which we pass the info to connect to the database to.



        public DatabaseAccess()
        {

            //Creates the connection to the database. Edit the password as needed.
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "1234",
                Database = "mydb",
            };

            BuilderString = builder;
        }


        // Saves new customers to the customer table
        /* InsertRecordIfNotExists
         * 
         * Checks if a customer exists and adds the customer to the db if the record doesn't exist.
         * 
         * @Param Customer_Name The name of the customer being inserted into the data.
         * @param Phone_Number The phone number associated with the customer.
         
         */
        public void InsertRecordIfNotExists(string Customer_Name, string Phone_Number)
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString)) //Creates the Connection
            {
                connection.Open(); //Opens the connection

                //Insert data into the db.
                string query = "INSERT INTO Customer (Customer_Name, Phone_Number) value (@Customer_Name, @Phone_Number)"; using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@Phone_Number", Phone_Number);
                    int result = command.ExecuteNonQuery();
                    if (result < 0) //If no data is entered display error message.
                    {
                        Console.WriteLine("Error inserting data into the database."); 
                    }

                }

                connection.Close(); //Closes connection
            }
        }

        //Deletes Customer by their Customer ID, the PK
        // method must be async to handle popup windows in maui (this so the program thread continues while waiting for the user to click ok on the popup menu)
        public async Task<bool> DeleteRecordIfExists(int customer_num)
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString)) //Creates connection
            {
                try
                {
                    connection.Open(); //Opens the connection

                    string query = "DELETE FROM Customer WHERE `Customer#` = @customer_num"; //Deletes the Customer information
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customer_num", customer_num);
                        int result = command.ExecuteNonQuery(); //Error handling
                        return true; //If the customer is deleted return true.
                    }
                }
                catch (MySqlException ex) when (ex.Message.Contains("a foreign key constraint fails")) //Displays error message
                {
                    // away users to click ok
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot delete customer with active reservations.", "OK");
                    return false; //Returns  false if the customer is not deleted.
                }
                finally 
                {
                    connection.Close(); //Closes connection.
                }
            }
        }


        //retrieves all customers and adds them to a list, this list will be used by the picker wheels
        public List<Customer> FetchAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT `Customer#`, Customer_Name, Phone_Number FROM Customer"; //Selects the customer#, customer_name, Phone_number from the database
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader()) //Sets the values for the Customer Objects to the data retrieved.
                {
                    //
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Customer_Num = reader.GetInt32(0),
                            Name = reader.GetString(1), 
                            PhoneNumber = reader.GetString(2)
                        });
                    }
                }
                connection.Close(); 
            }
            return customers; //Return the list of customers that was created.
        }

        //************END CUSTOMER METHODS*******************//


        //Selects all the booths from the database.
        public Dictionary<int, string> getAllBooths()
        {
            Dictionary<int, string> boothNames = new Dictionary<int, string>(); //Creates a dictionary for each boothNames
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                //Selects booth# and selects boothName from the booth table.
                string sql = "SELECT `Booth#`, Booth_Name " +
                               "FROM booth";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader()) //Assigns the value retrieved from the db to the dictionary.
                {
                    while (reader.Read())
                    {
                        boothNames.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
                connection.Close();
            }
            return boothNames; //Returns the dictionary with the booth# and boothName.
        }

        //Checks if the timeslot was taken. 
        public List<DateTime> takenTimeSlots(int boothID, DateTime date_time)
        {
            List<DateTime> takenTimeSlots = new List<DateTime>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                //Selects the booth and the time that matches the parameters passed.
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT BSC.Date_Time " +
                                        "FROM Booth_Schedule BSC JOIN Booth BTH" +
                                        "                          ON BSC.`Booth#` = BTH.`Booth#`" +
                                       "WHERE BTH.`Booth#` = @selected_booth" +
                                       "  AND DATE(BSC.Date_Time) = @selected_date"; 

                command.Parameters.AddWithValue("@selected_booth", boothID);
                command.Parameters.AddWithValue("@selected_date", date_time.Date);
                
                //If it matches the the parameters given add it to the takenTimeSlots list.
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        takenTimeSlots.Add(reader.GetDateTime(0));
                    }
                }
                connection.Close();
            }
            return takenTimeSlots; //A list of the taken time slots.
        }

        //Confirms the reservation.
        public int confirmReservation(Customer customer, DateTime schedule, int boothID)
        {
            int reservationID = 0;
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                //Inserts into the data into the reservation table.
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "INSERT INTO booth_schedule (Date_Time, `Booth#`) " +
                                           "VALUES (@selected_schedule, @selected_booth)";

                command.Parameters.AddWithValue("@selected_schedule", schedule);
                command.Parameters.AddWithValue("@selected_booth", boothID);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT Booth_schedule_ID " +
                                        "FROM booth_schedule " +
                                       "WHERE Date_Time = @selected_schedule " +
                                         "AND `Booth#` = @selected_booth";

                int boothScheduleID = 0;

                //Sets the boothScheduleID
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boothScheduleID = (int)reader.GetInt64(0);
                    }
                }

                //inserts the data into the reservation table
                command.CommandText = "INSERT INTO reservation (Booth_Schedule_ID, `Customer#`)" +
                                        "VALUES (@booth_scheduleID, @customer_number)";
                command.Parameters.AddWithValue("@booth_scheduleID", boothScheduleID);
                command.Parameters.AddWithValue("@customer_number", customer.Customer_Num);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT LAST_INSERT_ID()"; //Selects the autoincremeted reservationID

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservationID = (int)reader.GetInt64(0);
                    }
                }



                connection.Close();
            }
            return reservationID; //Returns the reservation ID
        }


        public int createBill(int reservation)
        {
            int bill_num = 0;

            //Opens connection to the database.
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                //Creates new bill table using the autoincrement function in sql.
                command.CommandText = "INSERT INTO bill() VALUES ()"; 
                command.Parameters.AddWithValue("@`bill#`", bill_num);
                command.ExecuteNonQuery();


                //Grab the bill_num and assign it to the bill_num Variable.
                command.CommandText = "SELECT LAST_INSERT_ID()";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bill_num = Convert.ToInt32(reader[0]);

                    }
                }

                if (reservation != -1)
                {
                    command.CommandText = "UPDATE Reservation " +
                                             "SET `Bill#` = @bill_num " +
                                           "WHERE `Booking#` = @booking_num";

                    command.Parameters.AddWithValue("@bill_num", bill_num);
                    command.Parameters.AddWithValue("@booking_num", reservation);
                    command.ExecuteNonQuery();
                }

                connection.Close(); //Closes Connection
            }
            return bill_num;
        }

        /*SaveToBill_items
         * Saves the items to the item_bill table.
         * 
         * @param The quantity of the ordered.
         * @param item_name The name of the orderded
         * @param bill number associated to the order
         */
        public void SaveToBill_Items(int quantity, string item_name, int bill_num)
        {
            //Execute if the quantity of pops is not 0. Else do not save any data.
            if (quantity != 0)
            {
                //Placeholder Variables
                int item_num = 1001;

                //Opens connection to the database.
                using (var connection = new MySqlConnection(BuilderString.ConnectionString))
                {
                    connection.Open();


                    //Grabs the item# that correlates to the pop selected.
                    string item = $"SELECT `item#` FROM menu_item WHERE item_name = @item_name";
                    using (MySqlCommand command = new MySqlCommand(item, connection))
                    {
                        command.Parameters.AddWithValue("@item_name", item_name);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                item_num = Convert.ToInt32(reader[0]);
                            }
                        }
                    }

                    //Insets the data into bill_items table
                    string save = "INSERT INTO bill_items(`Item#`, `Bill#`, Quantity) VALUES (@`item#`, @`bill#`, @quantity)"; 
                    using (var command = new MySqlCommand(save, connection))
                    {
                        command.Parameters.AddWithValue("@`item#`", item_num);
                        command.Parameters.AddWithValue("@`bill#`", bill_num);
                        command.Parameters.AddWithValue("@quantity", quantity);

                        command.ExecuteNonQuery();
                    }

                    connection.Close(); //Closes Connection
                }
            }

        }

        /*FetchMainsItems
        * Generates the data from the menu_item table for the Mains Picker.
        * 
        * @return items1 The selected data from the menu_item table.

        */
      
        public List<Item> FetchMainsItems()
        {
            List<Item> items1 = new List<Item>(); //Creates list to store Mains in.
            //Opens the connection to the database.
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT item_name, item_price FROM menu_item WHERE `item#`<2000"; //Selects only the mains from the database table.

                //Adds the data to the items1 List.
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items1.Add(new Item()
                        {
                            Name = reader.GetString(0),
                            Price = reader.GetDouble(1)
                        });
                    }
                }
                connection.Close(); //Close connection
            }
            return items1;
        }

        /*FetchPopsItems
         * 
         * Generates the data from the menu_item table for the Pop Picker.
         * 
         * @return items2 The selected data from the menu_item table.
         
         */
        public List<Item> FetchPopsItems()
        {
            List<Item> items2 = new List<Item>(); //Creates list to store the pop in.

            //Opens connection to the database.
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT item_name, item_price FROM menu_item WHERE `item#`>2000"; //Select only the pops.

                //Adds the data to the items2 list.
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items2.Add(new Item
                        {
                            Name = reader.GetString(0),
                            Price = reader.GetDouble(1)
                        });
                    }
                }
                connection.Close(); //Close connection.
            }
            return items2;
        }

        //Fetch Rez


        //Selects the reservations and assigns them to the database
        public List<Reservation> FetchReservationItems()
        {
            List<Reservation> reservation = new List<Reservation>(); //List for the reservations
            //Selects the data from the reservation where the bill# is null.
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT r.`Booking#`, bs.date_time, c.`customer#`, c.customer_name, c.phone_number, b.booth_name " +
                               "FROM reservation r JOIN customer c " +
                                                    "ON r.`Customer#` = c.`Customer#` " +
                                                  "JOIN booth_schedule bs " +
                                                    "ON r.booth_schedule_id = bs.booth_schedule_id " +
                                                  "JOIN booth b " +
                                                    "ON bs.`Booth#` = b.`Booth#`" +
                             " WHERE r.`Bill#` IS NULL"; 
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservation.Add(new Reservation
                        {
                           BookingNumber = reader.GetInt32(0),
                            Schedule = reader.GetDateTime(1),
                            Customer = new Customer (reader.GetInt32(2), reader.GetString(3), reader.GetString(4)),
                            BoothName = reader.GetString(5)

                        });
                    }

                    connection.Close(); //Close
                }
            }

            return reservation; // Returns the list of the reservations

        }

        //Returns reservation history
        public Dictionary<Reservation, BillClass> GetReservationHistory()
        {
            Dictionary<Reservation, BillClass> reservation = new Dictionary<Reservation, BillClass>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT r.`Booking#`, bs.date_time, c.`customer#`, c.customer_name, c.phone_number, b.booth_name, r.`Bill#` " +
                                        "FROM reservation r JOIN customer c " +
                                                             "ON r.`Customer#` = c.`Customer#` " +
                                                           "JOIN booth_schedule bs " +
                                                             "ON r.booth_schedule_id = bs.booth_schedule_id " +
                                                           "JOIN booth b " +
                                                             "ON bs.`Booth#` = b.`Booth#`" +
                                       "WHERE r.`Bill#` IS NOT NULL " +
                                       "ORDER BY bs.date_time DESC"; //Select the reservation that bill# is not null


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    //Assigns the value to the reservation objects
                    while (reader.Read())
                    {
                        Reservation reservationQuery = new Reservation
                        {
                            BookingNumber = reader.GetInt32(0),
                            Schedule = reader.GetDateTime(1),
                            Customer = new Customer(reader.GetInt32(2), reader.GetString(3), reader.GetString(4)),
                            BoothName = reader.GetString(5)
                        };

                        
                        BillClass reservationBill = new BillClass(); //Creates a Bill

                        //Gets the data from the bill_items table that matches the bill#.
                        using (var secondConnection = new MySqlConnection(BuilderString.ConnectionString))
                        {
                            secondConnection.Open();

                            MySqlCommand itemCommand = secondConnection.CreateCommand();

                            itemCommand.CommandText = "SELECT MI.Item_Name, MI.Item_Price, BI.Quantity" +
                                                       " FROM Bill_Items BI JOIN Menu_Item MI" +
                                                                            " ON BI.`Item#` = MI.`Item#`" +
                                                      " WHERE BI.`Bill#` = @bill_num";
                            itemCommand.Parameters.AddWithValue("@bill_num", reader.GetInt32(6));

                            //Assigns the data into an Item.
                            using (MySqlDataReader itemReader = itemCommand.ExecuteReader())
                            {
                                while (itemReader.Read())
                                {
                                    Item billItem = new Item();
                                    billItem.Name = itemReader.GetString(0);
                                    billItem.Price = itemReader.GetDouble(1);

                                    reservationBill.AddItem(billItem, itemReader.GetInt32(2));
                                }
                            }
                            secondConnection.Close();
                        }
                        

                        reservation.Add(reservationQuery, reservationBill); 
                    }
                }
                connection.Close();
            }

            return reservation; //Returns the reservations and the bill associated with the reservations

        }

        public Dictionary<int, BillClass> GetTakeOutHistory()
        {
            Dictionary<int, BillClass> takeOutOrder = new Dictionary<int, BillClass>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT BI.`Bill#`, MI.Item_Name, MI.Item_Price, BI.Quantity" +
                                            " FROM Bill_Items BI JOIN Menu_Item MI" +
                                                                " ON BI.`Item#` = MI.`Item#`" +
                                            " WHERE BI.`Bill#` NOT IN (SELECT `Bill#`" +
                                                                        "FROM Reservation" +
                                                                      " WHERE `Bill#` IS NOT NULL)" +
                                            " ORDER BY `Bill#` DESC"; //Selects the data from the bill_items table

                using (MySqlDataReader itemReader = command.ExecuteReader())
                {
                    BillClass takeOutBill = new BillClass();
                    int currentOrderId = -1;

                    //Add the bill_items to the dictionary and add an order number to the dictionary
                    while (itemReader.Read())
                    {
                        if (currentOrderId != itemReader.GetInt32(0))
                        {
                            if (currentOrderId != -1)
                            {
                                takeOutOrder.Add(currentOrderId, takeOutBill);
                            }
                            currentOrderId = itemReader.GetInt32(0);
                            takeOutBill = new BillClass();
                        }
                        //Creates item object and assigns the value
                        Item billItem = new Item();
                        billItem.Name = itemReader.GetString(1);
                        billItem.Price = itemReader.GetDouble(2);

                        takeOutBill.AddItem(billItem, itemReader.GetInt32(3));
                    }

                    //Insert the last record to takeOutOrder. If not -1 it means query returned something.
                    if (currentOrderId != -1)
                    {
                        takeOutOrder.Add(currentOrderId, takeOutBill);
                    }
                }
                    
                connection.Close();
            }

            return takeOutOrder; //Returns the order number and the items associated with their quantity.

        }
    }
}