using System.Resources;
using System.Windows;
using System.Reflection;
using Control = System.Windows.Controls;
using System;
using System.Windows.Navigation;

namespace Chess.App
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainWin { get; set; }

        public static ResourceManager ResourceManager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainWin = this;

            Load("Page/MainMenu.xaml");
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            // Set data
            Title = ((Control.Page)Frame.Content).Title;
            Height = ((Control.Page)Frame.Content).Height;
            Width = ((Control.Page)Frame.Content).Width;
            MinHeight = ((Control.Page)Frame.Content).MinHeight;
            MinWidth = ((Control.Page)Frame.Content).MinWidth;
            MaxHeight = ((Control.Page)Frame.Content).MaxHeight;
            MaxWidth = ((Control.Page)Frame.Content).MaxWidth;
        }

        /// <summary>
        /// Load a page
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            // Navigate
            MainWin.Frame.Source = new Uri(path, UriKind.Relative);
        }
    }
}
