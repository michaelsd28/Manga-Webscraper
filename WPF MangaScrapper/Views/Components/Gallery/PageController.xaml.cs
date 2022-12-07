using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Pages;
using WPF_MangaScrapper.Views.Windows;

namespace WPF_MangaScrapper.Views.Components.Gallery
{
    /// <summary>
    /// Interaction logic for PageController.xaml
    /// </summary>
    public partial class PageController : UserControl
    {


        public static PageController PageControllerContext { get; set; }

        public PageController()
        {
            InitializeComponent();
            PageControllerContext = this;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = ComboBox.SelectedItem;
            GalleryPage.GalleryPageCONTEXT.DisplayChapter(selectedItem);

        }



        private void BGoHome(object sender, RoutedEventArgs e)
        => MainWindow.mainWindowCONTEXT.Navigate(typeof(DashboardPage));


        private async void PreviousButton(object sender, System.Windows.RoutedEventArgs e)
        {
            #region gridfs example
            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);


            //var imgdata = System.IO.File.ReadAllBytes("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\manga1.png");
            //var id = bucket.UploadFromBytes("newIMG.png", imgdata);

            //Debug.WriteLine($"id:: -> {id}");





            //string OnePiece_Chapters = "http://127.0.0.1:5500/Read%20Mushoku%20Tensei%20Manga%20Online%20-%20English%20Scans.html";
            //string BorutoQuery = "div.su-expand-content ul.su-posts-list-loop li a";


            //await WebscrapeService.GetElementsAsync(OnePiece_Chapters, BorutoQuery);

            #endregion

            try
            {


                #region get next chapter

                string mangaTitle = GlobalStateService._state["CurrentManga"].ToString();
                string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
                MangaList mangaList = GlobalStateService._MangaList[mangaKey];
                var titleList = mangaList.Titles.ToList().Select(title => title.ToString()).ToList();
                int index = titleList.IndexOf(mangaTitle);



                var prevTitle = titleList[index + 1];

                #endregion


                GalleryPage.GalleryPageCONTEXT.DisplayChapter(prevTitle);


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PreviusButton:: {ex.Message}");
            }
        }


        private void NextButton(object sender, System.Windows.RoutedEventArgs e)
        {

            #region gridfs example
            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);

            //ObjectId id = new ObjectId("63742278c91007b582fb4436");
            ///63743ccb4707489c9fc9c8b0
            //var byteIMG = bucket.DownloadAsBytes(id);


            //for (int x = 0; x < 20; x++)
            //{
            //    imgStackPanel.Children.Add(new System.Windows.Controls.Image { Source = UtilServices.ByteToBitmapIMG(byteIMG), UseLayoutRounding = true, StretchDirection = StretchDirection.DownOnly });
            //}
            #endregion

            #region get next chapter


            string mangaTitle = GlobalStateService._state["CurrentManga"].ToString();
            string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
            MangaList mangaList = GlobalStateService._MangaList[mangaKey];
            var titleList = mangaList.Titles.ToList().Select(title => title.ToString()).ToList();
            int index = titleList.IndexOf(mangaTitle);


            var nextTitle = titleList[index - 1];


            #endregion


            GalleryPage.GalleryPageCONTEXT.DisplayChapter(nextTitle);


        }



        private async void DisplayInWebview_Click(object sender, RoutedEventArgs e)
        {

            //BsonValue CurrentMangaChapter =  GlobalStateService._state["CurrentMangaChapter"];


            //     Debug.WriteLine($"DisplayInWebview_Click -> CurrentMangaChapter:: {CurrentMangaChapter}");


            var link = "http://127.0.0.1:5503/mushoku%20tensei,%20Chapter%2057%20-%20English%20Scans_files/31.jpg";
          


            var path = @"C:\Users\rd28\Videos\Coding 2022\My Personal Projects\03 - Manga Webscrape  Remastered\WPF MangaScrapper\WPF MangaScrapper\Assets\Webview\index.html";
            


            for (int x = 0; x < 20; x++) {

                string htmlString = File.ReadAllText(path);

                using var client = new HttpClient();

                using var response = await client.GetAsync(link);
                    
                        byte[] imageBytes =  await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        var fileName = "loco"+x+".png";
                      await  File.WriteAllBytesAsync(@"C:\Users\rd28\Videos\Coding 2022\My Personal Projects\03 - Manga Webscrape  Remastered\WPF MangaScrapper\WPF MangaScrapper\Assets\Webview\MangaGallery\"+ fileName, imageBytes);
                        
                        var config = Configuration.Default;
                        using var context = BrowsingContext.New(config);
                        using var doc = await context.OpenAsync(req => req.Content(htmlString));
                        var divContainer = doc.QuerySelector(".galleryReader");

                #region clear container before adding images
                if (x <= 0)
                {
                    divContainer.InnerHtml = "";
                }

                #endregion

                        var wli = doc.CreateElement("img");
                        wli.SetAttribute("src", "./MangaGallery/"+ fileName);
                        wli.SetAttribute("alt", x.ToString());

                 divContainer.AppendChild(wli);
         

                await  File.WriteAllTextAsync(path, doc.ToHtml());




                    
                
            }









            UtilServices.ToggleWebviewScreen();
            Manga_Webview web = (Manga_Webview)GalleryPage.GalleryPageCONTEXT.WebView_CONTAINER.Children[0];
            await web.webView.EnsureCoreWebView2Async();


            web.webView.CoreWebView2.Navigate("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\Webview\\index.html");


        } 





        

        private void SaveToMongoDB_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
