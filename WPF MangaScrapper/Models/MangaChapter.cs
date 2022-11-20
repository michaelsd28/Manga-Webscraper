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
        public object Title { get; set; }
        public object Link { get; set; }
        public IEnumerable<object>? GalleryLinks { get; set; }

        public MangaChapter(string? mangaKey, object title, object link, IEnumerable<object>? galleryLinks)
        {
            MangaKey = mangaKey;
            Title = title;
            Link = link;
            GalleryLinks = galleryLinks;
        }
    }
}
