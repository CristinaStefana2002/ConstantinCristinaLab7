using ConstantinCristinaLab7.Models;
using Plugin.LocalNotification;
namespace ConstantinCristinaLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
        BindingContext = new Shop();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        bool answer = await DisplayAlert("Confirm", "Are you sure you want to delete this shop?", "Yes", "No");

        if (answer)
        {
            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();
        }
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;  
        var address = shop.Adress;  

        var locations = await Geocoding.GetLocationsAsync(address);
        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };

        var shopLocation = locations?.FirstOrDefault();

        var myLocation = await Geolocation.GetLocationAsync();
        /*   var myLocation = new Location(46.7731796289, 23.6213886738); 
//pentru Windows Machine */

        var distance = myLocation.CalculateDistance(shopLocation, DistanceUnits.Kilometers);

        if (distance < 5 && distance >= 0)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumpărături în apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(shopLocation, options);
    }
    

}