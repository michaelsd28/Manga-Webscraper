using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Windows;

namespace WPF_MangaScrapper.Views.Components
{
    /// <summary>
    /// Interaction logic for FullScreenButton.xaml
    /// </summary>
    public partial class FullScreenButton : UserControl
    {
   
        public FullScreenButton()
        {
            InitializeComponent();
       
        }

        private void BFullScreen(object sender, RoutedEventArgs e)
        {
            UtilServices.ToggleFullScreen(MainWindow.mainWindowCONTEXT);
        }
    }
}
