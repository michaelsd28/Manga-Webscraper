using System;
using System.IO;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using WPF_MangaScrapper.Views.Pages;
using WPF_MangaScrapper.Views.Windows;
using LottieSharp.WPF;


namespace WPF_MangaScrapper.Services
{
    internal class UtilServices
    {

        public static void QuickMessageBox(string title , string content) {

            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Title = title;
            messageBox.Content = content;
            messageBox.ButtonLeftClick += messageBoxClose;
            messageBox.ButtonLeftName = "OK";
            messageBox.ButtonRightClick += messageBoxClose;
            messageBox.Show();

        }

        private static void messageBoxClose(object sender, RoutedEventArgs e)
        {
            var messageBox = (Wpf.Ui.Controls.MessageBox)sender;
            messageBox.Close();
        }

        public static LottieAnimationView LottieAnimation(string fileName)
        {


            var lottie = new LottieAnimationView
            {
                Width = 180,
                Height = 180,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                AutoPlay = true,
                FileName = fileName,
                RepeatCount = -1,
                Name = "LottieAnimation"
            };


            return lottie;



        }

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

        public static void ToggleWebviewScreen() 
        {
            var headerVisibility = GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility;

            if (headerVisibility == Visibility.Visible)
            {
                GalleryPage.GalleryPageCONTEXT.galleryGRID.Visibility = Visibility.Collapsed;
                GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility = Visibility.Hidden;
                GalleryPage.PageController_Window = new PageController_Window();
                GalleryPage.PageController_Window.Show();

                ToggleFullScreen(MainWindow.mainWindowCONTEXT);
                GalleryPage.GalleryPageCONTEXT.PageController_GRID.Visibility = Visibility.Collapsed;
                GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Visibility = Visibility.Visible;

            }
            else
            {
                GalleryPage.GalleryPageCONTEXT.galleryGRID.Visibility = Visibility.Visible;
                GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility = Visibility.Visible;
                GalleryPage.PageController_Window.Close();
                ToggleFullScreen(MainWindow.mainWindowCONTEXT);
                GalleryPage.GalleryPageCONTEXT.PageController_GRID.Visibility = Visibility.Visible;
                GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Visibility = Visibility.Collapsed;


            }

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



                GalleryPage.GalleryPageCONTEXT.ContentGRID.Background = new SolidColorBrush(Color.FromRgb(16, 16, 18));

                mainWindow.MainWindowRoot.WindowStyle = WindowStyle.None;
                mainWindow.MainWindowRoot.WindowState = WindowState.Maximized;
                mainWindow.WindowCornerPreference = WindowCornerPreference.DoNotRound;

            }
            else {

                GalleryPage.GalleryPageCONTEXT.FullScreenButton.Visibility = Visibility.Collapsed;
                DashboardPage.DashboardPageCONTEXT.FullScreenButton.Visibility = Visibility.Collapsed;
                mainWindow.RootNavigation.Visibility = Visibility.Visible;
                mainWindow.uiTitleBar.Visibility = Visibility.Visible;

                GalleryPage.GalleryPageCONTEXT.ContentGRID.Background = new SolidColorBrush(Colors.Transparent);

                mainWindow.MainWindowRoot.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.MainWindowRoot.WindowState = WindowState.Normal;
                mainWindow.WindowCornerPreference = WindowCornerPreference.Round;

                GalleryPage.PageController_Window.Close();


            }







   






        }

    }
}
