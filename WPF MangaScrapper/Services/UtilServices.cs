

using AngleSharp;
using LottieSharp.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using WPF_MangaScrapper.Views.Components.Gallery;
using WPF_MangaScrapper.Views.Pages;
using WPF_MangaScrapper.Views.Windows;
using Path = System.IO.Path;

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
    // Create a local variable to store the header visibility
    var headerVisibility = GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility;

    // Check the header visibility
    if (headerVisibility == System.Windows.Visibility.Visible)
    {
        // Hide the header and show the web view
        GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility = Visibility.Hidden;
        GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Visibility = Visibility.Visible;

        // Show the page controller window and toggle full screen mode
        GalleryPage.PageController_Window = new PageController_Window();
        GalleryPage.PageController_Window.Show();
        ToggleFullScreen(MainWindow.mainWindowCONTEXT);

        // Hide the gallery grid and the page controller grid
        GalleryPage.GalleryPageCONTEXT.galleryGRID.Visibility = Visibility.Collapsed;
        GalleryPage.GalleryPageCONTEXT.PageController_GRID.Visibility = Visibility.Collapsed;
    }
    else
    {
        // Show the header and hide the web view
        GalleryPage.GalleryPageCONTEXT.Header_GRID.Visibility = Visibility.Visible;
        GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Visibility = Visibility.Collapsed;

        // Close the page controller window and toggle full screen mode
        GalleryPage.PageController_Window.Close();
        ToggleFullScreen(MainWindow.mainWindowCONTEXT);

        // Show the gallery grid and the page controller grid
        GalleryPage.GalleryPageCONTEXT.galleryGRID.Visibility = Visibility.Visible;
        GalleryPage.GalleryPageCONTEXT.PageController_GRID.Visibility = Visibility.Visible;
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

        internal static async Task WriteToWebGallery(IEnumerable<object>? galleryLinks)
        {



            string currentDir = Directory.GetCurrentDirectory();
            string htmlFilePath = Path.Combine(currentDir, "Assets", "Webview", "index.html");
            string ImagefilePath = Path.Combine(currentDir, "Assets", "Webview", "MangaGallery");





            for (int x = 0; x < galleryLinks.Count(); x++)
            {

                var currentImageLink = galleryLinks.ElementAt(x).ToString();

                Debug.WriteLine($"mangaLinks[{x}]:: {currentImageLink}");

                #region download imaga and write to file

                using var client = new HttpClient();
                using var response = await client.GetAsync(currentImageLink);
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                string fileName = "loco" + x + ".png";
                await File.WriteAllBytesAsync(ImagefilePath + "\\" + fileName, imageBytes);

                #endregion


                #region get div container from html
                string htmlString = File.ReadAllText(htmlFilePath);
                var config = AngleSharp.Configuration.Default;
                using var context = BrowsingContext.New(config);
                using var doc = await context.OpenAsync(req => req.Content(htmlString));
                var divContainer = doc.QuerySelector(".galleryReader");


                /// h1 title
                var h1Title = doc.QuerySelector(".mangaTitle");


                var mangaTitle = (string) GlobalStateService._state["CurrentManga"];
                h1Title.TextContent = mangaTitle;



                #endregion


                #region clear container before adding images
                if (x <= 0)
                {
                    divContainer.InnerHtml = "";
                }

                #endregion


                #region create image element and add it to html

                var imageElement = doc.CreateElement("img");
                imageElement.SetAttribute("src", "./MangaGallery/" + fileName);
                imageElement.SetAttribute("alt", x.ToString());

                divContainer.AppendChild(imageElement);


                await File.WriteAllTextAsync(htmlFilePath, doc.ToHtml());

                #endregion




            }


           await ReloadWebview();


        }

        internal static async Task ReloadWebview()
        {

   

            await GalleryPage.GalleryPageCONTEXT.Dispatcher.InvokeAsync(() =>
            {
                Manga_Webview web = (Manga_Webview)GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Children[0];
                web.webView.EnsureCoreWebView2Async();

                string currentDir = Directory.GetCurrentDirectory();
                string htmlFilePath = Path.Combine(currentDir, "Assets", "Webview", "index.html");
                web.webView.CoreWebView2.Navigate(htmlFilePath);
            });


            //Manga_Webview web = (Manga_Webview)GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Children[0];
            //await web.webView.EnsureCoreWebView2Async();

            //string currentDir = Directory.GetCurrentDirectory();
            //string htmlFilePath = Path.Combine(currentDir, "Assets", "Webview", "index.html");
            //web.webView.CoreWebView2.Navigate(htmlFilePath);
        }
    }
}
