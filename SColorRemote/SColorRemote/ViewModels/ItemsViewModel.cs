using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SColorRemote.Models;
using SColorRemote.Views;

namespace SColorRemote.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<RemoteItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Télécommande";
            Items = new ObservableCollection<RemoteItem>();
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
                Items.Add(new RemoteItem { Id = 1, Title = "Allumé", Description = "Versailles" });
                Items.Add(new RemoteItem { Id = 2, Title = "Eteint", Description = "La nuit" });
                Items.Add(new RemoteItem { Id = 3, Title = "Couleur fixe", Description = "Donnez-moi une couleur." });
                Items.Add(new RemoteItem { Id = 4, Title = "Cercle Chromatique", Description = "Toutes les couleurs possile." });
                Items.Add(new RemoteItem { Id = 5, Title = "Aléatoire", Description = "Parce que quand on s'y attend pas c'est cool." });
                Items.Add(new RemoteItem { Id = 6, Title = "Reconnaissance vocale", Description = "Parlez-moi." });
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