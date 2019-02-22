using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SColorRemote.Models;
using SColorRemote.Views;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SColorRemote.ViewModels
{
    public class DeviceViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public DeviceViewModel()
        {
            
            Title = "LEDs disponibles";
            Items = new ObservableCollection<DeviceItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                //Items.Add(new DeviceItem() { Id = Guid.NewGuid().ToString(), Name = "Fictif", IP = "192.168.1.1" });

                using (var client = new UdpClient())
                {
                    client.EnableBroadcast = true;
                    var endpoint = new IPEndPoint(IPAddress.Broadcast, 32000);
                    var message = Encoding.ASCII.GetBytes("sc");
                    await client.SendAsync(message, message.Length, endpoint);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}