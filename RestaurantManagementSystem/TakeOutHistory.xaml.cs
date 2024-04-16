namespace RestaurantManagementSystem;

public partial class TakeOutHistory : ContentPage
{
    public TakeOutHistory()
    {
        InitializeComponent();

        //Retrieves values from the Database and formats the values to display in Take-Out History
        DatabaseAccess dbAccess = new DatabaseAccess();
        Dictionary<int, BillClass> takeOutHistory = dbAccess.GetTakeOutHistory();

        List<String> display = new List<String>();

        foreach (KeyValuePair<int, BillClass> entry in takeOutHistory)
        {
            string displayValue = $"Order# {entry.Key}";

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

        TakeOutCollection.ItemsSource = display;
    }

    public async void BackToMainPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrderHistory());
    }
}