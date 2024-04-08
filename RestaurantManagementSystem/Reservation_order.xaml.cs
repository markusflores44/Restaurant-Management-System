namespace RestaurantManagementSystem;

public partial class Reservation_order : ContentPage
{
	public Reservation_order()
	{
		InitializeComponent();
	}

    private async void OnBacktoMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnGoToOrderButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrderMain());
    }
}