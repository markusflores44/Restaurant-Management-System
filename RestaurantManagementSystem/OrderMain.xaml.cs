using MySqlConnector;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

namespace RestaurantManagementSystem;

public partial class OrderMain : ContentPage
{
    private BillClass bill = new BillClass();
    private Item selectedMainsItem;
    private Item selectedPopsItem;
    public OrderMain()
    {
        InitializeComponent();


        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            UserID = "root",
            Password = "password",
            Database = "mydb",
        };

        DatabaseAccess dbAccess = new DatabaseAccess(builder);
        List<Item> items1 = dbAccess.FetchMainsItems();
        MenuMainsPicker.ItemsSource = items1;
        MenuMainsPicker.ItemDisplayBinding = new Binding("FullDetails");

        List<Item> items2 = dbAccess.FetchPopsItems();
        MenuPopsPicker.ItemsSource = items2;
        MenuPopsPicker.ItemDisplayBinding = new Binding("FullDetails");



    }

    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnConfirmOrderButtonClicked(object sender, EventArgs e)
    {
        Item selectedItem1 = (Item)MenuMainsPicker.SelectedItem;
        Item selectedItem2 = (Item)MenuPopsPicker.SelectedItem;
        double totalcost= Convert.ToDouble(totalPrice.Text);
        int mainsquantity = Convert.ToInt32(MainsItemsNumber.Text);
        int popsquantity= Convert.ToInt32(PopsItemsNumber.Text);

        await Navigation.PushAsync(new OrderDisplay(selectedItem1,mainsquantity, selectedItem2, popsquantity, totalcost));
    }


    public class Item
    {

        public string itemname { get; set; }
        public double itemprice { get; set; }
        public string FullDetails => $"Item Name: {itemname}, Item Price:{itemprice}";
    }

    public class DatabaseAccess
    {
        public MySqlConnectionStringBuilder BuilderString { get; set; }
        public DatabaseAccess(MySqlConnectionStringBuilder builderString)
        {
            BuilderString = builderString;
        }

        //connect with database to choose mains items and display
        List<Item> items1 = new List<Item>();
        public List<Item> FetchMainsItems()
        {

            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT item_name, item_price FROM menu_item WHERE `item#`<2000";

                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items1.Add(new Item
                        {
                            itemname = reader.GetString(0),
                            itemprice = reader.GetDouble(1)
                        });
                    }
                }
                connection.Close();
            }
            return items1;
        }


        //connect with database to choose pops items and display
        List<Item> items2 = new List<Item>();
        public List<Item> FetchPopsItems()
        {
            using (var connection = new MySqlConnection(BuilderString.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT item_name, item_price FROM menu_item WHERE `item#`>2000";

                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items2.Add(new Item
                        {
                            itemname = reader.GetString(0),
                            itemprice = reader.GetDouble(1)
                        });
                    }
                }
                connection.Close();
            }
            return items2;
        }
    }


    double total_Price = 0;
    double MainsperItemPrice = 0;
    double PopsperItemPrice = 0;
    int MainsItems = 0;
    int PopsItems = 0;
    //items add function
    private void OnAddMainsItemsClicked(object sender, EventArgs e)
    {

        MainsItems++;


        if (MainsItems < 0)
        {
            MainsItems = 1;
            MainsItemsNumber.Text = $"{MainsItems}";
        }
        else
        {

            MainsItemsNumber.Text = $"{MainsItems}";
        }


        UpdateTotalPrice();

    }

    //items minus button
    private void OnMinusMainsItemsClicked(object sender, EventArgs e)
    {

        MainsItems--;

        if (MainsItems < 0)
        {
            MainsItems = 0;
        }
        else
        {

            MainsItemsNumber.Text = $"{MainsItems}";
        }

        UpdateTotalPrice();
    }

    private void OnAddPopsItemsClicked(object sender, EventArgs e)
    {

        PopsItems++;

        if (PopsItems < 0)
        {
            PopsItems = 1;
            PopsItemsNumber.Text = $"{PopsItems}";
        }
        else
        {

            PopsItemsNumber.Text = $"{PopsItems}";
        }
        UpdateTotalPrice();

    }



    //items minus button


    private void OnMinusPopsItemsClicked(object sender, EventArgs e)
    {

        PopsItems--;
        if (PopsItems < 0)
        {
            PopsItems = 0;
        }
        else
        {

            PopsItemsNumber.Text = $"{PopsItems}";
        }

        
        UpdateTotalPrice();
    }
  

    public void MenuMainsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedItem = (Item)picker.SelectedItem;
        if (selectedItem != null)
        {
            MainsperItemPrice = selectedItem.itemprice;

        }
        UpdateTotalPrice();
    }

    public void MenuPopsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedItem = (Item)picker.SelectedItem;
        if (selectedItem != null)
        {
            PopsperItemPrice = selectedItem.itemprice;

        }
        UpdateTotalPrice();
    }

    public void UpdateTotalPrice()
    {
        total_Price = MainsperItemPrice * MainsItems + PopsperItemPrice * PopsItems;
        totalPrice.Text = total_Price.ToString("F2");
    }


}