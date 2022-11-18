using Wpf.Ui.Common.Interfaces;

namespace WPF_MangaScrapper.Views.Pages
{
    /// <summary>
    /// Interaction logic for MangaManager.xaml
    /// </summary>
    public partial class MangaManager : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public static MangaManager MangaManagerCONTEXT { get; set; }
        public MangaManager(ViewModels.DashboardViewModel viewModel)
        {

            InitializeComponent();
            ViewModel = viewModel;
            MangaManagerCONTEXT = this;



        }
    }
}
