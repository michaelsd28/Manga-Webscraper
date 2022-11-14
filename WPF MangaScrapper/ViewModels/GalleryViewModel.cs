using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

namespace WPF_MangaScrapper.ViewModels
{
    public partial class GalleryViewModel : ObservableObject, INavigationAware
    {

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void OnCounterIncrement()
        {
         
        }
    }
}
