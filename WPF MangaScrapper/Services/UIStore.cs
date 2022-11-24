
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_MangaScrapper.Services
{
    internal class UIStore
    {

        public static UIStore? instance { get; set; } = null;

        public static Dictionary<string, BitmapImage> NavICONS { get; set; }  = new Dictionary<string, BitmapImage> 
        {
            {"BorutoICON", new BitmapImage (new Uri("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\Components\\boruto icon.png")) },
            {"GalleryICON", new BitmapImage (new Uri("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\Dashboard\\galleryIcon.png")) },
            {"ManagerICON", new BitmapImage (new Uri("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\Dashboard\\magicStick icon.png")) },
        }; 

        private UIStore() { }

        public UIStore GetInstance()
        {

            if (instance == null)
            {
                instance = new UIStore();
            }
            return instance;

        }


    }
}
