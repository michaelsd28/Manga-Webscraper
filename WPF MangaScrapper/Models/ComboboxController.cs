using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Components.Gallery;
using WPF_MangaScrapper.Views.Pages;

namespace WPF_MangaScrapper.Models
{
    public class ComboBoxController
    {
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

  
                var combobox = sender as ComboBox;

                //// prevent performance issues
                combobox.SelectionChanged -= ComboBoxSelectionChanged;
            //// end



           
                GalleryPage.GalleryPageCONTEXT.DisplayChapter(combobox.SelectedItem);

            


        }

        // This method subscribes to the SelectionChanged event of the underlying ComboBox object
        public void SubscribeToSelectionChanged()
        {
            PageController.PageControllerContext.ControllerComboBox.SelectionChanged += ComboBoxSelectionChanged;
        }
    }

}
