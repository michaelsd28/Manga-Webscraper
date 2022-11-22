using AngleSharp.Dom;
using AngleSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using WPF_MangaScrapper.Models;
using System;
using System.Threading;
using System.Text.Json;
using MongoDB.Driver.Core.Misc;

namespace WPF_MangaScrapper.Services
{
    internal class WebscrapeService
    {

        #region variables for chapters


        readonly static string Mushoku_Chapters = "http://127.0.0.1:5500/Read%20Mushoku%20Tensei%20Manga%20Online%20-%20English%20Scans.html";
        readonly static string MushokuQuery = "div.su-expand-content ul.su-posts-list-loop li a";

        readonly static string OnePiece_Chapters = "http://127.0.0.1:5500/One%20Piece%20_%20TCB%20Scans.html";
        readonly static string OnepQuery = "body > main > div.overflow-hidden > div > div > div.col-span-2 > a:nth-child(n) > div.text-lg.font-bold";

        readonly static string Boruto_Chapters = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations.html";
        readonly static string BorutoQuery = "body > div.wrap > div > div.site-content > div > div.c-page-content.style-1 > div > div > div > div > div > div > div > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";

        readonly static string BokuNoHero_Chapters = "http://127.0.0.1:5500/Read%20My%20Hero%20Academia%20Manga%20Online%20[FREE]%E2%9C%85.html";
        readonly static string BokuNoHeroQuery = "#manga-chapters-holder > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";


        #endregion

        #region variables for gallery


        readonly static string Mushoku_Base = "http://127.0.0.1:5501/mushoku%20tensei,%20Chapter%2057%20-%20English%20Scans.html";
        readonly static string MushokuGallery_Query = "body > main > div.overflow-hidden > div > div > div.col-span-2 > a:nth-child(n) > div.text-lg.font-bold";

        readonly static string OnePiece_Base = "http://127.0.0.1:5500/One%20Piece%20Chapter%201066%20_%20TCB%20Scans.html";
        readonly static string OnepGallery_Query = "body > div.container.px-3.mx-auto > div > div.w-full.md\\:w-2\\/3.px-2 > div:nth-child(n) > div > div.col-span-3 > a";

        readonly static string Boruto_Base = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations,%20Chapter%2074%20-%20Boruto_%20Naruto%20Next%20Generations%20Manga%20Online.html";
        readonly static string BorutoGallery_Query = "div.entry-content img[src]";

        readonly static string BokuNoHero_Base = "http://127.0.0.1:5500/My%20Hero%20Academia,%20Chapter%20372%20-%20My%20Hero%20Academia%20Manga%20Online.html";
        readonly static string BokuGallery_Query = ".entry-content div.separator img";
        #endregion

        //.su-expand-content ul.su-posts li.su-post a

        #region GetElements
        public static async Task<IEnumerable<object>?> GetElementsAsync(string url, string query, string? attribute = null)
        {
            Debug.WriteLine($"GetElementsAsync:: -> url:: {url} -> query:: {query} -> attribute:: {attribute}");

            IEnumerable<String?>? elementsSelected = null;

            try
            {

                //Create a new context for evaluating webpages with the default config
                IBrowsingContext context = BrowsingContext.New(Configuration.Default);

                //Create a document from a virtual request / response pattern

                string content = GetWebContent(url);

                IDocument document = await context.OpenAsync(req => req.Content(content));





                // directly with CSS selectors
                //IHtmlCollection<IElement> elementsSelected = document.QuerySelectorAll(query);

                if (attribute == null)
                {
                    elementsSelected = document.QuerySelectorAll(query).Select(m => m.TextContent).Take(50);
                }
                else
                {
                    elementsSelected = document.QuerySelectorAll(query).Select(m => m.GetAttribute(attribute)).Take(50);
                }




                foreach (var element in elementsSelected)
                {
                    Debug.WriteLine($"GetElementsAsync element:: {element}");
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetElementsAsync:: {ex.Message}  **** url:: {url} -> query:: {query} -> attribute:: {attribute}");
            }

            return elementsSelected;
        }
        #endregion


        public static async Task UpdateChapterList1()
        {

            #region get manga callers from db

            var callerCollection = DatabaseServiceUTILS.MongoCollection("ChaptersFetcher");
            var documents = callerCollection.Find(new BsonDocument()).ToList();
            var mangaCallerList = new List<MangaCaller>();

            foreach (var document in documents)
            {

                document.Remove("_id");
                var current = JsonSerializer.Deserialize<MangaCaller>(document.ToString());

                mangaCallerList.Add(current);
            }

            #endregion





            foreach (var mangaCaller in mangaCallerList)
            {
                var titles = await GetElementsAsync(mangaCaller.ChatersLink, mangaCaller.ChapterQuery);
                var callerLinks = await GetElementsAsync(mangaCaller.ChatersLink, mangaCaller.ChapterQuery, "href");

                MangaList? mangaList = new MangaList
                    (
                   keyName: mangaCaller.KeyName,
                   titles: titles,
                    links: callerLinks,
                    colorTheme: mangaCaller.ColorTheme,
                    posterLink: mangaCaller.PosterLink,
                    logoIMG: mangaCaller.LogoIMG
                    );

                var fetchCollection = DatabaseServiceUTILS.MongoCollection("ChapterList");

          

                await fetchCollection.ReplaceOneAsync
                    (
                    filter: new BsonDocument("KeyName", mangaCaller.KeyName),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: mangaList.ToBsonDocument()
                    );
            }





        }

        internal static async Task  FetchMangaAsync(MangaCaller mangaCaller, object title)
        {
            string KeyName = (string)GlobalStateService._state["CurrentKey"];
            //// get manga list from db
            MangaList? mangaList = GlobalStateService.ChapterListDic[KeyName];
            var titleList = mangaList.Titles.ToList();
            int index = titleList.IndexOf(title);
            var mangaLinks = mangaList.Links.ToList();
            var link = mangaLinks[index].ToString();


            Debug.WriteLine
                (
                $"FetchMangaAsync:: index-> {index} * " +
                $"title:: {title.ToString()} * " +
                $"mangaList.Titles:: {mangaList.Titles.ToList()[0]} * " +
                $"mangaLinks:: {mangaLinks.ToJson()} * " +
                //$"mangaList-> {mangaList.ToJson()} * " +
                //$"link-> {link} * " +
                //$"mangaList.Titles.ToList()-> {mangaList.Titles.ToList().ToJson()}" +
                $""
                );

            var GalleryLinks = await GetElementsAsync
                (
                url: link,
                query: mangaCaller.GalleryQuery,
                attribute: "src"
                );



            MangaChapter chapter = new MangaChapter
                (
                mangaKey: KeyName,
                title: title.ToString(),
                link: link,
                galleryLinks: GalleryLinks
                );

            //Debug.WriteLine($"FetchMangaAsync:: {chapter.ToJson()}");

            await DatabaseService.InsertMangaChapterAsync(chapter);


        }

  



        #region getContent
        private static string GetWebContent(string url)
        {

            string? content = null;
            try
            {

                Debug.WriteLine($"GetWebContent:: {url}");




                using (WebClient client = new WebClient())
                {
                    content = client.DownloadString(url);
                }


            }
            catch (Exception ex)
            {

                Debug.WriteLine($"GetWebContent:: {ex.Message}");

                return "";
            }

            return content;
        }


        #endregion

    }
}
