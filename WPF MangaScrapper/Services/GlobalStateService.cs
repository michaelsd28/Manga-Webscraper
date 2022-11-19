using AngleSharp.Dom;
using MongoDB.Bson;
using System.Collections.Generic;
using WPF_MangaScrapper.Models;

namespace WPF_MangaScrapper.Services
{
    internal class GlobalStateService
    {

        public static BsonDocument? _state;

        private static GlobalStateService? globalStateService;

        private GlobalStateService() { }

        public static Dictionary<string,MangaCaller> ChapterCallerDic { get;  set; }

        public static GlobalStateService GetInstance() {


            if (globalStateService == null)
            { 
                globalStateService = new GlobalStateService();
            }
                
            return globalStateService;

        }

    }
}
