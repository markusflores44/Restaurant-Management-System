using MySqlConnector;

using System;
using Microsoft.Maui.Controls; // For displaying popups in MAUI

namespace RestaurantManagementSystem

{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnReservationButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Reservation_input());
        }

        private async void OnGoToOrderButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderMain());
        }

        private async void OnAddCustomerButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageCustomers());
        }

        private async void OnOrderNowButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderNow());
        }


    }




        //************BEGIN CUSTOMER METHODS*******************//


        // this class holds all methods for retrieving and deleting Database information
        public class DatabaseAccess
        {
            public MySqlConnectionStringBuilder BuilderString { get; set; }

            public DatabaseAccess(MySqlConnectionStringBuilder builderString)
            {
                BuilderString = builderString;
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


        //Old code without proper error handling
        /*            public void DeleteRecordIfExists(int customer_num)
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
                    }*/


        //Deletes Customer by their Customer ID, the PK

        // method must be async to handle popup windows in maui (this so the program thread continues while waiting for the user to click ok on the popup menu)
        public async Task<bool> DeleteRecordIfExists(int customer_num)
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Customer WHERE `Customer#` = @customer_num";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customer_num", customer_num);
                        int result = command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (MySqlException ex) when (ex.Message.Contains("a foreign key constraint fails"))
                {
                    // away users to click ok
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot delete customer with active reservations.", "OK");
                    return false;   
                }
                finally
                {
                    connection.Close();
                }
            }
        }




        //Deletes Customer by their Customer ID, the PK


        //retrieves all customers and adds them to a list, this list will be used by the picker wheels
        public List<Customer> FetchAllCustomers()
            {
                List<Customer> customers = new List<Customer>();
                using (var connection = new MySqlConnection(BuilderString.ConnectionString))
                {
                    connection.Open();
                //WHERE `Customer#` NOT IN (SELECT `Customer#` FROM reservation)"
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


        }


    

}
