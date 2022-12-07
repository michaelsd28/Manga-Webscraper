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
            this.PageController_Component = GalleryPage.GalleryPageCONTEXT.PageController_Component;
        }
    }
}
