using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Views.Components;
using WPF_MangaScrapper.Views.Pages;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Collections;
using WPF_MangaScrapper.Views.Windows;

namespace WPF_MangaScrapper.Services
{


    internal class DatabaseService
    {

        static string ConnectionString = "mongodb://localhost:6082";

        internal static void GetChaptersDB()
        {


            var searchList = new List<string> { "OnePieceList", "BorutoList", "BokuNoHeroList", "Mushoku" };
            var chaperList = DabaseServiceUTILS.GetInventory(searchList);
            DabaseServiceUTILS.UpdateDashBoard(chaperList);


        }





        public void SaveList() { }


        //internal static void GetChapters()
        //{
        //    throw new NotImplementedException();
        //}

        public static IMongoCollection<BsonDocument> getCollection()

           => DabaseServiceUTILS.MongoCollection();




    }




    internal class DabaseServiceUTILS
    {

        static string ConnectionString = "mongodb://localhost:6082";

        public static IMongoCollection<BsonDocument> MongoCollection()
        {

            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase("MangaWebscrapeDB");
            var collection = database.GetCollection<BsonDocument>("ChapterList");




            return collection;
        }


        public static void UpdateDashBoard(List<MangaList> mangaList) 
        {



            var chapterTitles = new List<string>();



                

            foreach (var manga in mangaList) 
            {


              




             
        

              MangaCard mangaCard = new MangaCard 
                {
                    BackgroundPoster = "https://i.pinimg.com/originals/eb/85/c4/eb85c4376b474030b80afa80ad1cd13a.jpg",
                    CardColor = "#1A0101",
                    TopIMG = "C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\one piece logo.png",
         

              };



                foreach (var title in manga.Titles)
                {
                    var button = new Button { Content = title, Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center };
                    Debug.WriteLine($"title:: {title}");
                    button.Click += NavigateToGallery;
                     mangaCard.ChaptersSTACKPANEL
                        .Children
                        .Add
                        (
                       button
                        );

                }




                DashboardPage
                    .DashboardPageCONTEXT
                    .WrapPanel.Children.Clear();

                DashboardPage
                    .DashboardPageCONTEXT
                    .WrapPanel
                    .Children
                    .Add(mangaCard);

        
            }
            
        }

        private static void NavigateToGallery(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowCONTEXT.Navigate(typeof(GalleryPage));
        }

        public static List<MangaList> GetInventory(List<String> searchList)
        {
            var list = new List<MangaList>();



            foreach (var search in searchList)
            {

                var collection = MongoCollection();
                var filter = new BsonDocument { { "KeyName", $"_{search}" } };

                var current = collection.Find(filter).FirstOrDefault();


                if (current != null)
                {
                    current.Remove("_id");
                    var mangaList = JsonSerializer.Deserialize<MangaList>(current.ToString());
                    list.Add(mangaList);
                }



            }
            return list;
        }


        public static FilterDefinition<BsonDocument> BSONFilter(string key, object value)
        {
            return Builders<BsonDocument>.Filter.Eq(key, value); ;
        }

    }
}
