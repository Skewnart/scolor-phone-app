using Amporis.Xamarin.Forms.ColorPicker;
using SColorRemote.Models;
using SColorRemote.Services;
using SColorRemote.ViewModels;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SColorRemote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as RemoteItem;
            if (item == null)
                return;

            if (item.Id == 1)
                ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/leds?red=255&green=255&blue=255")).GetResponseAsync();
            else if (item.Id == 2)
                ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/leds?red=0&green=0&blue=0")).GetResponseAsync();
            else if (item.Id == 3)
                await Navigation.PushModalAsync(new ColorPickerPage());
            else if (item.Id == 4)
                ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/allcolors")).GetResponseAsync();
            else if (item.Id == 5)
                ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/random")).GetResponseAsync();
            else if (item.Id == 6)
                foreach (ColorAction action in VoiceRecognition.Recognize(await DependencyService.Get<IVoiceRecognition>().LaunchRecognition()))
                    ((HttpWebRequest)WebRequest.Create(action.ToString())).GetResponseAsync();

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void ChangeItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}