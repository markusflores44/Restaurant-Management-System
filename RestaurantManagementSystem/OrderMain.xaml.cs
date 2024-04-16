using MySqlConnector;

namespace RestaurantManagementSystem;

public partial class OrderMain : ContentPage
{
    public MySqlConnectionStringBuilder BuilderString { get; set; }
    public OrderMain()
    {
        //Initalizes the component and creates connection to the database. 
        InitializeComponent();
        DatabaseAccess access = new DatabaseAccess();

        //Displays the pickers for the mains and drinks.
        List<Item> mains = access.FetchMainsItems();
        MenuMainsPicker.ItemsSource = mains;
        MenuMainsPicker.ItemDisplayBinding = new Binding("FullDetails");
        List<Item> drinks = access.FetchPopsItems();
        MenuPopsPicker.ItemsSource = drinks;
        MenuPopsPicker.ItemDisplayBinding = new Binding("FullDetails");


    }

    //Button that lets you go back to the main page of the application.
    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    //Functionality of the Confirmation Button.
    private async void OnConfirmOrderButtonClicked(object sender, EventArgs e)
    {
        //Displays an error message if the user has not selected any items.
        if (MenuMainsPicker.SelectedIndex == -1
            || MenuPopsPicker.SelectedIndex == -1)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields before confirming", "OK");
        }
        else //Creates two Item objects and sets the values to the selected items from the pickers. Calculates the mains and pops quantity and total price.
        {
            Item selectedItem1 = (Item)MenuMainsPicker.SelectedItem;
            Item selectedItem2 = (Item)MenuPopsPicker.SelectedItem;
            double totalcost = Convert.ToDouble(totalPrice.Text);
            int mainsquantity = Convert.ToInt32(MainsItemsNumber.Text);
            int popsquantity = Convert.ToInt32(PopsItemsNumber.Text);
            await Navigation.PushAsync(new OrderDisplay(selectedItem1, mainsquantity, selectedItem2, popsquantity, totalcost)); //Navigates to the OrderDisplay Page
        }
    }

    //Fields
    double total_Price = 0;
    double MainsperItemPrice = 0;
    double PopsperItemPrice = 0;
    int MainsItems = 0;
    int PopsItems = 0;
    
    //Mains add function
    private void OnAddMainsItemsClicked(object sender, EventArgs e)
    {

        MainsItems++; //ADDS 1 to the MainsItems Variable

        //Stops the user from going below 0 and updates the text display.
        if (MainsItems < 0)
        {
            MainsItems = 1;
            MainsItemsNumber.Text = $"{MainsItems}";
        }
        else
        {

            MainsItemsNumber.Text = $"{MainsItems}";
        }
        UpdateTotalPrice(); //Calculates and updates the total price. 

    }

    //Mains minus button
    private void OnMinusMainsItemsClicked(object sender, EventArgs e)
    {

        MainsItems--; //Subtract 1 from the MainsItems Variable

        //Stops the user from going below 0 and updates the text display.
        if (MainsItems < 0)
        {
            MainsItems = 0; //Sets MainsItems to 0
        }
        else 
        {

            MainsItemsNumber.Text = $"{MainsItems}"; //Displays the MainsItems Variable
        }

        UpdateTotalPrice(); //Calculates and updates the total price.
    }

    //Pops add button
    private void OnAddPopsItemsClicked(object sender, EventArgs e)
    {

        PopsItems++; //Adds 1 to the PopsItems variable.

        ////Stops the user from going below 0 and updates the text display.
        if (PopsItems < 0)
        {
            PopsItems = 1; //Sets pop to 1 and displays the PopsItem  to ensure that the correct value.
            PopsItemsNumber.Text = $"{PopsItems}";
        }
        else //Display PopsItems variable to the screen. 
        {

            PopsItemsNumber.Text = $"{PopsItems}";
        }
        UpdateTotalPrice(); //Calculates update the value of the totalPrice varaible.

    }



    //Pops minus button
    private void OnMinusPopsItemsClicked(object sender, EventArgs e)
    {

        PopsItems--; //Subtract 1 from the PopsItem variable
        if (PopsItems < 0)
        {
            PopsItems = 0; //Sets the variable to 0.
        }
        else //Display variable.
        {

            PopsItemsNumber.Text = $"{PopsItems}";
        }

        UpdateTotalPrice(); //Calcuates and updates
    }

    //Assigns the selected values from the MainsPicker
    public void MenuMainsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedItem = (Item)picker.SelectedItem;
        if (selectedItem != null)
        {
            MainsperItemPrice = selectedItem.Price;

        }
        UpdateTotalPrice(); //Calculate and updates the totalPrice
    }

    //Assigns the selected values from the PopsPicker
    public void MenuPopsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedItem = (Item)picker.SelectedItem;
        if (selectedItem != null)
        {
            PopsperItemPrice = selectedItem.Price;

        }
        UpdateTotalPrice(); //Calculates and updates the totalPrice
    }

    //Calculates the Total Price
    public void UpdateTotalPrice()
    {
        total_Price = MainsperItemPrice * MainsItems + PopsperItemPrice * PopsItems;
        totalPrice.Text = total_Price.ToString("F2");
    }

}