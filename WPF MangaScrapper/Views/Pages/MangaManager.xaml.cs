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
        Wpf.Ui.Controls.MessageBox? messageBox = null;
        StandardColorPicker? standardColorPicker = null;
        private void BOpenColorPicker(object sender, System.Windows.RoutedEventArgs e)
        {
            //< colorpicker:StandardColorPicker x:Name = "main" Width = "100" Height = "100" />
            messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Width = 500;
            messageBox.Height = 500;


            messageBox.ButtonLeftName = "Pick Color";
            messageBox.ButtonRightName = "Just close me";

            messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            standardColorPicker= new StandardColorPicker();
            standardColorPicker.Name = "main";
            standardColorPicker.Width= 100; 
            standardColorPicker.Height= 100;

            messageBox.Show("Pick a color", standardColorPicker);

        }

        private void MessageBox_RightButtonClick(object sender, RoutedEventArgs e)
        {
            messageBox.Close();
        }

        private void MessageBox_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            String hexColor = ManagerUTILS.GetHexValue(standardColorPicker); 

            TextBoxColor.Text = hexColor;
            messageBox.Close();
        }


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
        {

        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
