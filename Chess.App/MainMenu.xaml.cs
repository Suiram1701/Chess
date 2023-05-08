using Chess.App.Files;
using Localization;
using Microsoft.Win32;
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
        public string L_Load => LangHelper.GetString("MainMenu.Load");
        #endregion

        public MainMenu() =>
            InitializeComponent();

        private void Local_Click(object sender, RoutedEventArgs e)
        {
            new ChessGame().Show();
            Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = LangHelper.GetString("MainMenu.Load"),
                Filter = "Chess file (*.chess)|*.chess",
                CheckFileExists = true,
            };
            if (dialog.ShowDialog() == true)
            {
                ChessFile file = ChessFile.Load(dialog.FileName);
                if (file == null)     // Null if not validated
                    return;

                if (!file.Veryfy())
                {
                    MessageBox.Show(LangHelper.GetString("MainMenu.Mani"), LangHelper.GetString("MainMenu.ManiTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                new ChessGame(file).Show();
                Close();
            }
        }
    }
}
