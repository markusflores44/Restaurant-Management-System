namespace RestaurantManagementSystem;

public partial class Reservation_input : ContentPage
{
	public Reservation_input()
	{
		InitializeComponent();
	}

    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSubmitReservationButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Reservation_confirm());
    }

    //if timeslot is booked, error message will appear

}