using System.Collections.Generic;
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


            #region add titles to combobox

            string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
            MangaList mangaList = GlobalStateService._MangaList[mangaKey];

           var currentTitle =  GlobalStateService._state["CurrentManga"] ;



            // Create an IEnumerable of objects
            var titleList = mangaList.Titles.ToList();

            // Convert the IEnumerable to a List of strings using the ConvertToStringList method
            List<string> stringTitleList = ComboBoxController.ConvertToStringList(titleList);

            // Create a ComboBox object
            ComboBoxController comboBoxController = ComboBoxController.GetInstance();

            // Unsubscribe from the SelectionChanged event before changing the SelectedIndex
            comboBoxController.UnsubscribeFromSelectionChanged();

            // Use the ComboBox object to access the properties and methods of the underlying ComboBox object
            comboBoxController.ItemsSource = stringTitleList;

            // assign title to controller
            PageController_Hidden.TBlockMangaTitle.Text = currentTitle.ToString();


            int indexTitle = stringTitleList.IndexOf(currentTitle.ToString());
            comboBoxController.SelectedIndex = indexTitle;

            // Resubscribe to the SelectionChanged event
            comboBoxController.SubscribeToSelectionChanged();


            #endregion
        }
    }
}
