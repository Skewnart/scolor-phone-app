using SColorRemote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SColorRemote.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerPage : ContentPage
	{
        public Color EditedColor
        {
            get { return Global.SELECTEDCOLOR; }
            set { Global.SELECTEDCOLOR = value;
                this.Changed = true;
                OnPropertyChanged(); }
        }
        public bool Changed { get; set; }

        public ColorPickerPage ()
		{
            this.BindingContext = this;

			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            ((HttpWebRequest)WebRequest.Create($"http://{Global.IP}/leds?red={(int)(Global.SELECTEDCOLOR.R*255)}&green={(int)(Global.SELECTEDCOLOR.G*255)}&blue={(int)(Global.SELECTEDCOLOR.B*255)}")).GetResponseAsync();
            Navigation.PopModalAsync();
        }
    }
}