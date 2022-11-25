
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
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media;
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
        int boundIndex = 0;
        int boundCapacity = 0;

        public GalleryPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            GalleryPageCONTEXT = this;

           

            Debug.WriteLine("********GalleryPage initialized********");
        }

        private async void PreviousButton(object sender, System.Windows.RoutedEventArgs e)
        {


            #region gridfs example
            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);


            //var imgdata = System.IO.File.ReadAllBytes("C:\\Users\\rd28\\Videos\\Coding 2022\\My Personal Projects\\03 - Manga Webscrape  Remastered\\WPF MangaScrapper\\WPF MangaScrapper\\Assets\\manga1.png");
            //var id = bucket.UploadFromBytes("newIMG.png", imgdata);

            //Debug.WriteLine($"id:: -> {id}");





            //string OnePiece_Chapters = "http://127.0.0.1:5500/Read%20Mushoku%20Tensei%20Manga%20Online%20-%20English%20Scans.html";
            //string BorutoQuery = "div.su-expand-content ul.su-posts-list-loop li a";


            //await WebscrapeService.GetElementsAsync(OnePiece_Chapters, BorutoQuery);

            #endregion


            try
            {


                NavigationService.Navigate(null);
                NavigationService.Navigate(typeof (GalleryPage));
                #region get previous chapter



                string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
                MangaList mangaList = GlobalStateService._MangaList[mangaKey];
                var titleList = mangaList.Titles.ToList();
                int index = titleList.IndexOf(mangaTitle);
                boundIndex = index + 1;
                boundCapacity = titleList.Count;
                var prevTitle = titleList[index + 1];

                #endregion



                Helper.CheckButtonStatus(BPrev, BNext, boundIndex, boundCapacity);
                DisplayChapter(prevTitle);


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PreviusButton:: {ex.Message}");
            }
        }





        private void NextButton(object sender, System.Windows.RoutedEventArgs e)
        {

            #region gridfs example
            //MongoClient client = new MongoClient("mongodb://localhost:6082");
            //IMongoDatabase database = client.GetDatabase("Images");
            //IGridFSBucket bucket = new GridFSBucket(database);

            //ObjectId id = new ObjectId("63742278c91007b582fb4436");
            ///63743ccb4707489c9fc9c8b0
            //var byteIMG = bucket.DownloadAsBytes(id);


            //for (int x = 0; x < 20; x++)
            //{
            //    imgStackPanel.Children.Add(new System.Windows.Controls.Image { Source = UtilServices.ByteToBitmapIMG(byteIMG), UseLayoutRounding = true, StretchDirection = StretchDirection.DownOnly });
            //}
            #endregion



            #region get next chapter



            string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
            MangaList mangaList = GlobalStateService._MangaList[mangaKey];
            var titleList = mangaList.Titles.ToList();
            int index = titleList.IndexOf(mangaTitle);
            boundIndex = index - 1;
            boundCapacity = titleList.Count;

            var nextTitle = titleList[index - 1];
            #endregion



            Helper.CheckButtonStatus(BPrev, BNext, boundIndex, boundCapacity);
            DisplayChapter(nextTitle);


        }

        private void BGoHome(object sender, RoutedEventArgs e)

            => MainWindow.mainWindowCONTEXT.Navigate(typeof(DashboardPage));



        LottieAnimationView? lottie = null;

        internal async void DisplayChapter(object title)
        {
    






            TBlockMangaTitle.Text = title.ToString();
            mangaTitle = title;

            #region add titles to combobox
            string mangaKey = GlobalStateService._state["CurrentKey"].ToString();
            MangaList mangaList = GlobalStateService._MangaList[mangaKey];
            var titleList = mangaList.Titles.ToList();
            ComboBox.ItemsSource = titleList;

         
            int index = titleList.IndexOf(mangaTitle);
            ComboBox.SelectedIndex = index;
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




        private void OnRunWorkerCompletedAsync(object? sender, RunWorkerCompletedEventArgs e)
        {
            ContentGRID.Children.Remove(lottie);
            GalleryGRID.Visibility = Visibility.Visible;
            

        }

        private async void OnDoWorkAsync(object? sender, DoWorkEventArgs e)
        {

            Task.Delay(2000).Wait();

            MangaChapter chapter = await DatabaseService.GetMangaChapter(mangaTitle);



            Debug.WriteLine($"chapter != null - >>> {chapter} *** {chapter != null}");


            var GalleryLinks = chapter.GalleryLinks;


            var invokenumbers = 0;

           var  dispatcher = Application.Current.Dispatcher;




         await   dispatcher.BeginInvoke(() => 
            {
                Helper.AddImageToStack(galleryLinks: chapter.GalleryLinks, GalleryGRID: galleryGRID);
           
            }
            );

       


           



        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = ComboBox.SelectedItem;
            DisplayChapter(selectedItem);

        }
    }




    internal class Helper
    {


        public static void CheckButtonStatus(Button prev, Button next, int boundIndex, int boundCapacity)
        {
            next.IsEnabled = boundIndex != 0;
            prev.IsEnabled = boundIndex < boundCapacity - 1;
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
                        UseLayoutRounding = true,
                        StretchDirection = StretchDirection.DownOnly,
                        Margin = new Thickness(20)
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