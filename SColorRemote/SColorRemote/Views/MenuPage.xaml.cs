using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SColorRemote.Models;
using SColorRemote.Services;
using System;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SColorRemote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : TabbedPage
    {
        public DeviceItem Device { get; set; }

        public MenuPage(DeviceItem device)
        {
            this.Device = device;
            
            Stream receiveStream = ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/infos")).GetResponse().GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string response = readStream.ReadToEnd();
            if (!string.IsNullOrEmpty(response))
            {
                JObject jobj = JObject.Parse(response);
                if (jobj.ContainsKey("intensity"))
                    Global.INTENSITY = Int32.Parse(jobj["intensity"].Value<string>());
                if (jobj.ContainsKey("speed"))
                    Global.SPEED = Int32.Parse(jobj["speed"].Value<string>());
                if (jobj.ContainsKey("soundReco"))
                    Global.SOUNDRECO = jobj["speed"].Value<string>().Equals("1");

                if (jobj.ContainsKey("micmin"))
                    Global.MICMIN = Int32.Parse(jobj["micmin"].Value<string>());
                if (jobj.ContainsKey("micmax"))
                    Global.MICMAX = Int32.Parse(jobj["micmax"].Value<string>());
            }

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}