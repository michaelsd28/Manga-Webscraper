using AngleSharp.Dom;
using MongoDB.Bson;
using System.Collections.Generic;
using WPF_MangaScrapper.Models;

namespace WPF_MangaScrapper.Services
{
    internal class GlobalStateService
    {

        public static BsonDocument? _state = new BsonDocument();

        public static Dictionary<string,MangaList>? _MangaList =   new Dictionary<string, MangaList>();

        private static GlobalStateService? globalStateService;

        private GlobalStateService() {   }

        public static Dictionary<string,MangaCaller> ChapterCallerDic { get;  set; } = new Dictionary<string,MangaCaller>();
        public static Dictionary<string, MangaList> ChapterListDic { get;  set; }  = new Dictionary<string, MangaList>();

        public static GlobalStateService GetInstance() {


            if (globalStateService == null)
            { 
                globalStateService = new GlobalStateService();
            }
                
            return globalStateService;

        }

    }
}
