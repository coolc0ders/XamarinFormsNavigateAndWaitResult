using NavigateAndWaitResult.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NavigateAndWaitResult.Services
{
    public class SimpleNavigationService
    {
        public async Task<T> NavigateToModal<T>(string modalName)
        {
            var source = new TaskCompletionSource<T>();
            if (modalName == nameof(NewItemPage))
            {
                var page = new NewItemPage();
                page.PageDisapearing += (result) =>
                {
                    var res = (T)Convert.ChangeType(result, typeof(T));
                    source.SetResult(res);
                };
                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
            }
            return await source.Task;
        }
    }
}
