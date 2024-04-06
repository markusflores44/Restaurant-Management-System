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


    int Items = 0;
    //items add function
    private void OnAddItemsClicked(object sender, EventArgs e)
    {

        Items++;

        if (Items < 0)
        {
            Items = 1;
            ItemsNumber.Text = $"{Items}";
        }
        else
        {

            ItemsNumber.Text = $"{Items}";
        }

    }

    //items minus button
    private void OnMinusItemsClicked(object sender, EventArgs e)
    {

        Items--;

        if (Items < 0)
        {
            Items = 0;
        }
        else
        {

            ItemsNumber.Text = $"{Items}";
        }

    }

    public void totalPriceCalculate(object sender, EventArgs e)
    {




    }








}