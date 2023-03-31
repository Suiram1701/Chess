using System.Windows;

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
