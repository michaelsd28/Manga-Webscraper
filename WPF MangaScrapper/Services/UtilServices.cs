using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using WPF_MangaScrapper.Views.Pages;
using WPF_MangaScrapper.Views.Windows;
using Wpf.Ui.Extensions;
using WPF_MangaScrapper.ViewModels;

namespace WPF_MangaScrapper.Services
{
    internal class UtilServices
    {

        public static BitmapImage ByteToBitmapIMG(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }



        public static void ToggleFullScreen(MainWindow mainWindow) {

            Boolean isFullSCreen = mainWindow.RootNavigation.Visibility == Visibility.Collapsed;


            if (!isFullSCreen)
            {

                DashboardPage.DashboardPageCONTEXT.FullScreenButton.Visibility = Visibility.Visible;

                mainWindow.RootNavigation.Visibility = Visibility.Collapsed;
                mainWindow.Navigate(typeof(GalleryPage));
                mainWindow.uiTitleBar.Visibility = Visibility.Collapsed;
                GalleryPage.GalleryPageCONTEXT.FullScreenButton.Visibility = Visibility.Visible;



                GalleryPage.GalleryPageCONTEXT.MangaGalleryGrid.Background = new SolidColorBrush(Color.FromRgb(16, 16, 18));

                mainWindow.MainWindowRoot.WindowStyle = WindowStyle.None;
                mainWindow.MainWindowRoot.WindowState = WindowState.Maximized;
                mainWindow.WindowCornerPreference = WindowCornerPreference.DoNotRound;

            }
            else {
                GalleryPage.GalleryPageCONTEXT.FullScreenButton.Visibility = Visibility.Collapsed;
                DashboardPage.DashboardPageCONTEXT.FullScreenButton.Visibility = Visibility.Collapsed;
                mainWindow.RootNavigation.Visibility = Visibility.Visible;
                mainWindow.uiTitleBar.Visibility = Visibility.Visible;

                GalleryPage.GalleryPageCONTEXT.MangaGalleryGrid.Background = new SolidColorBrush(Colors.Transparent);

                mainWindow.MainWindowRoot.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.MainWindowRoot.WindowState = WindowState.Normal;
                mainWindow.WindowCornerPreference = WindowCornerPreference.Round;

            }







   






        }

    }
}
