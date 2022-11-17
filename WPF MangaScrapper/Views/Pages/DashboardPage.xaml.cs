
using System.Windows;
using Wpf.Ui.Common.Interfaces;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Windows;

namespace WPF_MangaScrapper.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public static DashboardPage DashboardPageCONTEXT { get; set; }  
        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            DashboardPageCONTEXT = this;

            bool isFullScreen = MainWindow.mainWindowCONTEXT.RootNavigation.Visibility == Visibility.Collapsed;
            if (isFullScreen) { 
            FullScreenButton.Visibility = Visibility.Visible;
            }
            
        }

        private void BRefreshCH(object sender, System.Windows.RoutedEventArgs e)
        {

            UtilServices.ToggleFullScreen(MainWindow.mainWindowCONTEXT);

        }
    }
}