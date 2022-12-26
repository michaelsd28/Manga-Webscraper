using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WPF_MangaScrapper.Views.Components.Gallery;
using WPF_MangaScrapper.Views.Pages;

namespace WPF_MangaScrapper.Models
{
    public class ComboBoxController
    {

        private static  ComboBoxController instance { get; set; }

       public static ComboBoxController GetInstance() {

            if (instance == null) { instance = new ComboBoxController(); }
        
        return instance;
        }

        private ComboBoxController() { }



        // This property represents the ItemsSource property of the underlying ComboBox object
        public IEnumerable ItemsSource
        {
            get { return PageController.PageControllerContext.ControllerComboBox.ItemsSource; }
            set { PageController.PageControllerContext.ControllerComboBox.ItemsSource = value; }
        }

        // This property represents the SelectedIndex property of the underlying ComboBox object
        public int SelectedIndex
        {
            get { return PageController.PageControllerContext.ControllerComboBox.SelectedIndex; }
            set { PageController.PageControllerContext.ControllerComboBox.SelectedIndex = value; }
        }

        // This method unsubscribes from the SelectionChanged event of the underlying ComboBox object
        public void UnsubscribeFromSelectionChanged()
        {
            PageController.PageControllerContext.ControllerComboBox.SelectionChanged -= ComboBoxSelectionChanged;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnsubscribeFromSelectionChanged();
            var combobox = sender as ComboBox;

           

            GalleryPage.GalleryPageCONTEXT.DisplayChapter(combobox.SelectedItem);

        }

        // This method subscribes to the SelectionChanged event of the underlying ComboBox object
        public void SubscribeToSelectionChanged()
        {
            PageController.PageControllerContext.ControllerComboBox.SelectionChanged += ComboBoxSelectionChanged;
        }

        // This method converts an IEnumerable<object> to a List<string>
        public static List<string> ConvertToStringList(IEnumerable<object> inputList)
        {
            return inputList.Select(item => item.ToString()).ToList();
        }

    }

}
