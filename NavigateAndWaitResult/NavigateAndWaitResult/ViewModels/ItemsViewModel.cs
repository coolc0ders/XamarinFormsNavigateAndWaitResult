using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using NavigateAndWaitResult.Models;
using NavigateAndWaitResult.Views;
using NavigateAndWaitResult.Services;

namespace NavigateAndWaitResult.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private SimpleNavigationService _navigationService;
        public Command AddItemCommand { get; private set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            _navigationService = new SimpleNavigationService();
            AddItemCommand = new Command(async () => await ExecuteAddItemCommand());
        }

        async Task ExecuteAddItemCommand()
        {
            var item = await new SimpleNavigationService().NavigateToModal<Item>(nameof(NewItemPage));
            Items.Add(item);
            await DataStore.AddItemAsync(item);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
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