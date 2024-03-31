namespace RestaurantManagementSystem;

public partial class OrderDisplay : ContentPage
{
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