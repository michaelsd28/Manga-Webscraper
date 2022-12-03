using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WPF_MangaScrapper.Services;

namespace WPF_MangaScrapper.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "WPF UI - WPF_MangaScrapper";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = new System.Windows.Controls.TextBlock{ Text = "Home", FontSize= 12},
                    PageTag = "dashboard",
                    PageType = typeof(Views.Pages.DashboardPage),
                    Image =UIStore.NavICONS["BorutoICON"]
                },

                new NavigationItem()
                {
                    Content = new System.Windows.Controls.TextBlock{ Text = "Gallery", FontSize= 12},
                    PageTag = "gallery",
                    PageType = typeof(Views.Pages.GalleryPage),
                         Image = UIStore.NavICONS["GalleryICON"]

                },
                 new NavigationItem()
                {
                    Content = new System.Windows.Controls.TextBlock{ Text = "Admin", FontSize= 12},
                    PageTag = "manager",
                    PageType = typeof(Views.Pages.MangaManager),
                         Image =UIStore.NavICONS["ManagerICON"]
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }
    }
}
