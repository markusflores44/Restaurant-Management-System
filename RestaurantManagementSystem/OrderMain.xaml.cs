namespace RestaurantManagementSystem;

public partial class OrderMain : ContentPage
{
	public OrderMain()
	{
		InitializeComponent();
	}

    private async void OnBackToMainButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnConfirmOrderButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrderDisplay());
    }

}