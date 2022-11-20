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
using WPF_MangaScrapper.Services;
using LottieSharp.WPF;

namespace WPF_MangaScrapper.Services
{


    internal class DatabaseService
    {

        static string ConnectionString = "mongodb://localhost:6082";

        internal static MangaCaller GetCaller(string key, string value)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("ChaptersFetcher");
            var filter = DatabaseServiceUTILS.BSONFilter( key,  value);
            var callerBSON =  collection.Find(filter).First();

            //Debug.WriteLine($"GetCaller:: {callerBSON.ToJson()}");
            callerBSON.Remove("_id");
            var caller = JsonSerializer.Deserialize<MangaCaller>(callerBSON.ToString());

            return caller;
        }

        internal static void GetChaptersDB()
        {

            try
            {

                var chaperList = DatabaseServiceUTILS.GetListInventory();

                DatabaseServiceUTILS.UpdateDashBoard(chaperList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception ex -> GetChaptersDB:: {ex.Message}");
            }

        }

        internal static  MangaChapter GetMangaChapter(object title)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("Mangas");
    
            var chapterColl =  collection.Find(new BsonDocument("Title", title.ToString())).FirstOrDefault();

            MangaChapter? chapter = null;
            if (chapterColl!= null)
            {
                chapterColl.Remove("_id");
                chapter = JsonSerializer.Deserialize<MangaChapter?>(chapterColl.ToString());
            }
       
      

     
           
            return chapter;
        }



        internal static async Task InsertMangaChapterAsync(MangaChapter chapter)
        {
           var collection = DatabaseServiceUTILS.MongoCollection("Mangas");
            var filter = DatabaseServiceUTILS.BSONFilter("MangaKey", chapter.MangaKey);

            await collection.ReplaceOneAsync
          (
          filter: filter,
          options: new ReplaceOptions { IsUpsert = true },
          replacement: chapter.ToBsonDocument()
          );
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
            //add list to global state
            GlobalStateService.ChapterListDic[manga.KeyName] = manga;

         

            MangaCard mangaCard = new MangaCard
            {

                BackgroundPoster = manga.PosterLink,
                CardColor = manga.ColorTheme,
                TopIMG = manga.LogoIMG,
                KeyName = manga.KeyName

            };



            #region add buttons to card

            if (manga.Titles == null) return;

            foreach (object title in manga.Titles)
            {
       
                var button = new System.Windows.Controls.Button 
                { 
                    Content = title, 
                    Margin = new Thickness(10), 
                    HorizontalAlignment = HorizontalAlignment.Center 
                };
                GlobalStateService._state["CurrentKey"] = manga.KeyName;
                button.Click += (sender, EventArgs) => { NavigateToGallery(sender, EventArgs, manga, title); };
                mangaCard.ChaptersSTACKPANEL.Children.Add(button);
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

    private static void NavigateToGallery(object sender, RoutedEventArgs e, MangaList manga, object title)
    {

        MainWindow.mainWindowCONTEXT.Navigate(typeof(GalleryPage));
        GalleryPage.GalleryPageCONTEXT.DisplayChapter(title);
    }



    public static List<MangaList> GetListInventory()
    {

 

        var list = new List<MangaList>();
        var collection = MongoCollection("ChapterList");
        var documents = collection.Find(new BsonDocument()).ToList();

        var state = GlobalStateService.ChapterListDic;



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

