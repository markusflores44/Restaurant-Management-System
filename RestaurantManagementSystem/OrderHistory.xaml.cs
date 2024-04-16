namespace RestaurantManagementSystem;

public partial class OrderHistory : ContentPage
{
	public OrderHistory()
	{
		InitializeComponent();
	}

    // Opens Take Out History Page
    private async void TakeOutHistory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TakeOutHistory());
    }

    // Opens Reservation History Page
    private async void ReservationHistory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReservationHistory());
    }

    // Opens the Main Page
    private async void BackToMainPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}