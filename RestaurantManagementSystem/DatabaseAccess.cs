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
        public MySqlConnectionStringBuilder BuilderString { get; set; }

        public DatabaseAccess()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "root",
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

    }
}
