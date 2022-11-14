using Wpf.Ui.Common.Interfaces;

namespace WPF_MangaScrapper.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class GalleryPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public GalleryPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel; InitializeComponent();

        }
    }
}