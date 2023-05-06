using Localization;
using System.Windows;

namespace Chess.App
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        #region Localization
        public string L_Title => LangHelper.GetString("MainMenu.Title");
        public string L_Label => LangHelper.GetString("MainMenu.Label");
        public string L_STBtn => LangHelper.GetString("MainMenu.STBtn");
        #endregion

        public MainMenu() =>
            InitializeComponent();

        private void Local_Click(object sender, RoutedEventArgs e)
        {
            new ChessGame().Show();
            Close();
        }
    }
}
