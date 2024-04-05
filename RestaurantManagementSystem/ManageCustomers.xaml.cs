using MySqlConnector;

namespace RestaurantManagementSystem;

public partial class ManageCustomers : ContentPage
{
	public ManageCustomers()
	{
		InitializeComponent();
        update_picker();
    }




    // on button click method
    public void Save_Cust_Button_Click(object sender, EventArgs e)
    {
        CustomerAdd();
        update_picker();

    }


    public void CustomerAdd()
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


        // Get the text from the Entry
        string userInput1 = Customer_nameEntry.Text;
        string userInput2 = Phone_numberEntry.Text;

        // call method to insert new customer into custoemr table, pass along arguments from Entry fields
        //Methods that read/write from DB must be in DB class and called with DB object
        dbAccess.InsertRecordIfNotExists(userInput1, userInput2);

        // CONFIRM CUSTOMER HAS BEEN SAVED
        displaycustomerEntry.Text = $"{userInput1} has been Saved!";
    }

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

    // on Button x:Name="del_button" click call these methods
    public void Delete_Cust_Button_Click(object sender, EventArgs e)
    {
        CustomerDelete();
        update_picker();
    }

    // Delete the Customer by ID
    public void CustomerDelete()
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


        // Get the text from the Entry This is fir manual entry into the text field we have changed this to be automatically populated by the picker
        //int userInput1 = int.Parse(del_entry.Text); this was an Entry field======= <Entry x:Name="del_entry" Placeholder="Enter Customer #" TextColor="White" HorizontalOptions="CenterAndExpand" WidthRequest="400"/>


        //selectedCustomerID is created from the function CustomerPicker_SelectedIndexChanged when a customer is selected on the picker wheel
        int userInput1 = selectedCustomerId;
        // call method to insert new customer into custoemr table, pass along arguments from Entry fields
        //Methods that read/write from DB must be in DB class and called with DB object
        dbAccess.DeleteRecordIfExists(userInput1);

        // CONFIRM CUSTOMER HAS BEEN Deleted
        displaycustomerEntry_del.Text = $"Customer #{userInput1} has been Deleted!";
    }




    private int selectedCustomerId = -1; // Variable to store selected Customer's ID
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
            selectedCustomerId = selectedCustomer.Customer_Num;

        }
    }








}