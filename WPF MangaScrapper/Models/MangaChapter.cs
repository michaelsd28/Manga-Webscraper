using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MangaScrapper.Models
{
    class MangaChapter
    {
        public string? MangaKey { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public List<String>? GalleryLinks { get; set; }

    }
}
