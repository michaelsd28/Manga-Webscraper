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
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WPF_MangaScrapper.Services
{
    internal class WebscrapeService
    {



        static string RemoveSpaces(string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        #region GetElements
        public static async Task<IEnumerable<object>?> GetElementsAsync(string url, string query, string? attribute = null)
        {
            Debug.WriteLine($"GetElementsAsync:: -> url:: {url} -> query:: {query} -> attribute:: {attribute}");

            // gets the url 
            Uri uri = new Uri(url);
            //gets the domain and domain will be "www.example.com"
            string baseUrl = uri.Scheme + "://" + uri.Host + ":" + uri.Port;

            Debug.WriteLine($"baseUrl:: {baseUrl}");
            

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
                    elementsSelected = document.QuerySelectorAll(query).Select(m => RemoveSpaces(m.TextContent.Replace("\n", "")).Trim()   ).Take(50);
                
                else
                    elementsSelected = document.QuerySelectorAll(query)
                              .Select(element => element.GetAttribute(attribute))
                              .Take(50)
                              .Select(element =>
                              {
                                  Debug.WriteLine($"Processing element: {element}");

                                  // Check if the element is a relative URL (i.e. it doesn't have a domain)
                                  if (!element.StartsWith("http"))
                                  {
                                      // If the element is a relative URL, add the base URL as the domain
                                      string updatedElement = baseUrl + element;
                                      return updatedElement;
                                  }
                                  else
                                  {
                                      return element;
                                  }
                              });




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

        internal static async Task<MangaChapter>  ScrapeManga(MangaCaller mangaCaller, object title)
        {


            #region scraping is here
            string KeyName = (string)GlobalStateService._state["CurrentKey"];

            MangaList? mangaList = GlobalStateService.ChapterListDic[KeyName];

            var titles = mangaList.Titles.Select(title => title.ToString()).ToList();
            var indexTitle = titles.IndexOf(title.ToString());
            var mangaLinkList = mangaList.Links.ToList();


            Debug.WriteLine($"ScrapeManga ->  mangaLinkList[indexTitle]==title:: {mangaLinkList[1] == title.ToString()} " +
                $"mangaLinkList[1] = {mangaLinkList[1]}   ***  title = {title} " +
                $"indexTitle:: {indexTitle}" +
                $"");
            var mangaLink = mangaLinkList[indexTitle];
      
            var GalleryLinks = await GetElementsAsync(url: mangaLink.ToString(), query: mangaCaller.GalleryQuery, "src");
            #endregion
  

            MangaChapter chapter = new MangaChapter
                (
                mangaKey: KeyName,
                title: title.ToString(),
                link: mangaLink,
                galleryLinks: GalleryLinks
                );


             await DatabaseService.InsertMangaChapterAsync(chapter);


            return chapter;

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
