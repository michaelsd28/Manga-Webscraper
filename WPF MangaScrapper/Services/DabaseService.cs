﻿using MongoDB.Bson;
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
using Wpf.Ui.Mvvm.Services;

namespace WPF_MangaScrapper.Services
{


    internal class DatabaseService
    {

        static string ConnectionString = "mongodb://localhost:6082";

        internal static async Task<MangaCaller>  GetCaller(string key, string value)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("ChaptersFetcher");
            var filter = DatabaseServiceUTILS.BSONFilter(key, value);
            var callerBSON = await collection.Find(filter).FirstAsync();

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
                chaperList.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception ex -> GetChaptersDB:: {ex.Message}");
            }

        }

        internal static async Task<MangaChapter>  GetMangaChapter(object title)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("Mangas");

            if (title == null) {

                title = GlobalStateService._state["CurrentManga"].ToString();
          
            }

            var chapterColl = await collection.Find(new BsonDocument("Title", title?.ToString())).FirstOrDefaultAsync();


            MangaChapter? chapter = null;
            if (chapterColl != null)
            {
                chapterColl.Remove("_id");
                chapter = JsonSerializer.Deserialize<MangaChapter?>(chapterColl.ToString());
            
            }
            else 
            {

                string currentKey = (string)GlobalStateService._state["CurrentKey"];
                MangaCaller mangaCaller = await GetCaller("KeyName", currentKey);
                chapter = await WebscrapeService.ScrapeManga(mangaCaller, title);




    

   
            }

            return chapter;

        }



        internal static async Task InsertMangaChapterAsync(MangaChapter chapter)
        {
            var collection = DatabaseServiceUTILS.MongoCollection("Mangas");
            var filter = DatabaseServiceUTILS.BSONFilter("Title", chapter.Title);

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

   
            UtilServices.QuickMessageBox("Saved Succefully", "Please press ok to continue");
        }


    }






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


        #region clear container before adding 

        DashboardPage
     .DashboardPageCONTEXT
     .WrapPanel.Children.Clear();
        #endregion



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
        
                button.Click += (sender, EventArgs) => { NavigateToGallery(sender, EventArgs, manga, title); };
                mangaCard.ChaptersSTACKPANEL.Children.Add(button);
            }



            #endregion

            #region clear and add cards



            DashboardPage
                .DashboardPageCONTEXT
                .WrapPanel
                .Children
                .Add(mangaCard);

            #endregion

        }

    }



    private static void NavigateToGallery(object sender, RoutedEventArgs e, MangaList manga, object title)
    {

        GlobalStateService._state["CurrentManga"] = title.ToString();
        GlobalStateService._state["CurrentKey"] = manga.KeyName;

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
                GlobalStateService._MangaList[mangaList.KeyName] = mangaList;
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

