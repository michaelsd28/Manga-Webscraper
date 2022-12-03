
//String onePieceUrl = "https://readtcbscans.com/mangas/5/one-piece";
//String BorutoURL = "https://ww1.read-boruto.online/manga/";
//String BokuNoURL = "https://muheroacademia.com/";


using LottieSharp.WPF;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Windows;
using System.Windows.Controls;
using System.Linq;
using WPF_MangaScrapper.Views.Components.Gallery;
/// chapters
namespace WPF_MangaScrapper.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class GalleryPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public static GalleryPage GalleryPageCONTEXT { get; set; }

        object? mangaTitle = null;
        public GalleryPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            GalleryPageCONTEXT = this;



            Debug.WriteLine("********GalleryPage initialized********");
        }



        private void BGoHome(object sender, RoutedEventArgs e)
            => MainWindow.mainWindowCONTEXT.Navigate(typeof(DashboardPage));






        LottieAnimationView? lottie = null;

        internal async void DisplayChapter(object title)
        {

            try
            {

                Debug.WriteLine($"DisplayChapter -> title:: {title}");

                //TBlockMangaTitle.Text = title.ToString();
                mangaTitle = title;
                GlobalStateService._state["CurrentManga"] = title.ToString();


                #region check button status

                string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
                MangaList mangaList = GlobalStateService._MangaList[mangaKey];
                var titleList = mangaList.Titles.ToList();


                //boundCapacity = titleList.Count; 


                Helper.CheckButtonStatus();

                #endregion

                #region add titles to combobox

                //ComboBox.ItemsSource = titleList;
                int index = titleList.IndexOf(mangaTitle);

                #endregion

                #region add lottie animation 
                lottie = UtilServices.LottieAnimation("Assets/Animation/book read.json");
                lottie.Width = 200;
                lottie.Height = 200;
                GalleryGRID.Visibility = Visibility.Collapsed;
                ContentGRID.Children.Add(lottie);
                #endregion

                #region background worker
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += OnDoWorkAsync;
                worker.RunWorkerCompleted += OnRunWorkerCompletedAsync;
                worker.RunWorkerAsync();

                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception ex:: {ex.Message}       ***DisplayChapter***");
            }
        }




        private void OnRunWorkerCompletedAsync(object? sender, RunWorkerCompletedEventArgs e)
        {
            ContentGRID.Children.Remove(lottie);
            GalleryGRID.Visibility = Visibility.Visible;


        }

        private async void OnDoWorkAsync(object? sender, DoWorkEventArgs e)
        {

            Task.Delay(500).Wait();

            MangaChapter chapter = await DatabaseService.GetMangaChapter(mangaTitle);


            var GalleryLinks = chapter.GalleryLinks;


            var invokenumbers = 0;

            var dispatcher = Application.Current.Dispatcher;




            await dispatcher.BeginInvoke(() =>
               {
                   Helper.AddImageToStack(galleryLinks: chapter.GalleryLinks, GalleryGRID: galleryGRID);
               }
               );


        }







        internal class Helper
        {


            public static void CheckButtonStatus()
            {

                //Button prev, Button next, int boundIndex, int boundCapacity





                #region get list
                string mangaTitle = GlobalStateService._state["CurrentManga"].ToString();
                string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
                MangaList mangaList = GlobalStateService._MangaList[mangaKey];
                var titleList = mangaList.Titles.ToList().Select(title => title.ToString()).ToList();
                int index = titleList.IndexOf(mangaTitle);
                var listLimitIndex = index - 1;
                var listCapacity = titleList.Count;


          
                #endregion



                Debug.WriteLine($"CheckButtonStatus -> index:: {index}   ***  listLimitIndex:: {listLimitIndex}   ***  listCapacity:: {listCapacity}");





                if (listLimitIndex == -1)
                {
                    PageController.PageControllerContext.BNext.IsEnabled = false;
                }
                else
                {
                    PageController.PageControllerContext.BNext.IsEnabled = true;
                }




                if (index == listCapacity-1)
                {
                    PageController.PageControllerContext.BPrev.IsEnabled = false;

                }
                else
                {
                    PageController.PageControllerContext.BPrev.IsEnabled = true;
                }






            }




            internal static void AddImageToStack(IEnumerable<object>? galleryLinks, Grid GalleryGRID)
            {

                StackPanel stackPanel = new StackPanel();


                GalleryGRID.Children.Clear();




                foreach (var link in galleryLinks)
                {

                    Debug.WriteLine($"AddImageToStack -> link:: {link}    ****  {galleryLinks.Count()}");
                    if (link != null && !link.ToString().Contains("./"))
                    {

                        var image = new Image
                        {
                            StretchDirection = StretchDirection.DownOnly,



                            UseLayoutRounding = true,
                            Margin = new Thickness(10),


                        };



                        var fullFilePath = link.ToString();

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                        bitmap.EndInit();

                        image.Source = bitmap;


                        stackPanel.Children.Add(image);



                    }


                }

                GalleryGRID.Children.Add(stackPanel);
            }
        }
    }
}