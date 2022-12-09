using System.Windows;
using System.Windows.Controls;
using WPF_MangaScrapper.Views.Components.Gallery;

namespace WPF_MangaScrapper.Views.Windows
{
    /// <summary>
    /// Interaction logic for PageController_Window.xaml
    /// </summary>
    public partial class PageController_Window : Window
    {
        public PageController_Window()
        {
            InitializeComponent();
            HiddenPageController.PageController_Hidden = PageController.PageControllerContext;
        }
    }
}
