using SColorRemote.Models;
using SColorRemote.Services;
using SColorRemote.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SColorRemote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        public DeviceViewModel viewModel { get; set; }
        public UdpListener UdpClient { get; set; }

        public ConnectionPage()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new DeviceViewModel();
            this.UdpClient = new UdpListener(AddItem);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as DeviceItem;
            if (item == null)
                return;

            Global.NAME = item.Name;
            Global.IP = item.IP;

            await Navigation.PushModalAsync(new NavigationPage(new MenuPage(item)));
            ItemsListView.SelectedItem = null;
        }

        public void AddItem(string message)
        {
            string[] msg = message.Split(new string[] { ";" }, StringSplitOptions.None);
            if (msg.Length == 3)
                if (!this.viewModel.Items.Any(x => x.IP.Equals(msg[2])))
                    this.viewModel.Items.Add(new DeviceItem() { Id = Guid.NewGuid().ToString(), Name = msg[1], IP = msg[2] });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            this.UdpClient.StartListening();
        }

        protected override void OnDisappearing()
        {
            this.UdpClient.StopListening();
            base.OnDisappearing();
        }
    }
}