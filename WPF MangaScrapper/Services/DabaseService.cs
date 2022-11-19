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
using WPF_MangaScrapper.Views.Windows;
using System.Diagnostics;
using Wpf.Ui.Controls;

namespace WPF_MangaScrapper.Services
{


    internal class DatabaseService
    {

        static string ConnectionString = "mongodb://localhost:6082";

        internal static void GetChaptersDB()
        {

            try 
            { 

            } catch (Exception ex) 
            {
                Debug.Write($"Exception GetChaptersDB {ex.Message}");
            }

            var searchList = new List<string> { "OnePieceList", "BorutoList", "BokuNoHeroList", "Mushoku" };
            var chaperList = DabaseServiceUTILS.GetListInventory(searchList);
            DabaseServiceUTILS.UpdateDashBoard(chaperList);


        }




        public static IMongoCollection<BsonDocument> getCollection(string CollecName)

           => DabaseServiceUTILS.MongoCollection(CollecName);

        internal static void SaveMangaFetch(MangaFetch mangaFetch)
        {
            return;
        }
    }




    internal class DabaseServiceUTILS
    {

        static string ConnectionString = "mongodb://localhost:6082";

        public static IMongoCollection<BsonDocument> MongoCollection(string collecName)
        {

            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase("MangaWebscrapeDB");
            var collection = database.GetCollection<BsonDocument>(collecName);




            return collection;
        }


        public static void UpdateDashBoard(List<MangaList> mangaList) 
        {


            foreach (var manga in mangaList) 
            {


              MangaCard mangaCard = new MangaCard 
                {

                    BackgroundPoster = "https://i.pinimg.com/originals/eb/85/c4/eb85c4376b474030b80afa80ad1cd13a.jpg",
                    CardColor = "#1A0101",
                    TopIMG = "/Assets/one piece logo.png",
                    KeyName = manga.KeyName
             
              };

         

                #region add buttons to card

                foreach (object title in manga.Titles)
                {

                    var button = new System.Windows.Controls.Button { Content = title, Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center };
                    button.Click += (sender, EventArgs) => { NavigateToGallery(sender, EventArgs, manga); };
                    mangaCard.ChaptersSTACKPANEL.Children.Add (  button );
                }

                

                #endregion

                #region clear and add cards

                DashboardPage
                    .DashboardPageCONTEXT
                    .WrapPanel.Children.Clear();

                DashboardPage
                    .DashboardPageCONTEXT
                    .WrapPanel
                    .Children
                    .Add(mangaCard);

                #endregion

            }

        }

        //private static RoutedEventHandler NavigateToGallery(string? keyName)
        //{
           
        //}

        private static void NavigateToGallery(object sender, RoutedEventArgs e, MangaList manga)
        {
            Debug.WriteLine($"manga:: {manga.ToJson()}");
            MainWindow.mainWindowCONTEXT.Navigate(typeof(GalleryPage));
        }



        public static List<MangaList> GetListInventory(List<String> searchList)
        {


            var list = new List<MangaList>();



            foreach (var search in searchList)
            {

                var collection = MongoCollection("ChapterList");
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
