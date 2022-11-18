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



        public MangaList(String? keyName, IEnumerable<object>? titles, IEnumerable<object>? links)
        {
            KeyName = keyName;
            Titles = titles;
            Links = links;
        }
    }
}
