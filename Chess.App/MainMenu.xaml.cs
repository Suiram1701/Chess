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
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Local_Click(object sender, RoutedEventArgs e)
        {
            new ChessGame().Show();
            Close();
        }
    }
}
