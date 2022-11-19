using AngleSharp.Dom;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MangaScrapper.Models
{
    class MangaList
    {


        public String? KeyName { get; set; } 
        public IEnumerable<object>? Titles { get; set; }
        public IEnumerable<object>? Links { get; set; }
        public string ColorTheme { get; set; }
        public string PosterLink { get; set; }
        public string LogoIMG { get; set; }

        public MangaList
            (
            string? keyName, 
            IEnumerable<object>? titles, 
            IEnumerable<object>? links, 
            string colorTheme, 
            string posterLink, 
            string logoIMG
            )
        {
            KeyName = keyName;
            Titles = titles;
            Links = links;
            ColorTheme = colorTheme;
            PosterLink = posterLink;
            LogoIMG = logoIMG;
        }
    }
}
