using AngleSharp.Dom;
using MongoDB.Bson;
using System.Collections.Generic;
using WPF_MangaScrapper.Models;

namespace WPF_MangaScrapper.Services
{
    internal class GlobalStateService
    {

        public static  BsonDocument _state = new BsonDocument();

        public static  Dictionary<string, MangaList>? _MangaList { get; set; } = new Dictionary<string, MangaList>();

        public static GlobalStateService globalStateService { get; set; }  

        public static  Dictionary<string, MangaList> ChapterListDic { get; set; } = new Dictionary<string, MangaList> ();



        private GlobalStateService() { }
        public static GlobalStateService GetInstance() {

      

            if (globalStateService == null)
            { 
                globalStateService = new GlobalStateService();
            }
                
            return globalStateService;

        }

    }
}
