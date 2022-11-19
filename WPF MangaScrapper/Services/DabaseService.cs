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
using System.Threading.Tasks;

namespace WPF_MangaScrapper.Services
{


    internal class DatabaseService
    {

        static string ConnectionString = "mongodb://localhost:6082";

        internal static void GetChaptersDB()
        {

            try {

                var chaperList = DatabaseServiceUTILS.GetListInventory();

                Debug.WriteLine($"GetChaptersDB():: {chaperList.ToJson()}");

                DatabaseServiceUTILS.UpdateDashBoard(chaperList);
            }
            catch (Exception ex) 
                {

                Debug.WriteLine($"GetChaptersDB:: {ex.Message}");
            }

            }

        internal static async void InsertMangaFetcher(MangaCaller mangaFetch)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("ChaptersFetcher");
            await collection.ReplaceOneAsync
                      (
                      filter: new BsonDocument("KeyName", mangaFetch.KeyName),
                      options: new ReplaceOptions { IsUpsert = true },
                      replacement: mangaFetch.ToBsonDocument()
                      );
        }
    }



        //public static IMongoCollection<BsonDocument> GetCollection(string CollecName)

        //   => DatabaseServiceUTILS.MongoCollection(CollecName);



    //internal static async void InsertMangaFetcher(MangaCaller mangaCaller)
    //    {
    //    var collection =    DatabaseServiceUTILS.MongoCollection("ChaptersFetcher");

    //        await collection.ReplaceOneAsync
    //        (
    //        filter: new BsonDocument("KeyName", mangaCaller.KeyName),
    //        options: new ReplaceOptions { IsUpsert = true },
    //        replacement: mangaCaller.ToBsonDocument()
    //        );

    //    }



    }




    internal class DatabaseServiceUTILS
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

            if (manga.Titles == null) return;

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
            MainWindow.mainWindowCONTEXT.Navigate(typeof(GalleryPage));
        }



        public static List<MangaList> GetListInventory()
        {


            var list = new List<MangaList>();
            var collection = MongoCollection("ChapterList");
            var documents = collection.Find(new BsonDocument()).ToList();



            foreach (var document in documents)
            {

      
            
                var filter = new BsonDocument { { "KeyName", document["KeyName"] } };

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

