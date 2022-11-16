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

namespace WPF_MangaScrapper.Services
{
    internal class WebscrapeService
    {

        #region variables for chapters
        readonly string OnePiece_Chapters = "http://127.0.0.1:5500/One%20Piece.html";
        readonly string OnepQuery = "body > div.container.px-3.mx-auto > div > div.w-full.md\\:w-2\\/3.px-2 > div:nth-child(n) > div > div.col-span-3 > a";

        readonly string Boruto_Chapters = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations.html";
        readonly string BorutoQuery = "body > div.wrap > div > div.site-content > div > div.c-page-content.style-1 > div > div > div > div > div > div > div > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";

        readonly string BokuNoHero_Chapters = "http://127.0.0.1:5500/One%20Piece.html";
        readonly string BokuNoHeroQuery = "#manga-chapters-holder > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";
        #endregion

        #region variables for gallery
        readonly string OnePiece_Base= "http://127.0.0.1:5500/One%20Piece.html";
        readonly string OnepGallery_Query = "body > div.container.px-3.mx-auto > div > div.w-full.md\\:w-2\\/3.px-2 > div:nth-child(n) > div > div.col-span-3 > a";

        readonly string Boruto_Base = "http://127.0.0.1:5500/Boruto_%20Naruto%20Next%20Generations.html";
        readonly string BorutoGallery_Query = "body > div.wrap > div > div.site-content > div > div.c-page-content.style-1 > div > div > div > div > div > div > div > div.page-content-listing.single-page > div > ul > li:nth-child(n) > div:nth-child(1) > a";

        readonly string BokuNoHero_Base = "http://127.0.0.1:5500/One%20Piece.html";
        readonly string BokuGallery_Query = "body > div.container.px-3.mx-auto > div > div.w-full.md\\:w-2\\/3.px-2 > div:nth-child(n) > div > div.col-span-3 > a";
        #endregion



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
