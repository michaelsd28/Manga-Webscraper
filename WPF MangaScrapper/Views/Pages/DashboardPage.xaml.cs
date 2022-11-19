﻿
using LottieSharp.WPF;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Pages;
using WPF_MangaScrapper.Views.Windows;

namespace WPF_MangaScrapper.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public static DashboardPage DashboardPageCONTEXT { get; set; }  
        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {

            InitializeComponent();
            ViewModel = viewModel;
            DashboardPageCONTEXT = this;
            MangaScrapperUTILS.FullScreenINIT(this);
            DatabaseService.GetChaptersDB();





        }
        LottieAnimationView? lottie = null;
        private async void BRefreshCH(object sender, System.Windows.RoutedEventArgs e)
        {

                lottie = MangaScrapperUTILS.LottieAnimation("Assets/Animation/rocket 2.json");
                WrapPanel.Visibility = Visibility.Collapsed;
     

                GridContent.Children.Add(lottie);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += OnDoWorkAsync;
                worker.RunWorkerCompleted += OnRunWorkerCompletedAsync;
                worker.RunWorkerAsync();
                

        }

        private  void OnRunWorkerCompletedAsync(object? sender, RunWorkerCompletedEventArgs e)
        {
       
            WrapPanel.Visibility = Visibility.Visible;
            GridContent.Children.Remove(lottie);
        }

        private async void  OnDoWorkAsync(object? sender, DoWorkEventArgs e)
        {

            await WebscrapeService.UpdateChapterList();
     
            Application.Current.Dispatcher.Invoke(delegate
            {
                DatabaseService.GetChaptersDB();
            });


        }
    }

}
public class MangaScrapperUTILS
{


    public static LottieAnimationView LottieAnimation(string fileName) {


        var lottie = new LottieAnimationView {
            Width = 180,
            Height = 180,
            HorizontalAlignment = HorizontalAlignment.Center ,
            VerticalAlignment =  VerticalAlignment.Center,
            AutoPlay =  true,
            FileName = fileName,
            RepeatCount = -1,
            Name = "LottieAnimation"
        };


        return lottie;



    }

    public static void FullScreenINIT(DashboardPage DashboardPageCONTEXT)
    {

        bool isFullScreen = MainWindow.mainWindowCONTEXT.RootNavigation.Visibility == Visibility.Collapsed;
        if (isFullScreen)
        {
            DashboardPageCONTEXT.FullScreenButton.Visibility = Visibility.Visible;
        }
    }

    internal static void FetchChapters(DashboardPage dashboardPage)
    {
        
    }
}