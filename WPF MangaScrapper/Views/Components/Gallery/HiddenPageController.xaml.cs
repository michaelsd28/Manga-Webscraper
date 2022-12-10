using System.Linq;
using System.Windows.Controls;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Pages;

namespace WPF_MangaScrapper.Views.Components.Gallery
{
    /// <summary>
    /// Interaction logic for HiddenPageController.xaml
    /// </summary>
    public partial class HiddenPageController : UserControl
    {
        public HiddenPageController()
        {
            InitializeComponent();


            //string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
            //MangaList mangaList = GlobalStateService._MangaList[mangaKey];
            //var titleList = mangaList.Titles.ToList();


            #region add titles to combobox

            //var title =   GlobalStateService._state["CurrentManga"].ToString();

            //PageController_Hidden.TBlockMangaTitle.Text = title;

            //PageController_Hidden.ComboBox.ItemsSource = titleList;
            //int indexCombobox = titleList.IndexOf(title);



            #endregion
        }
    }
}
