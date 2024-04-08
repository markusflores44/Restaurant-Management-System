namespace RestaurantManagementSystem;

public partial class Reservation_confirm : ContentPage
{
	public Reservation_confirm()
	{
		InitializeComponent();
	}

    private async void OnConfirmReservationButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Reservation_order());
    }

    private async void OnBacktoReservationInputButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Reservation_input());
    }
    
}