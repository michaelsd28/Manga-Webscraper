using AngleSharp.Dom;
using MongoDB.Bson;

namespace WPF_MangaScrapper.Services
{
    internal class GlobalStateService
    {

        public static BsonDocument? _state;

        private static GlobalStateService? globalStateService;

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
