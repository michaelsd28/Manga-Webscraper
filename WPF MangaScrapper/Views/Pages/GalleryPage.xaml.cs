﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Diagnostics;
using Wpf.Ui.Common.Interfaces;
using System.Windows.Controls;
using WPF_MangaScrapper.Services;

/// chapters
//String onePieceUrl = "https://ww8.readonepiece.com/chapter/";
//String BorutoURL = "https://ww1.read-boruto.online/manga/";
//String BokuNoURL = "https://muheroacademia.com/";





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

        private  async void PreviusButton(object sender, System.Windows.RoutedEventArgs e)
        {

            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);


            //var imgdata = System.IO.File.ReadAllBytes("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\manga1.png");
            //var id = bucket.UploadFromBytes("newIMG.png", imgdata);

            //Debug.WriteLine($"id:: -> {id}");


            var webscraper = new WebscrapeService();

             string OnePiece_Chapters = "http://127.0.0.1:5500/Read%20My%20Hero%20Academia%20Manga%20Online%20[FREE]%E2%9C%85.html";
            string BorutoQuery = "#manga-chapters-holder > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";


            await webscraper.GetElementsAsync(OnePiece_Chapters, BorutoQuery);





        }




        private void NextButton(object sender, System.Windows.RoutedEventArgs e)
        {

            MongoClient client = new MongoClient("mongodb://localhost:6082");
            IMongoDatabase database = client.GetDatabase("Images");
            IGridFSBucket bucket = new GridFSBucket(database);



          
            
            ObjectId id =  new ObjectId("63742278c91007b582fb4436");
            ///63743ccb4707489c9fc9c8b0
            var byteIMG = bucket.DownloadAsBytes(id);



            for (int x = 0; x<20; x++) {
                imgStackPanel.Children.Add(new Image { Source = UtilServices.ByteToBitmapIMG(byteIMG), Width = 300 });
            }

           




        }
    }
}