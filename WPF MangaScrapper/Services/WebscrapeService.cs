using AngleSharp.Dom;
using AngleSharp;
using MongoDB.Bson;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Windows.Documents;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using WPF_MangaScrapper.Views.Windows;
using Wpf.Ui.Appearance;

namespace WPF_MangaScrapper.Services
{
    internal class WebscrapeService
    {

        #region variables for chapters


        readonly string Musholu_Chapters = "http://127.0.0.1:5500/Read%20Mushoku%20Tensei%20Manga%20Online%20-%20English%20Scans.html";
        readonly string MushokuQuery = "div.su-expand-content ul.su-posts li.su-post a";

        readonly string OnePiece_Chapters = "http://127.0.0.1:5500/One%20Piece%20_%20TCB%20Scans.html";
        readonly string OnepQuery = "body > main > div.overflow-hidden > div > div > div.col-span-2 > a:nth-child(n) > div.text-lg.font-bold";

        readonly string Boruto_Chapters = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations.html";
        readonly string BorutoQuery = "body > div.wrap > div > div.site-content > div > div.c-page-content.style-1 > div > div > div > div > div > div > div > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";

        readonly string BokuNoHero_Chapters = "http://127.0.0.1:5500/Read%20My%20Hero%20Academia%20Manga%20Online%20[FREE]%E2%9C%85.html";
        readonly string BokuNoHeroQuery = "#manga-chapters-holder > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";


        #endregion

        #region variables for gallery


        readonly string Musholu_Base = "http://127.0.0.1:5501/mushoku%20tensei,%20Chapter%2057%20-%20English%20Scans.html";
        readonly string MushokuGallery_Query = "body > main > div.overflow-hidden > div > div > div.col-span-2 > a:nth-child(n) > div.text-lg.font-bold";

        readonly string OnePiece_Base= "http://127.0.0.1:5500/One%20Piece%20Chapter%201066%20_%20TCB%20Scans.html";
        readonly string OnepGallery_Query = "body > div.container.px-3.mx-auto > div > div.w-full.md\\:w-2\\/3.px-2 > div:nth-child(n) > div > div.col-span-3 > a";

        readonly string Boruto_Base = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations,%20Chapter%2074%20-%20Boruto_%20Naruto%20Next%20Generations%20Manga%20Online.html";
        readonly string BorutoGallery_Query = "div.entry-content img[src]";

        readonly string BokuNoHero_Base = "http://127.0.0.1:5500/My%20Hero%20Academia,%20Chapter%20372%20-%20My%20Hero%20Academia%20Manga%20Online.html";
        readonly string BokuGallery_Query = ".entry-content div.separator img";
        #endregion

        //.su-expand-content ul.su-posts li.su-post a

        #region GetElements
        public async Task<IEnumerable<IElement>> GetElementsAsync(string url,string query) {


            //Create a new context for evaluating webpages with the default config
            IBrowsingContext context = BrowsingContext.New(Configuration.Default);

            //Create a document from a virtual request / response pattern
            IDocument document = await context.OpenAsync(req => req.Content(GetWebContent(url)));

            //Or directly with CSS selectors
            IHtmlCollection<IElement> elementsSelected = document.QuerySelectorAll(query);

            foreach (var item in elementsSelected) {
                Debug.WriteLine($"item:: {item.OuterHtml}");
            }

            return elementsSelected.Take(50);
        }
        #endregion




        #region getContent
        private string? GetWebContent(string url) {

            Debug.WriteLine($"GetWebContent:: {url}");
        

            string? content = null;

            using (WebClient client = new WebClient())
            {
                content  = client.DownloadString(url);
            }
          
      
          

            return content;
        }
        #endregion

    }
}
