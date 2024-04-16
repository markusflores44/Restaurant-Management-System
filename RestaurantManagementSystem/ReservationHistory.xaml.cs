namespace RestaurantManagementSystem;

public partial class ReservationHistory : ContentPage
{
	public ReservationHistory()
	{
		InitializeComponent();

        //Retrieves values from the Database and formats the values to display in Reservation History
        DatabaseAccess dbAccess = new DatabaseAccess();
        Dictionary<Reservation, BillClass> reservations = dbAccess.GetReservationHistory();

        List<String> display = new List<String>();

        foreach (KeyValuePair<Reservation, BillClass> entry in reservations)
        {
            string displayValue = entry.Key.ToString();

            double totalCost = 0;

            int index = 0;
            foreach (var item in entry.Value.ItemList)
            {
                displayValue += $"\n{item.Name} - Qty. {entry.Value.QuantityList[index]} - ${Math.Round(item.Price * entry.Value.QuantityList[index], 2)}";
                totalCost += item.Price * entry.Value.QuantityList[index];
                index++;
            }
            displayValue += $"\nTotal Cost: ${Math.Round(totalCost, 2)}";
            display.Add(displayValue);
        }

        HistoryCollection.ItemsSource = display;
    }

    public async void BackToMainPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrderHistory());
    }
}