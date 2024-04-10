using MySqlConnector;

namespace RestaurantManagementSystem;

public partial class OrderNow : ContentPage
{
	public OrderNow()
	{
		InitializeComponent();
        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            UserID = "root",
            Password = "password",
            Database = "mydb",
        };
        //mains picker
        DatabaseAccess dbAccess = new DatabaseAccess(builder);
        List<Item> items1 = dbAccess.FetchMainsItems();
        MenuMainsPicker.ItemsSource = items1;
        MenuMainsPicker.ItemDisplayBinding = new Binding("FullDetails");
        //pops picker
        List<Item> items2 = dbAccess.FetchPopsItems();
        MenuPopsPicker.ItemsSource = items2;
        MenuPopsPicker.ItemDisplayBinding = new Binding("FullDetails");
        //reservation information picker
        List<Reservation> reservations=dbAccess.FetchReservationItems();
        ReservationSearchPicker.ItemsSource = reservations;
        ReservationSearchPicker.ItemDisplayBinding = new Binding("FullDetails");


    }

    //back to the main page
    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    //click button event, collect data from ordernow to orderdisplay
    private async void OnConfirmOrderButtonClicked(object sender, EventArgs e)
    {
        Item selectedItem1 = (Item)MenuMainsPicker.SelectedItem;
        Item selectedItem2 = (Item)MenuPopsPicker.SelectedItem;
        double totalcost = Convert.ToDouble(totalPrice.Text);
        int mainsquantity = Convert.ToInt32(MainsItemsNumber.Text);
        int popsquantity = Convert.ToInt32(PopsItemsNumber.Text);
        Reservation reservation = (Reservation)ReservationSearchPicker.SelectedItem;

        await Navigation.PushAsync(new OrderDisplay(selectedItem1, mainsquantity, selectedItem2, popsquantity, totalcost,reservation));
    }


    public class Item
    {

        public string itemname { get; set; }
        public double itemprice { get; set; }
        public string FullDetails => $"Item Name: {itemname}, Item Price:{itemprice}";
    }

    public class Reservation
    {
        public int bookingnumber {  get; set; }
        public DateTime reservationTime { get; set; }
        public string reservationName { get; set; }
        public string reservationTable { get; set; }
        public string FullDetails => $"Booking Number : {bookingnumber} Reservation Time: {reservationTime}, Reservation Name:{reservationName}, Reservation Table:{reservationTable}";
    }

    public class DatabaseAccess
    {
        public MySqlConnectionStringBuilder BuilderString { get; set; }
        public DatabaseAccess(MySqlConnectionStringBuilder builderString)
        {
            BuilderString = builderString;
        }

        //select items from database where mains items number is smaller than 2000
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


        //select items from database where pop items number is bigger than 2000
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


        //Search reservation here

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
             "JOIN booth b ON bs.`Booth#` = b.`Booth#`";
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


    //add pops function
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



    // minus pops button
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

    //make selected item display on the picker
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



    //reservation combined
    public int booking_number;
    public void ReservationResearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedReservation = (Reservation)picker.SelectedItem;
        if (selectedReservation != null)
        {
            booking_number = selectedReservation.bookingnumber;

        }
    }



    public void UpdateTotalPrice()
    {
        total_Price = MainsperItemPrice * MainsItems + PopsperItemPrice * PopsItems;
        totalPrice.Text = total_Price.ToString("F2");
    }


}