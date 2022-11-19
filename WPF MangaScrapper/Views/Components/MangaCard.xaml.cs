using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_MangaScrapper.Views.Components
{
    /// <summary>
    /// Interaction logic for MangaCard.xaml
    /// </summary>
    public partial class MangaCard : UserControl
    {


        public string CardColor
        {
            get { return (string)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardColorProperty =
            DependencyProperty.Register("CardColor", typeof(string), typeof(MangaCard), new PropertyMetadata(String.Empty));



        public string BackgroundPoster
        {
            get { return (string)GetValue(BackgroundPosterProperty); }
            set { SetValue(BackgroundPosterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundPoster.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundPosterProperty =
            DependencyProperty.Register("BackgroundPoster", typeof(string), typeof(MangaCard), new PropertyMetadata(String.Empty));



        public string TopIMG
        {
            get { return (string)GetValue(TopIMGProperty); }
            set { SetValue(TopIMGProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopIMG.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopIMGProperty =
            DependencyProperty.Register("TopIMG", typeof(string), typeof(MangaCard), new PropertyMetadata(String.Empty));

        public  string KeyName =  null;
        public MangaCard()
        {
            InitializeComponent();
        }
    }
}
