using MySqlConnector;
using System;

namespace RestaurantManagementSystem

{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // Opens the respective pages based on the button clicked

        private async void OnReservationButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Reservation_input());
        }

        private async void OnGoToOrderButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderMain());
        }

        private async void OnAddCustomerButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageCustomers());
        }

        private async void OnOrderNowButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderNow());
        }

        private async void OrderHistoryButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderHistory());
        }

    }




    


}
