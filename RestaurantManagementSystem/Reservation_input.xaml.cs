using MySqlConnector;

namespace RestaurantManagementSystem;

public partial class Reservation_input : ContentPage
{
	public Reservation_input()
	{
		InitializeComponent();
        update_picker(); //Call update_picker funciton so it loads on the page load.
    }




    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSubmitReservationButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Reservation_confirm());
    }

    //if timeslot is booked, error message will appear






    //**************************** Kyles Code ***************************//


    public void update_picker()         //Update the picker wheel Method
    {
        //DataBase Connection
        // Create a new instance of the MySQL connection string builder
        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            UserID = "root",
            Password = "1234",
            Database = "mydb",
        };
        DatabaseAccess dbAccess = new DatabaseAccess(builder);  //create object of the MSQL string builder

        //Methods that read/write from DB must be in DB class and called with DB object
        List<Customer> customers = dbAccess.FetchAllCustomers(); // Fetch the list of customers
                                                                 // Set picker item source to list of customers
        customerPicker.ItemsSource = customers;
        //display the string line FullDetails from EACH object in Customer Class/Reservation Class
        customerPicker.ItemDisplayBinding = new Binding("FullDetails");
    }






    ////////***************** USE THIS VARIABLE FOR CUSTOMER ID WHEN ADDING RESO TO DATABASE!!!*********///////////
    private int rselectedCustomerId = -1;
    ////////***************** USE THIS VARIABLE FOR CUSTOMER ID WHEN ADDING RESO TO DATABASE!!!*********///////////
    //When Customer is selected in picker , select 
    private void CustomerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Check if the picker selection is valid
        if (customerPicker.SelectedIndex != -1)
        {
            //store selected customer Object in variable
            var selectedCustomer = (Customer)customerPicker.SelectedItem;
            // Store the Customer ID in the variable
            //Store object variable Customer_num in selectedCustomerId
            rselectedCustomerId = selectedCustomer.Customer_Num;

        }
    }

    //**************************** Kyles Code END ***************************//










}