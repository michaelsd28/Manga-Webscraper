using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MangaScrapper.Models
{
    internal class MangaFetch
    {

        public string KeyName { get; set; }
        public string ColorTheme { get; set; }
        public string ChatersLink { get; set; }
        public string GalleryLink { get; set; }
        public string PosterLink { get; set; }
        public string LogoIMG{ get; set;}

        public MangaFetch(string keyName, string colorTheme, string chatersLink, string galleryLink, string posterLink, string logoIMG)
        {
            KeyName = keyName;
            ColorTheme = colorTheme;
            ChatersLink = chatersLink;
            GalleryLink = galleryLink;
            PosterLink = posterLink;
            LogoIMG = logoIMG;
        }
    }
}
