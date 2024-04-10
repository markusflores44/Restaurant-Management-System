using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Working ON Latest Apr 4, 2024
namespace RestaurantManagementSystem
{
    // this class holds all methods for retrieving and deleting Database information
    public class DatabaseAccess
    {
        public MySqlConnectionStringBuilder BuilderString { get; set; }

        //May need to change.
        public DatabaseAccess()
        {
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
        public void InsertRecordIfNotExists(string Customer_Name, string Phone_Number)
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO Customer (Customer_Name, Phone_Number) value (@Customer_Name, @Phone_Number)"; using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@Phone_Number", Phone_Number);
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into the database.");
                    }

                }

                connection.Close();
            }
        }

        //Deletes Customer by their Customer ID, the PK
        public void DeleteRecordIfExists(int customer_num)
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string query = "Delete FROM Customer WHERE `Customer#` = @customer_num"; using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customer_num", customer_num);
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into the database.");
                    }
                }
                connection.Close();
            }
        }


        //retrieves all customers and adds them to a list, this list will be used by the picker wheels
        public List<Customer> FetchAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT `Customer#`, Customer_Name, Phone_Number FROM Customer";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
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
            return customers;
        }

        //************END CUSTOMER METHODS*******************//


        public Dictionary<int, string> getAllBooths()
        {
            Dictionary<int, string> boothNames = new Dictionary<int, string>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT `Booth#`, Booth_Name " +
                               "FROM booth";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boothNames.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
                connection.Close();
            }
            return boothNames;
        }

        public List<DateTime> takenTimeSlots(int boothID, DateTime date_time)
        {
            List<DateTime> takenTimeSlots = new List<DateTime>();
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT BSC.Date_Time " +
                                        "FROM Booth_Schedule BSC JOIN Booth BTH" +
                                        "                          ON BSC.`Booth#` = BTH.`Booth#`" +
                                       "WHERE BTH.`Booth#` = @selected_booth" +
                                       "  AND DATE(BSC.Date_Time) = @selected_date";

                command.Parameters.AddWithValue("@selected_booth", boothID);
                command.Parameters.AddWithValue("@selected_date", date_time.Date);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        takenTimeSlots.Add(reader.GetDateTime(0));
                    }
                }
                connection.Close();
            }
            return takenTimeSlots;
        }

        public int confirmReservation(Customer customer, DateTime schedule, int boothID)
        {
            int reservationID = 0;
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();

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

                //NOT WORKING???(What is createCommand???? Reader is Null!
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boothScheduleID = (int)reader.GetInt64(0);
                    }
                }


                command.CommandText = "INSERT INTO reservation (Booth_Schedule_ID, `Customer#`, Number_of_Customers)" +
                                        "VALUES (@booth_scheduleID, @customer_number, @num_of_customers)";
                command.Parameters.AddWithValue("@booth_scheduleID", boothScheduleID);
                command.Parameters.AddWithValue("@customer_number", customer.Customer_Num);
                command.Parameters.AddWithValue("@num_of_customers", 0);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT LAST_INSERT_ID()";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservationID = (int)reader.GetInt64(0);
                    }
                }



                connection.Close();
            }
            return reservationID;
        }

        //Creates a single table and saves the mains and pop to the database.
        int sharedBillNum = 0;

        /*SaveMainToBill_items
         * Saves the mains to the database under the bill_items table.
         * @param quantity The quantity of each item.
         * @param item_name The name of the main that is being saved.
         
         */
        public void SaveMainToBill_Items(int quantity, string item_name)
        {

            //Placeholder Variables
            int item_num = 1001;
            int bill_num = 0;

            //Execute only if the quantity is not 0. Else do not save any data.
            if (quantity != 0)
            {
                //Opens the connection to the database.
                using (var connection = new MySqlConnection(BuilderString.ConnectionString))
                {
                    connection.Open();

                    //Creates new bill table using the autoincrement function in sql.
                    string billNum = "INSERT INTO bill() VALUES ()"; using (var command = new MySqlCommand(billNum, connection))
                    {

                        command.Parameters.AddWithValue("@`bill#`", bill_num);


                        command.ExecuteNonQuery();
                    }

                    //Grab the bill_num and assign it to the bill_num Variable.
                    string query = "SELECT LAST_INSERT_ID()";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bill_num = Convert.ToInt32(reader[0]);

                            }
                        }
                    }

                    //Selects the item# that matches the selected main.
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

                    //Inserts the data into the bill_items table.
                    string save = "INSERT INTO bill_items(`Item#`, `Bill#`, Quantity) VALUES (@`item#`, @`bill#`, @quantity)"; using (var command = new MySqlCommand(save, connection))
                    {
                        command.Parameters.AddWithValue("@`item#`", item_num);
                        command.Parameters.AddWithValue("@`bill#`", bill_num);
                        command.Parameters.AddWithValue("@quantity", quantity);

                        command.ExecuteNonQuery();
                    }

                    sharedBillNum = bill_num; //Assigns value to sharedBillNum variable.
                    connection.Close(); //Closes connection
                }
            }
        }
        /*SavePopToBill_items
         * Saves the pop items to the item_bill table.
         * 
         * @param The quantity of the pop ordered.
         * @param item_name The name of the pop orderded
         */
        public void SavePopToBill_Items(int quantity, string item_name)
        {
            //Execute if the quantity of pops is not 0. Else do not save any data.
            if (quantity != 0)
            {
                //Placeholder Variables
                int item_num = 1001;
                int bill_num = sharedBillNum; //Assigns the bill_num var to the sharedBillNum to be on the same bill as the Mains.

                //Opens connection to the database.
                using (var connection = new MySqlConnection(BuilderString.ConnectionString))
                {
                    connection.Open();

                    //ERROR HANDLING: If mains quantity is 0. Bill_num was not created so creates it here.
                    if (bill_num == 0)
                    {

                        //Creates new bill table using the autoincrement function in sql.
                        string billNum = "INSERT INTO bill() VALUES ()"; using (var command = new MySqlCommand(billNum, connection))
                        {

                            command.Parameters.AddWithValue("@`bill#`", bill_num);


                            command.ExecuteNonQuery();
                        }

                        //Grab the bill_num and assign it to the bill_num Variable.
                        string query = "SELECT LAST_INSERT_ID()";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    bill_num = Convert.ToInt32(reader[0]);

                                }
                            }
                        }
                    }

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
                    string save = "INSERT INTO bill_items(`Item#`, `Bill#`, Quantity) VALUES (@`item#`, @`bill#`, @quantity)"; using (var command = new MySqlCommand(save, connection))
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
        //Creates Item class(ASK WHY??)


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
        public class Reservation
        {
            public int bookingnumber { get; set; }
            public DateTime reservationTime { get; set; }
            public string reservationName { get; set; }
            public string reservationTable { get; set; }
            public string FullDetails => $"Booking Number : {bookingnumber} Reservation Time: {reservationTime}, Reservation Name:{reservationName}, Reservation Table:{reservationTable}";
        }

        List<Reservation> reservation = new List<Reservation>();
        public List<Reservation> FetchReservationItems()
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT r.`Booking#`, bs.date_time, c.customer_name, b.booth_name " +
             "FROM reservation r " +
             "JOIN customer c ON r.`Customer#` = c.`Customer#` " +
             "JOIN booth_schedule bs ON r.booth_schedule_id = bs.booth_schedule_id " +
             "JOIN booth b ON bs.`Booth_Booth#` = b.`Booth#`";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservation.Add(new Reservation
                        {
                            bookingnumber = reader.GetInt32(0),
                            reservationTime = reader.GetDateTime(1),
                            reservationName = reader.GetString(2),
                            reservationTable = reader.GetString(3)

                        }); ;
                    }

                    connection.Close();
                }

                return reservation;
            }

        }
    }
}