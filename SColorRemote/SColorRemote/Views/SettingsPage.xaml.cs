using SColorRemote.Services;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SColorRemote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private bool canSendIntensity = false;
        public bool CanSendIntensity
        {
            get { return canSendIntensity && this.NotSoundReco; }
            set { canSendIntensity = value;
                OnPropertyChanged("CanSendIntensity"); }
        }
        private bool canSendSpeed = false;
        public bool CanSendSpeed
        {
            get { return canSendSpeed; }
            set { canSendSpeed = value;
                OnPropertyChanged("CanSendSpeed"); }
        }

        public int Intensity
        {
            get { return Global.INTENSITY; }
            set { Global.INTENSITY = value;
                OnPropertyChanged("Intensity");
            }
        }

        public int Speed
        {
            get { return Global.SPEED; }
            set
            {
                Global.SPEED = value;
                OnPropertyChanged("Speed");
            }
        }

        public bool NotSoundReco
        {
            get { return !Global.SOUNDRECO; }
            set
            {
                OnPropertyChanged("NotSoundReco");
                OnPropertyChanged("CanSendIntensity");
            }
        }

        public SettingsPage()
        {
            this.BindingContext = this;

            InitializeComponent();
            this.Entry_Name.Text = Global.NAME;

            this.CanSendIntensity = false;
            this.CanSendSpeed = false;
            this.NotSoundReco = true;
        }

        async void InfoItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new AboutPage()));
        }

        async void Entry_SSIDPassword_CompletedAsync(object sender, EventArgs e)
        {
            string ssid = Entry_SSID.Text;
            string passwd = Entry_Password.Text;
            if (!string.IsNullOrEmpty(ssid) && !string.IsNullOrEmpty(passwd))
                if (await DisplayAlert("Nouvelle connexion", "Se connecter au nouveau réseau ?", "Oui", "Non"))
                {
                    ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/connection?ssid={ssid}&password={passwd}")).GetResponseAsync();
                    await Navigation.PopModalAsync();
                }
        }

        private void Entry_Name_Completed(object sender, EventArgs e)
        {
            string name = Entry_Name.Text.Trim();
            if (!String.IsNullOrEmpty(name))
                ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/name?name={name}")).GetResponseAsync();
        }

        private void SliderIntensity_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.CanSendIntensity = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/intensity?int={this.Intensity}")).GetResponseAsync();
            this.CanSendIntensity = false;
        }

        private void SwitchRecoSound_Toggled(object sender, ToggledEventArgs e)
        {
            ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/soundreco?reco={(((bool)e.Value) ? 1 : 0)}")).GetResponseAsync();
            Global.SOUNDRECO = (bool)e.Value;
            this.NotSoundReco = true;
        }

        private void SliderSpeed_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider.Value == 0) slider.Value = 1;

            this.CanSendSpeed = true;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/speed?speed={this.Speed}")).GetResponseAsync();
            this.CanSendSpeed = false;
        }
    }
}