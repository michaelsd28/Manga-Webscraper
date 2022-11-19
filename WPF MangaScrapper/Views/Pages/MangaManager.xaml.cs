using ColorPicker;
using MongoDB.Bson;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using WPF_MangaScrapper.Models;
using WPF_MangaScrapper.Services;
using WPF_MangaScrapper.Views.Windows;


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
        public Wpf.Ui.Controls.MessageBox? MessageBox { get => messageBox; set => messageBox = value; }

        public MangaManager(ViewModels.DashboardViewModel viewModel)
        {

            InitializeComponent();
            ViewModel = viewModel;
            MangaManagerCONTEXT = this;



        }

        #region color picker


        Wpf.Ui.Controls.MessageBox? messageBox = null;
        StandardColorPicker? standardColorPicker = null;
        private void BOpenColorPicker(object sender, System.Windows.RoutedEventArgs e)
        {

            MessageBox = new Wpf.Ui.Controls.MessageBox();
            MessageBox.Width = 500;
            MessageBox.Height = 500;


            MessageBox.ButtonLeftName = "Pick Color";
            MessageBox.ButtonRightName = "Just close me";

            MessageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
            MessageBox.ButtonRightClick += MessageBox_RightButtonClick;

            standardColorPicker = new StandardColorPicker();
            standardColorPicker.Name = "main";
            standardColorPicker.Width = 100;
            standardColorPicker.Height = 100;

            MessageBox.Show("Pick a color", standardColorPicker);

        }


        private void MessageBox_RightButtonClick(object sender, RoutedEventArgs e)
            => MessageBox.Close();


        private void MessageBox_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            String hexColor = ManagerUTILS.GetHexValue(standardColorPicker);

            TBoxColor.Text = hexColor;
            MessageBox.Close();
        }

        #endregion





        internal class ManagerUTILS {


            public static string GetHexValue(StandardColorPicker standardColorPicker) 
            {
                String hexColor = standardColorPicker.SelectedColor.ToBrush().ToString();


                hexColor = hexColor.Substring(3);

                Debug.WriteLine($"GetHexValue:: {hexColor}");

                return "#"+hexColor;
            }
        
        }

        private void SaveButton(object sender, RoutedEventArgs e)
            => new MangaManagerUTILS().SaveNewManga(this);

  

            
        

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowCONTEXT.Navigate(typeof(DashboardPage));
        }


        internal class MangaManagerUTILS {

            Wpf.Ui.Controls.MessageBox? messageBox = null;

            public  void SaveNewManga(MangaManager mangaManager) 
            {

                if (
                    mangaManager.TboxKey.Text == string.Empty ||
                    mangaManager.TBoxColor.Text == string.Empty ||
                    mangaManager.TBoxChapers.Text == string.Empty ||
                    mangaManager.TBoxGallery.Text == string.Empty ||
                    mangaManager.TBoxPoster.Text == string.Empty ||
                    mangaManager.TBoxLogo.Text == string.Empty
                    )
                {
                    QuickMessage("Please enter data", "Please press ok to continue").Show();
                    return;
                }
                else 
                {
                    MangaFetch mangaFetch = new MangaFetch
                      (
                      keyName: mangaManager.TboxKey.Text,
                      colorTheme: mangaManager.TBoxColor.Text,
                      chatersLink: mangaManager.TBoxChapers.Text,
                      galleryLink: mangaManager.TBoxGallery.Text,
                      posterLink: mangaManager.TBoxPoster.Text,
                      logoIMG: mangaManager.TBoxLogo.Text
                      );

                    DatabaseService.SaveMangaFetch(mangaFetch);
                    QuickMessage("Succefully saved","Please press ok to continue").Show();
                }
 
            }

            private void CloseMessageBox(object sender, RoutedEventArgs e)
                => messageBox.Close();



            public Wpf.Ui.Controls.MessageBox QuickMessage(string Title,string Message) 
            {

                messageBox = new Wpf.Ui.Controls.MessageBox();
                messageBox.Width = 350;
                messageBox.Height = 250;


                messageBox.ButtonLeftName = "Click to continue";


                messageBox.ButtonLeftClick += CloseMessageBox;
                messageBox.ButtonRightClick += CloseMessageBox;


                messageBox.Title = Title;
                messageBox.Content = Message;

                return messageBox;

          

            }
        }
    }
}
