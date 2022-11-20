﻿
//String onePieceUrl = "https://readtcbscans.com/mangas/5/one-piece";
//String BorutoURL = "https://ww1.read-boruto.online/manga/";
//String BokuNoURL = "https://muheroacademia.com/";





using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Windows;
/// chapters
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

        public static GalleryPage GalleryPageCONTEXT { get; set; }
        public GalleryPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            GalleryPageCONTEXT = this;

            Debug.WriteLine("********GalleryPage initialized********");
        }

        private async void PreviusButton(object sender, System.Windows.RoutedEventArgs e)
        {

            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);


            //var imgdata = System.IO.File.ReadAllBytes("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\manga1.png");
            //var id = bucket.UploadFromBytes("newIMG.png", imgdata);

            //Debug.WriteLine($"id:: -> {id}");





            //string OnePiece_Chapters = "http://127.0.0.1:5500/Read%20Mushoku%20Tensei%20Manga%20Online%20-%20English%20Scans.html";
            //string BorutoQuery = "div.su-expand-content ul.su-posts-list-loop li a";


            //await WebscrapeService.GetElementsAsync(OnePiece_Chapters, BorutoQuery);


        }




        private void NextButton(object sender, System.Windows.RoutedEventArgs e)
        {

            MongoClient client = new MongoClient("mongodb://localhost:6082");
            IMongoDatabase database = client.GetDatabase("Images");
            IGridFSBucket bucket = new GridFSBucket(database);





            ObjectId id = new ObjectId("63742278c91007b582fb4436");
            ///63743ccb4707489c9fc9c8b0
            var byteIMG = bucket.DownloadAsBytes(id);


            for (int x = 0; x < 20; x++)
            {
                imgStackPanel.Children.Add(new System.Windows.Controls.Image { Source = UtilServices.ByteToBitmapIMG(byteIMG), UseLayoutRounding = true, StretchDirection = StretchDirection.DownOnly });
            }

        }

        private void BGoHome(object sender, RoutedEventArgs e)

            => MainWindow.mainWindowCONTEXT.Navigate(typeof(DashboardPage));




        internal async void DisplayChapter(object title)
        {

            MangaChapter? chapter = DatabaseService.GetMangaChapter(title);
            await Helper.HandleNull(chapter, title);

            var GalleryLinks = chapter.GalleryLinks;





            foreach (var link in GalleryLinks)
            {

                Debug.WriteLine($"GalleryLinks:: {link}");
                if (link != null && !link.ToString().Contains("./"))
                {


                    imgStackPanel.Children.Add(
         new Image
         {
             Source = new BitmapImage { UriSource = new Uri(link.ToString()) },
             UseLayoutRounding = true,
             StretchDirection = StretchDirection.DownOnly
         }
         );

                }

            }
        }



    }


    internal class Helper
    {



        public static async Task HandleNull(MangaChapter chapter, object title)
        {
            if (chapter == null)
            {
                string currentKey = (string)GlobalStateService._state["CurrentKey"];
                MangaCaller mangaCaller = DatabaseService.GetCaller("KeyName", currentKey);
                await WebscrapeService.FetchMangaAsync(mangaCaller, title);
                chapter = DatabaseService.GetMangaChapter(title);

            }
        }

    }
}