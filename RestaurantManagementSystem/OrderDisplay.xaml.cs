namespace RestaurantManagementSystem;

public partial class OrderDisplay : ContentPage
{
    private OrderNow.Item selectedItem1;
    private OrderNow.Item selectedItem2;

    //take out order display
    public OrderDisplay(OrderMain.Item selectedItem1, int mainsquantity, OrderMain.Item selectedItem2, int popsquantity, double totalcost)
    {
        InitializeComponent();

        MainsChosen.Text = $"Mains: {selectedItem1.itemname}    Price-{selectedItem1.itemprice}    Number-{mainsquantity} ";
        PopsChosen.Text = $"Pops: {selectedItem2.itemname}    Price-{selectedItem2.itemprice}    Number-{popsquantity}";
        TotalCost.Text = $"Total Cost: {totalcost}";

        SelectedItem1 = selectedItem1;
        SelectedItem2 = selectedItem2;
        Totalcost = totalcost;
    }


    //reservation related order display
    public OrderDisplay(OrderNow.Item selectedItem1, int mainsquantity, OrderNow.Item selectedItem2, int popsquantity, double totalcost, OrderNow.Reservation reservation)
    {
        InitializeComponent();
        BookingNumber.Text = $"Booking Number:{reservation.bookingnumber}";
        MainsChosen.Text = $"Mains: {selectedItem1.itemname}    Price-{selectedItem1.itemprice}    Number-{mainsquantity}";
        PopsChosen.Text = $"Pops: {selectedItem2.itemname}   Price-{selectedItem2.itemprice}    Number-{popsquantity}";
        TotalCost.Text = $"Total Cost: {totalcost}";

        this.selectedItem1 = selectedItem1;
        this.selectedItem2 = selectedItem2;
        Totalcost = totalcost;
    }

    public OrderMain.Item SelectedItem1 { get; }
    public OrderMain.Item SelectedItem2 { get; }
    public double Totalcost { get; }

    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSendToOrderButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Bill());
    }

}