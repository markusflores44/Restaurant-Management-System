namespace RestaurantManagementSystem;

public partial class OrderDisplay : ContentPage
{
    private Reservation _reservation;
    private Item _item1;
    private Item _item2;

    public OrderDisplay()
	{
		InitializeComponent();
    }



    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSendToOrderButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Bill());
    }

}