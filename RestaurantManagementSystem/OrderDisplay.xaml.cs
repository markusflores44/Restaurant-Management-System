namespace RestaurantManagementSystem;
public partial class OrderDisplay : ContentPage
{
    private Item selectedItem1;
    private Item selectedItem2;
    int mq;
    int pq;
    string item_name1;
    string item_name2;
    int reservation_num;

    //Take  out order display
    public OrderDisplay(Item selectedItem1, int mainsquantity, Item selectedItem2, int popsquantity, double totalcost)
    {
        InitializeComponent();

        MainsChosen.Text = $"Mains: {selectedItem1.Name}    Price-{selectedItem1.Price}    Qty-{mainsquantity} ";
        PopsChosen.Text = $"Pops: {selectedItem2.Name}    Price-{selectedItem2.Price}    Qty-{popsquantity}";
        TotalCost.Text = $"Total Cost: {totalcost}";
        mq = mainsquantity;
        pq = popsquantity;
        item_name1 = selectedItem1.Name;
        item_name2 = selectedItem2.Name;

        //If the order is not associated to any reservation, the reservation is -1 and the reservation is not visible in the UI
        reservation_num = -1;
        BookingNumber.IsVisible = false;
        
    }

    //reservation related order display
    public OrderDisplay(Item selectedItem1, int mainsquantity, Item selectedItem2, int popsquantity, double totalcost, Reservation reservation)
    {
        InitializeComponent();
        BookingNumber.Text = $"Booking Number: {reservation.BookingNumber}";
        MainsChosen.Text = $"Mains: {selectedItem1.Name}    Price-{selectedItem1.Price}    Qty-{mainsquantity}";
        PopsChosen.Text = $"Pops: {selectedItem2.Name}   Price-{selectedItem2.Price}    Qty-{popsquantity}";
        TotalCost.Text = $"Total Cost: {totalcost}";

        this.selectedItem1 = selectedItem1;
        this.selectedItem2 = selectedItem2;
        this.mq = mainsquantity ;
        this.pq = popsquantity ;
        item_name1 = selectedItem1.Name;
        item_name2 = selectedItem2.Name;
        reservation_num = reservation.BookingNumber;

    }
    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    //saves data to db once clicked.
    private async void OnSendToOrderButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());

        // Generate the bill number first and then save the items associated to the bill
        DatabaseAccess access = new DatabaseAccess();
        int bill_num = access.createBill(reservation_num);
        access.SaveToBill_Items(mq, item_name1, bill_num);
        access.SaveToBill_Items(pq, item_name2, bill_num);
    }

}