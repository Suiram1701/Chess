using Chess.Figures;
using Localization;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Chess.App
{
    /// <summary>
    /// Select a figure for farmer transform
    /// </summary>
    public partial class SelectFigureDialog : Window
    {
        #region Localization
        public string L_Title => LangHelper.GetString("SelectFigureDialog.Title");
        public string L_Label => LangHelper.GetString("SelectFigureDialog.Label");
        public string L_Queen => LangHelper.GetString("Figure.Queen");
        public string L_Jumper => LangHelper.GetString("Figure.Jumper");
        public string L_Tower => LangHelper.GetString("Figure.Tower");
        public string L_Bishop => LangHelper.GetString("Figure.Bishop");
        #endregion

        public Type SelectedType { get; private set; }

        public SelectFigureDialog() =>
            InitializeComponent();

        private void Figure_Click(object sender, RoutedEventArgs e)
        {
            // Evaluate buttons
            switch (((Button)sender).Tag)
            {
                case "Queen":
                    SelectedType = typeof(Queen);
                    break;
                case "Jumper":
                    SelectedType = typeof(Jumper);
                    break;
                case "Bishop":
                    SelectedType = typeof(Bishop);
                    break;
                case "Tower":
                    SelectedType = typeof(Tower);
                    break;
            }

            DialogResult = true;
        }
    }
}
